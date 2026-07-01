using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface INotificationHubService
    {
        Task SendAsync(
            string receiverId,
            string title,
            string message,
            int notificationType,
            int? referenceId,
            DateTime createdAt);
    }
}
