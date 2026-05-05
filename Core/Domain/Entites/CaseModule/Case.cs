using Domain.Entites.AppointmentModule;
using Domain.Entites.PatientModule;
using Domain.Entites.ReportModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.CaseModule
{
    public class Case : BaseEntity<int>
    {
        public int PatientId { get; set; }

        public string Disease { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string City { get; set; } = null!;

        public CaseStatus Status { get; set; } = CaseStatus.Pending;

        public string? AiAnalysisResult { get; set; }

        public Patient Patient { get; set; } = null!;

        public ICollection<CaseImage> Images { get; set; } = new List<CaseImage>();

        public DateTime? LastUpdatedAt { get; set; }

        public ICollection<Report> Reports { get; set; } = new List<Report>();

        public ICollection<TreatmentRequest> TreatmentRequests { get; set; } = new List<TreatmentRequest>();





    }
}
