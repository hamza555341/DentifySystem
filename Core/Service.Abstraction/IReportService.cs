using Shared.CommonResult;
using Shared.DTOs.ReportDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface IReportService
    {
        Task<Result<ReportResponseDTO>> CreateReportAsync(string userId, CreateReportDTO dto);
        Task<Result<ReportResponseDTO>> GetReportByCaseAsync(int caseId,string userId);
        Task<Result<IEnumerable<ReportResponseDTO>>> GetStudentReportsAsync(string userId);
    }
}
