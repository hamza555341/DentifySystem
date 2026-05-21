using Shared.CommonResult;
using Shared.DTOs.TreatmentRequestsDTOs;
using Shared.DTOs.TreatmentRequestsDTOs.Shared.DTOs.TreatmentRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface ITreatmentRequestService
    {
        Task<Result> StudentSendRequestAsync(int caseId , string IdentityUserId);
        Task<Result> PatientSendRequestAsync(int studentId, int caseId, string identityUserId);
        Task<Result> AcceptRequestAsync(int requestId, string identityUserId);
        Task<Result> RejectUserAsync(int requestId, string identityUserId);

        Task<Result<IEnumerable<TreatmentRequestResponseDTO>>> GetRequestsByCaseAsync(string identityUserId);

        Task<Result<IEnumerable<StudentRequestResponseDTO>>> GetStudentRequestsAsync(string identityUserId);
   
    }
}
