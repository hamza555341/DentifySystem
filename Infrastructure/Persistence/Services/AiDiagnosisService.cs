using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.AiDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class AiDiagnosisService : IAiDiagnosisService
    {
        private readonly HttpClient _httpClient;

        private static readonly HashSet<string> ValidDiagnoses = new()
    {
        "Dental Caries",
        "Periodontal Disease",
        "Hypodontia",
        "Mouth Ulcer",
        "Tooth Discoloration"
    };

        public AiDiagnosisService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");

        }

        public async Task<AiResult> AnalyzeImageAsync(
            IFormFile image,
            string? painDuration = null,
            string? chronicDiseases = null)
        {
            using var form = new MultipartFormDataContent();

            var stream = image.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(image.ContentType);

            form.Add(fileContent, "file", image.FileName);

            if (!string.IsNullOrEmpty(painDuration))
                form.Add(new StringContent(painDuration), "pain_duration");

            if (!string.IsNullOrEmpty(chronicDiseases))
                form.Add(new StringContent(chronicDiseases), "chronic_diseases");
    
            var response = await _httpClient.PostAsync("/analyze/", form);

            if (!response.IsSuccessStatusCode)
                return new AiResult { IsValidDentalImage = false };

            var json = await response.Content.ReadAsStringAsync();



            return ParseReport(json);
        }

        public async Task<AiResult> AnalyzeTextAsync(string symptomsText)
        {
            var payload = JsonSerializer.Serialize(new
            {
                symptoms_description = symptomsText
            });

            var content = new StringContent(
                payload, Encoding.UTF8, "application/json");

            var response = await _httpClient
                .PostAsync("/triage-symptoms/", content);

            if (!response.IsSuccessStatusCode)
                return new AiResult { IsValidDentalImage = false };

            var json = await response.Content.ReadAsStringAsync();

            return ParseReport(json);
        }

        private AiResult ParseReport(string json)
        {
            Console.WriteLine("PARSE INPUT:");
            Console.WriteLine(json);

            // صورة مش أسنان
            if (json.Contains("يرجى رفع صورة أسنان") ||
                json.Contains("not dental") ||
                json.Contains("Needs_Clarification") ||
                json.Contains("Out_of_Domain"))
            {
                return new AiResult
                {
                    IsValidDentalImage = false,
                    FullReport = json
                };
            }

            AiReportDto? report = null;

            try
            {
                report = JsonSerializer.Deserialize<AiReportDto>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                return new AiResult
                {
                    IsValidDentalImage = false,
                    FullReport = json
                };
            }

            var diagnosis = report?.MedicalAssessment?.Diagnosis?.Trim()
                .Replace(" -", "-");

            // أسنان سليمة
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                return new AiResult
                {
                    IsValidDentalImage = false,
                    FullReport = json
                };
            }

            if (diagnosis.Contains("Healthy") ||
                diagnosis.Contains("سليم"))
            {
                return new AiResult
                {
                    IsValidDentalImage = true,
                    IsHealthy = true,
                    Diagnosis = "Healthy",
                    FullReport = json
                };
            }

            return new AiResult
            {
                IsValidDentalImage = true,
                IsHealthy = false,
                Diagnosis = diagnosis,
                FullReport = json
            };
        }
    }
}
