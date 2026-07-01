using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;
using Service.Abstraction;
using Shared.DTOs.TreatmentRequestsDTOs;
using Shared.DTOs.TreatmentRequestsDTOs.Shared.DTOs.TreatmentRequests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class TreatmentRequestsController:ApiBaseController
    {
        private readonly ITreatmentRequestService _treatmentRequestService;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public TreatmentRequestsController(ITreatmentRequestService treatmentRequestService, IHubContext<NotificationHub> notificationHub)
        {
            _treatmentRequestService = treatmentRequestService;
            _notificationHub = notificationHub;
        }
        [HttpPost("student/send/{caseId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentSendRequest(int caseId)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _treatmentRequestService.StudentSendRequestAsync(caseId, identityUserId);
            return  HandleResult(result);
        }

        [HttpPost("patient/send/{studentId}/{caseId}")]
        [Authorize(Roles = "Patient")]

        public async Task<IActionResult> PatientSendRequest(int studentId,int caseId)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result=await _treatmentRequestService.PatientSendRequestAsync(studentId,caseId, identityUserId);
            if(!result.IsSuccess) return BadRequest(result.Errors);
            return Ok();
        }

        [HttpPut("accept/{requestId}")]
        [Authorize(Roles = "Patient,Student")]
        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _treatmentRequestService.AcceptRequestAsync(requestId, identityUserId);
            return result.IsSuccess ? Ok() : BadRequest(result.Errors);
        }

        [HttpGet("cases")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<IEnumerable<TreatmentRequestResponseDTO>>> GetRequestsByCase()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _treatmentRequestService.GetRequestsByCaseAsync( identityUserId);
            return HandleResult(result);
        }

        [HttpGet("my/student")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<
    IEnumerable<StudentRequestResponseDTO>>> GetMyStudentRequests()
        {
            var identityUserId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!;

            return HandleResult(
                await _treatmentRequestService
                    .GetStudentRequestsAsync(
                        identityUserId));
        }

        [HttpGet("student/received-requests")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetPatientRequestsToStudent()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _treatmentRequestService.GetPatientRequestsToStudentAsync(identityUserId);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPut("reject/{requestId}")]
        [Authorize(Roles ="Patient,Student")]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var identityUserId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result=await _treatmentRequestService.RejectUserAsync(requestId, identityUserId);
            return result.IsSuccess ? Ok() : BadRequest(result.Errors);
        }


    }
}
