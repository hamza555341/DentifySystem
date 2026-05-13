using Domain.Entites.CaseModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.ReportModule
{
    public class Report : BaseEntity<int>
    {

        public int TreatmentRequestId { get; set; }

        public TreatmentRequest TreatmentRequest { get; set; } = null!;

        public string Diagnosis { get; set; } = null!;

        public string TreatmentPlan { get; set; } = null!;

        public string? Notes { get; set; }

        public ICollection<ReportImage> Images { get; set; } = new List<ReportImage>();
    }
}