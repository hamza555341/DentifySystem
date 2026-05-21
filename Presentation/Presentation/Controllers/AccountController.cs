using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
using Shared.DTOs.ProfileDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(
            IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<ProfileResponseDTO>> GetProfile()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(
                await _accountService
                    .GetCurrentProfileAsync(UserId!));
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(
            UpdateProfileDTO dto)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            return HandleResult(
                await _accountService
                    .UpdateProfileAsync(UserId!, dto));
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordDTO dto)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            return HandleResult(
                await _accountService
                    .ChangePasswordAsync(UserId!, dto));
        }
    }
}
