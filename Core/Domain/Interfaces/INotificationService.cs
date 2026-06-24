using Domain.Entites.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string receiverId, string senderId, string title, string message, NotificationType type, int? referenceId = null);
    }
}
