using Domain.Entites.IdentityModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        private string UserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var result = await _notificationService
                .GetMyNotificationsAsync(UserId);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }
        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var result = await _notificationService
                .MarkAllAsReadAsync(UserId);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return NoContent();
        }
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var result = await _notificationService
                .GetUnreadCountAsync(UserId);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }
    }
}