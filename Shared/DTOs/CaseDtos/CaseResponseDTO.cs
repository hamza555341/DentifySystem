using Microsoft.AspNetCore.Http;
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
        public string SpecidRequiredSpecialization { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int age { get; set; }
        public string PatientName { get; set; } = null!;

        public string Image { get; set; } = null!;

        public string? AiAnalysisResult { get; set; }
    }
}
