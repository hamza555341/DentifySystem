using Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;

namespace DentifySystem.Services
{
    public class NotificationHubService : INotificationHubService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHubService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendAsync(
            string receiverId,
            string title,
            string message,
            int notificationType,
            int? referenceId,
            DateTime createdAt)
        {
            await _hubContext.Clients
                .Group($"notification_{receiverId}")
                .SendAsync("ReceiveNotification", new
                {
                    title,
                    message,
                    notificationType,
                    referenceId,
                    createdAt
                });
        }
    }
}

