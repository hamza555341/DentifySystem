using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ChatBot
{
    public class ChatBotRequestDTO
    {
        public string Question { get; set; } = null!;
        public string? History { get; set; }
    }
}
