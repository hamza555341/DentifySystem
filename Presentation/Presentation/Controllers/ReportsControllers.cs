using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstraction;
using Shared.DTOs.ReportDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class ReportController : ApiBaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<ReportResponseDTO>> CreateReport(
            [FromForm] CreateReportDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await _reportService.CreateReportAsync(userId!, dto));
        }

        [HttpGet("case/{caseId}")]
        [Authorize]
        public async Task<ActionResult<ReportResponseDTO>> GetReportByCase(int caseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return HandleResult(await _reportService.GetReportByCaseAsync(caseId,userId!));
        }

        [HttpGet("my")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<ReportResponseDTO>>> GetMyReports()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await _reportService.GetStudentReportsAsync(userId!));
        }
    }
}
