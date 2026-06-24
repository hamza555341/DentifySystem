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

        [HttpGet("available-students")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAvailableStudents()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _patientService.GetAvailableStudentsAsync( identityUserId);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet("students/{studentId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            var result = await _patientService.GetStudentByIdAsync(studentId);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}