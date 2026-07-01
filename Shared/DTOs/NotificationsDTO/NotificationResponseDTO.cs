using Domain.Entites.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.NotificationsDTO
{
    public class NotificationResponseDTO
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }

        public string Title { get; set; } = null!;

        public string Message { get; set; } = null!;

        public NotificationType Type { get; set; }

        public int? ReferenceId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
