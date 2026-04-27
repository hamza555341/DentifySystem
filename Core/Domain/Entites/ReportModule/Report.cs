using Domain.Entites.CaseModule;
using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.ReportModule
{
    public class Report : BaseEntity<int>
    {

        public int CaseId { get; set; }

        public int StudentId { get; set; }

        public string Diagnosis { get; set; } = null!;

        public string TreatmentPlan { get; set; } = null!;

        public string? Notes { get; set; } = null!;
        public Case Case { get; set; } = null!;

        public Student Student { get; set; } = null!;

        public ICollection<ReportImage> Images { get; set; } = new List<ReportImage>();
    }
}