using Domain.Entites.Notifications;
using Shared.CommonResult;
using Shared.DTOs.NotificationsDTO;

namespace Service.Abstraction
{
    public interface INotificationService
    {
        Task<Result<NotificationResponseDTO>> CreateNotificationAsync(
            string receiverId,
            string senderId,
            NotificationType type,
            int? referenceId = null);

        Task<Result<IEnumerable<NotificationResponseDTO>>> GetMyNotificationsAsync(string receiverId);

        Task<Result> MarkAsReadAsync(int notificationId, string receiverId);

        Task<Result> MarkAllAsReadAsync(string receiverId);

        Task<Result<int>> GetUnreadCountAsync(string receiverId);
    }
}