using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs.AiDTOs
{
    public class AiReportDto
    {
        [JsonPropertyName("initial_medical_assessment")]
        public MedicalAssessment? MedicalAssessment { get; set; }
    }

    public class MedicalAssessment
    {
        [JsonPropertyName("case_classification")]
        public string? Diagnosis { get; set; }
    }
}
