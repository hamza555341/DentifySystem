using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.AppointmentModule
{
    public class Appointment : BaseEntity<int>
    {


        public DateTimeOffset AppointmentDate { get; set; }

        public AppointmentStatus Status { get; set; } =AppointmentStatus.proposed;

        public string Location { get; set; } = null!;

        public int TreatmentRequestId { get; set; }
        public TreatmentRequest TreatmentRequest { get; set; }


    }
}
