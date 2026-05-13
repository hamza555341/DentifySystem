using Shared.CommonResult;
using Shared.DTOs.ChatDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatMessageResponseDTO>>> GetChatHistoryAsync(int requestId, string identityUserId);

    }
}
