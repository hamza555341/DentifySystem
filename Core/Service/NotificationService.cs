using Domain.Entites.Notifications;
using Domain.Interfaces;
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

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task CreateNotificationAsync(string receiverId, string senderId, string title, string message, NotificationType type, int? referenceId = null)
        {

        }


        
    }
}
     



   

