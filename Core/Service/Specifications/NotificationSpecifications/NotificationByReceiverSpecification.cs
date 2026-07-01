using Domain.Entites.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.NotificationSpecifications
{
    public class NotificationByReceiverSpecification
        : BaseSpecification<Notification, int>
    {
        public NotificationByReceiverSpecification(string receiverId)
            : base(n => n.recieverId == receiverId)
        {
            AddOrderByDesc(n => n.CreatedAt);
        }
    }
}
