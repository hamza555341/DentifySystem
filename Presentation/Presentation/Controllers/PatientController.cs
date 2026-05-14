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
    public class PatientController : ApiBaseController
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("available-students/{caseId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAvailableStudents(int caseId)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _patientService.GetAvailableStudentsAsync(caseId, identityUserId);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}