using Domain.Entites.IdentityModule;
using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.ChatModule
{
    public class ChatMessage : BaseEntity<int>
    {
        public int TreatmentRequestId { get; set; }

        public string SenderId { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? MediaUrl { get; set; }

        public bool IsRead { get; set; } = false;

        public TreatmentRequest TreatmentRequest { get; set; } = null!;
        public ApplicationUser Sender { get; set; } = null!;
    }
}
