using Domain.Entites.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.NotificationSpecifications
{
    public class NotificationUnreadSpecification
         : BaseSpecification<Notification, int>
    {
        public NotificationUnreadSpecification(string receiverId)
            : base(n => n.recieverId == receiverId &&
                        !n.IsRead)
        {
        }
    }
}
