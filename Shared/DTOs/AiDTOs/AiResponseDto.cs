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
        [JsonPropertyName("التقييم_الطبي_المبدئي")]
        public MedicalAssessment? MedicalAssessment { get; set; }
    }

    public class MedicalAssessment
    {
        [JsonPropertyName("تصنيف_الحالة")]
        public string? Diagnosis { get; set; }
    }
}
