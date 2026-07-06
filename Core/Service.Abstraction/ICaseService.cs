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

        Task<Result<IEnumerable<CaseResponseDTO>>> GetAvailableCasesAsync(string? city, string identityUserId);

        Task<Result<IEnumerable<CaseResponseDTO>>> GetMyCasesAsync(
                                                                      string userId,
                                                                       string role);

        Task<Result<CaseResponseDTO>> GetCaseByIdAsync(int caseId);

        Task<Result>EditCase(int caseId, EditCaseDTO dto);
        Task<Result>DeleteCase(int caseId);
    }
}
