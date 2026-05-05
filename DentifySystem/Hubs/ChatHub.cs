using Domain.Entites.ChatModule;
using Domain.Entites.TreatmentRequestModule;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Service.Specifications.CaseSpecifications;
using Service.Specifications.TreatmentRequestSpecificaition;
using System.Text.RegularExpressions;

namespace DentifySystem.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            var message = new ChatMessage
            {
                TreatmentRequestId = requestId,
                SenderId = identityUserId,
                Content = content,
                IsRead = false
            };

            await _unitOfWork.GetRepository<ChatMessage,int>().AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

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
