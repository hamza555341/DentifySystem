using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ReportDtos
{
    public class ReportResponseDTO
    {
        public int Id { get; set; }

        public int TreatmentRequestId { get; set; }   // ✅ مهم

        public int CaseId { get; set; }               // ✅ جاي من request

        public string StudentName { get; set; } = null!;

        public string Diagnosis { get; set; } = null!;

        public string TreatmentPlan { get; set; } = null!;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<string> Images { get; set; } = new();
    }
}
