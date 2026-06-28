using Domain.Entites.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.Notifications
{
    public class Notification : BaseEntity<int>
    {
        public string recieverId { set; get; } = null!;
        public string Title { get; set; } = null!;

        public string Message { get; set; } = null!;

        public bool IsRead { get; set; }
        public NotificationType Type { get; set; }

        public int? ReferenceId { get; set; }
        public string SenderId { get; set; } = null!;

        public ApplicationUser reciever { get; set; } = null!;

        public ApplicationUser Sender { get; set; } = null!;
    }
}
