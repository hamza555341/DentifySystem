using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TreatmentRequestsDTOs
{
    public class TreatmentRequestResponseDTO
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentUniversity { get; set; } = null!;
        public string? StudentProfileImageUrl { get; set; }
        public string Status { get; set; } = null!;
        public string InitiatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
