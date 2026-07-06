using Domain.Entites.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.NotificationSpecifications
{
    public class NotificationByIdSpecification
         : BaseSpecification<Notification, int>
    {
        public NotificationByIdSpecification(int id)
            : base(n => n.Id == id)
        {
        }
    }
}
