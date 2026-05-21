using Domain.Entites.CaseModule;
using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TreatmentRequestsDTOs
{
    namespace Shared.DTOs.TreatmentRequests
    {
        public class StudentRequestResponseDTO
        {
            public int RequestId { get; set; }

            public int CaseId { get; set; }

            public string PatientName { get; set; } = null!;

            public string CaseDescription { get; set; } = null!;

            public string City { get; set; } = null!;

            public CaseStatus CaseStatus { get; set; }

            public TreatmentRequestStatus RequestStatus { get; set; }

            public DateTime CreatedAt { get; set; }
        }
    }
}
