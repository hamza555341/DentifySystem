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

   
        // Student
        [HttpGet("available")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<CaseResponseDTO>>> GetAvailableCases([FromQuery] string? city=null)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _caseService.GetAvailableCasesAsync(city, identityUserId);
            return HandleResult(result);
        }



        // Shared
        [HttpGet("my-cases")]
        [Authorize(Roles = "Patient,Student")]
        public async Task<ActionResult<IEnumerable<CaseResponseDTO>>> GetMyCases()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return HandleResult(
                await _caseService.GetMyCasesAsync(userId!, role!)
            );
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CaseResponseDTO>> GetCaseById(int id)
        {
            return HandleResult(await _caseService.GetCaseByIdAsync(id));
        }

        [HttpPut("{caseId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> EditCase(int caseId, [FromForm] EditCaseDTO dto)
        {
            return HandleResult(await _caseService.EditCase(caseId, dto));
        }

        [HttpDelete("{caseId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> DeleteCase(int caseId)
        {
            return HandleResult(await _caseService.DeleteCase(caseId));
        }


    }

}

