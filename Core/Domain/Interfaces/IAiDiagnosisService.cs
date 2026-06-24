using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAiDiagnosisService
    {
        Task<AiResult> AnalyzeImageAsync(
            IFormFile image,
            string? painDuration = null,
            string? chronicDiseases = null);

        Task<AiResult> AnalyzeTextAsync(string symptomsText);
    }

    public class AiResult
    {
        public bool IsValidDentalImage { get; set; }
        public bool IsHealthy { get; set; }
        public string? Diagnosis { get; set; }
        public string FullReport { get; set; } = null!;
    }
}
