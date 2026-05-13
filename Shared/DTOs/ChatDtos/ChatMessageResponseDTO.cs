using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ChatDtos
{
    public class ChatMessageResponseDTO
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? MediaUrl { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
