using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
using Shared.DTOs.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AppointmentsController: ApiBaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
           _appointmentService = appointmentService;
        }

        [HttpPost("Propose")]
        [Authorize(Roles ="Student")]

        public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> ProposeAppointments(ProposeAppointmentsDTO dto)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await _appointmentService.ProposeAppointmentsAsync(UserId!,dto));
        }



        [HttpPut("{CaseId}/Reject")]
        [Authorize(Roles ="Patient")]
        public async Task<IActionResult> RejectAllAppointments(int caseId)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await _appointmentService.RejectAllAppointmentsAsync(caseId, UserId!));
        }


        [HttpPut("{appointmentId}/Select")]
        [Authorize(Roles ="Patient")]
        public async Task<IActionResult> SelectAppointment(int appointmentId)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await _appointmentService.SelectAppointmentAsync(appointmentId, UserId!));
        }

        [HttpGet("My/Patient")]
        [Authorize(Roles="Patient")]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> PatientAppointments()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await _appointmentService.GetPatientAppointmentsAsync(UserId!));
        }

        [HttpGet("My/Student")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> StudentAppointments()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await _appointmentService.GetStudentAppointmentsAsync(UserId!));
        }


        [HttpGet("case/{caseId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDTO>>> GetCaseAppointments(int caseId)
    
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(
                await _appointmentService.GetCaseAppointmentsAsync(caseId, UserId!));
        }




    }
}
