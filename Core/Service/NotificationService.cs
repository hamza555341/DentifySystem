using AutoMapper;
using Domain.Entites.IdentityModule;
using Domain.Entites.Notifications;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Service.Abstraction;
using Service.Specifications.NotificationSpecifications;
using Shared.CommonResult;
using Shared.DTOs.NotificationsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<NotificationResponseDTO>> CreateNotificationAsync(
    string receiverId,
    string senderId,
    NotificationType type,
    int? referenceId = null)
        {
            var sender = await _userManager.FindByIdAsync(senderId);

            if (sender is null)
            {
                return Result<NotificationResponseDTO>.Failure(
                    Error.NotFound("Sender.NotFound"));
            }

            var senderName = string.IsNullOrWhiteSpace(sender.DisplayName)
                ? sender.UserName!
                : sender.DisplayName;

            var (title, message) = BuildNotificationContent(type, senderName);

            var notification = new Notification
            {
                recieverId = receiverId,
                SenderId = senderId,
                Title = title,
                Message = message,
                Type = type,
                ReferenceId = referenceId,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork
                .GetRepository<Notification, int>()
                .AddAsync(notification);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                return Result<NotificationResponseDTO>.Failure(
                    Error.Failure("Notification.SaveFailed"));
            }

            var dto = _mapper.Map<NotificationResponseDTO>(notification);

            return Result<NotificationResponseDTO>.Ok(dto);
        }

        private static (string title, string message) BuildNotificationContent(NotificationType type, string senderName)
        {
            return type switch
            {
                NotificationType.Request =>
                (
                    "New Request",
                    $"{senderName} sent you a request."
                ),

                NotificationType.RequestAccepted =>
                (
                    "Request Accepted",
                    $"{senderName} accepted your request."
                ),

                NotificationType.RequestRejected =>
                (
                    "Request Rejected",
                    $"{senderName} rejected your request."
                ),

                NotificationType.Message =>
                (
                    "New Message",
                    $"{senderName} sent you a new message."
                ),
                NotificationType.AppointmentProposed =>
                (
                    "Appointment Proposal",
                    $"{senderName} proposed appointment times."
                ),

                NotificationType.AppointmentConfirmed =>
                (
                    "Appointment Confirmed",
                    $"{senderName} confirmed the appointment."
                ),

                _ =>
                (
                    "Notification",
                    "You have a new notification."
                )
            };
        }

            public async Task<Result<IEnumerable<NotificationResponseDTO>>> GetMyNotificationsAsync(string receiverId)
        {
            var notifications = await _unitOfWork
                .GetRepository<Notification, int>()
                .GetAllAsync(new NotificationByReceiverSpecification(receiverId));

            var result = _mapper.Map<IEnumerable<NotificationResponseDTO>>(notifications);

            return Result<IEnumerable<NotificationResponseDTO>>.Ok(result);
        }
        public async Task<Result> MarkAsReadAsync(int notificationId, string receiverId)
        {
            var notification = await _unitOfWork
                .GetRepository<Notification, int>()
                .GetByIdAsync(new NotificationByIdSpecification(notificationId));

            if (notification is null)
                return Error.NotFound("Notification.NotFound");

            if (notification.recieverId != receiverId)
                return Error.Unauthorized("Notification.Unauthorized");

            if (!notification.IsRead)
            {
                notification.IsRead = true;

                _unitOfWork
                    .GetRepository<Notification, int>()
                    .Update(notification);

                await _unitOfWork.SaveChangesAsync();
            }

            return Result.Ok();
        }

        public async Task<Result> MarkAllAsReadAsync(string receiverId)
        {
            var notifications = await _unitOfWork
                .GetRepository<Notification, int>()
                .GetAllAsync(new NotificationByReceiverSpecification(receiverId));

            foreach (var notification in notifications)
            {
                if (!notification.IsRead)
                {
                    notification.IsRead = true;

                    _unitOfWork
                        .GetRepository<Notification, int>()
                        .Update(notification);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result<int>> GetUnreadCountAsync(string receiverId)
        {
            var count = await _unitOfWork
                .GetRepository<Notification, int>()
                .CountAsync(new NotificationUnreadSpecification(receiverId));

            return Result<int>.Ok(count);
        }
    }
}
     



   

