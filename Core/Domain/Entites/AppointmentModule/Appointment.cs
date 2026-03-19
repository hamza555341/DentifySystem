using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.AppointmentModule
{
    public class Appointment : BaseEntity<int>
    {

        public int CaseId { get; set; }

        public int StudentId { get; set; }

        public int PatientId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public AppointmentStatus Status { get; set; } =AppointmentStatus.Pending;

        public string Location { get; set; } = null!;

        public Case Case { get; set; } = null!;

        public Student Student { get; set; } = null!;

        public Patient Patient { get; set; } = null!;
    }
}
