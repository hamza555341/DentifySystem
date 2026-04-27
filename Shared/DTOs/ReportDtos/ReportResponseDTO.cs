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
        public int CaseId { get; set; }
        public string StudentName { get; set; } = null!;
        public string Diagnosis { get; set; } = null!;
        public string TreatmentPlan { get; set; } = null!;
        public string Notes { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<string> Images { get; set; } = new();
    }
}
