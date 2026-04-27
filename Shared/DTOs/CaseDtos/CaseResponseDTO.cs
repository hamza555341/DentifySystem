using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CaseDtos
{
    public class CaseResponseDTO
    {
        public int Id { get; set; }
        public string Disease { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public string PatientName { get; set; } = null!;

        public List<string> Images { get; set; } = new();

        public string? AiAnalysisResult { get; set; }
    }
}
