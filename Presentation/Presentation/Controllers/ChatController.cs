using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
using Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class ChatController : ApiBaseController
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("{requestId}")]
        [Authorize(Roles = "Patient,Student")]
        public async Task<IActionResult> GetChatHistory(int requestId)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _chatService.GetChatHistoryAsync(requestId, identityUserId);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors); ;
        }
    }
}
