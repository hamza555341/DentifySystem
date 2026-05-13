using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ReportDtos
{
    public class CreateReportDTO
    {
        public int TreatmentRequestId { get; set; }
        public string Diagnosis { get; set; } = null!;
        public string TreatmentPlan { get; set; } = null!;
        public string Notes { get; set; } = null!;
        public List<IFormFile>? Images { get; set; }
    }
}
