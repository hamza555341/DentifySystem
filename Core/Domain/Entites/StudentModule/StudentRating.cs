using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.StudentModule
{
    public class StudentRating:BaseEntity<int>
    {

        public int TreatmentRequestId { get; set; }
        public TreatmentRequest TreatmentRequest { get; set; } = null!;

        public int StudentId { get; set; }

        public int PatientId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public Student Student { get; set; } = null!;

        public Patient Patient { get; set; } = null!;
    }
}
