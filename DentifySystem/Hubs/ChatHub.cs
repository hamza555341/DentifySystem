using AutoMapper;
using Domain.Entites.ChatModule;
using Domain.Entites.Notifications;
using Domain.Entites.TreatmentRequestModule;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;
using Service.Abstraction;
using Service.Specifications.CaseSpecifications;
using Service.Specifications.TreatmentRequestSpecificaition;
using Shared.DTOs.NotificationsDTO;
using System.Text.RegularExpressions;

namespace DentifySystem.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ILogger<TreatmentRequest> _logger;
        private readonly INotificationHubService _notificationHubService;
        

        public ChatHub(IUnitOfWork unitOfWork,INotificationService notificationService,ILogger<TreatmentRequest>logger, INotificationHubService notificationHubService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _logger = logger;
            _notificationHubService = notificationHubService;
            
        }

        public override async Task OnConnectedAsync()
        {
            var requestId = Context.GetHttpContext()?.Request.Query["requestId"];

            if (string.IsNullOrEmpty(requestId))
            {
                Context.Abort();
                return;
            }

            var request = await _unitOfWork.GetRepository<TreatmentRequest,int>()
                .GetByIdAsync(new TreatmentRequestWithDetailsSpecification(int.Parse(requestId!)));

            if (request is null || request.Status != TreatmentRequestStatus.Accepted)
            {
                Context.Abort();
                return;
            }

            var identityUserId = Context.UserIdentifier;

            var isPatient = request.Case.Patient.IdentityUserId == identityUserId;
            var isStudent = request.Student.IdentityUserId == identityUserId;

            if (!isPatient && !isStudent)
            {
                Context.Abort();
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"chat_{requestId}");

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(int requestId, string content)
        {
            var identityUserId = Context.UserIdentifier!;

            var request = await _unitOfWork
            .GetRepository<TreatmentRequest, int>()
            .GetByIdAsync( new TreatmentRequestWithDetailsSpecification(requestId));

            if (request is null)
            {
                return;
            }

            var receiverId = identityUserId == request.Student.IdentityUserId ? request.Case.Patient.IdentityUserId: request.Student.IdentityUserId;

            var message = new ChatMessage
            {
                TreatmentRequestId = requestId,
                SenderId = identityUserId,
                Content = content,
                IsRead = false
            };

            await _unitOfWork.GetRepository<ChatMessage,int>().AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            var notificationResult =
                await _notificationService.CreateNotificationAsync(
                    receiverId,
                    identityUserId,
                    NotificationType.Message,
                    request.Id);

            if (notificationResult.IsFailure)
            {
                _logger.LogError(
                    "Failed to create notification for request {RequestId}",
                    request.Id);
            }
            else
            {
                var n = notificationResult.Value;
                await _notificationHubService.SendAsync(
                    receiverId,
                    n.Title,
                    n.Message,
                    (int)n.Type,
                    n.ReferenceId,
                    n.CreatedAt);
            }
            

            await Clients.Group($"chat_{requestId}").SendAsync("ReceiveMessage", new
            {
                message.Id,
                message.SenderId,
                message.Content,
                message.MediaUrl,
                message.IsRead,
                message.CreatedAt
            });
        }
    }
}
