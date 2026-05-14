using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
using Shared.DTOs.CaseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class CaseController : ApiBaseController
    {
        private readonly ICaseService _caseService;

        public CaseController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        // Patient
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<CaseResponseDTO>> CreateCase(
            [FromForm] CreateCaseDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await _caseService.CreateCaseAsync(userId!, dto));
        }

        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<IEnumerable<CaseResponseDTO>>> GetMyCases()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await _caseService.GetPatientCasesAsync(userId!));
        }

        // Student
        [HttpGet("available")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<CaseResponseDTO>>> GetAvailableCases([FromQuery] string? city)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _caseService.GetAvailableCasesAsync(city, identityUserId);
            return HandleResult(result);
        }

        [HttpGet("assigned")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<CaseResponseDTO>>> GetAssignedCases()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await _caseService.GetStudentCasesAsync(userId!));
        }

       
        // Shared
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CaseResponseDTO>> GetCaseById(int id)
        {
            return HandleResult(await _caseService.GetCaseByIdAsync(id));
        }

        // Admin
        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveCase(int id)
        {
            return HandleResult(await _caseService.ApproveCaseAsync(id));
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectCase(int id)
        {
            return HandleResult(await _caseService.RejectCaseAsync(id));
        }


    }

}

