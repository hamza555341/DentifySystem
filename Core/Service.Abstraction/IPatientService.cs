using Shared.CommonResult;
using Shared.DTOs.StudentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface IPatientService
    {
        Task<Result<IEnumerable<StudentResponseDTO>>> GetAvailableStudentsAsync(int caseId, string identityUserId);
    }
}
