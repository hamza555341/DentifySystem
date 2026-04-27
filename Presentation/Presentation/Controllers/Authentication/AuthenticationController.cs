using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Service.Abstraction;
using IAuthenticationService = Service.Abstraction.IAuthenticationService;

namespace Presentation.Controllers.Authentication
{
    public class AuthenticationController: ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register/patient")]
        public async Task<ActionResult<UserDTO>> RegisterPatient(RegisterPatientDTO dto)
        {
             var result= await _authenticationService.RegisterPatientAsync(dto);

            return HandleResult(result);
        }

        [HttpPost("register/student")]
        public async Task<ActionResult<UserDTO>> RegisterStudent(RegisterStudentDTO dto)
        {
            var result = await _authenticationService.RegisterStudentAsync(dto);
            return HandleResult(result);
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var result = await _authenticationService.LoginAsyns(loginDto);
            return HandleResult(result);
        }

        [Authorize]
        [HttpGet("currentuser")]
        public async Task<ActionResult<CurrentUserDTO>> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await _authenticationService.GetCurrentUserAsync(userId!));
        }


        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            var result = await _authenticationService.CheckEmailAsync(email);
            return Ok(result);
        }







    }
}
