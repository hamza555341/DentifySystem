using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
using Shared.DTOs.AppointmentDtos;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AppointmentsController : ApiBaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("Propose")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ProposeAppointment(ProposeAppointmentsDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _appointmentService.ProposeAppointmentsAsync(userId!, dto);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPut("{appointmentId}/Select")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> SelectAppointment(int appointmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _appointmentService.SelectAppointmentAsync(appointmentId, userId!);
            return result.IsSuccess ? Ok() : BadRequest(result.Errors);
        }

        [HttpGet("My/Patient")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> PatientAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _appointmentService.GetPatientAppointmentsAsync(userId!);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet("My/Student")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _appointmentService.GetStudentAppointmentsAsync(userId!);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}