using Shared.CommonResult;
using Shared.DTOs.CaseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface ICaseService
    {
        Task<Result<CaseResponseDTO>> CreateCaseAsync(string userId, CreateCaseDTO dto);

        Task<Result<IEnumerable<CaseResponseDTO>>> GetAvailableCasesAsync(string? city);

        Task<Result<IEnumerable<CaseResponseDTO>>> GetPatientCasesAsync(string userId);

        Task<Result<IEnumerable<CaseResponseDTO>>> GetStudentCasesAsync(string userId);

        Task<Result<CaseResponseDTO>> GetCaseByIdAsync(int caseId);

        Task<Result> AcceptTreatmentRequestAsync(int requestId, string patientUserId);

        Task<Result> ApproveCaseAsync(int caseId);

        Task<Result> RejectCaseAsync(int caseId);
    }
}
