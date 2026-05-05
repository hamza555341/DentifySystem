using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
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

        public TreatmentRequestsController(ITreatmentRequestService treatmentRequestService)
        {
            _treatmentRequestService = treatmentRequestService;
        }
        [HttpPost("student/send/{caseId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentSendRequest(int caseId)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _treatmentRequestService.StudentSendRequestAsync(caseId, identityUserId);
            return result.IsSuccess ? Ok() : BadRequest(result.Errors);
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

        [HttpGet("case/{caseId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetRequestsByCase(int caseId)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _treatmentRequestService.GetRequestsByCaseAsync(caseId, identityUserId);
            return result.IsSuccess ? Ok() : BadRequest(result.Errors);
        }

    }
}
