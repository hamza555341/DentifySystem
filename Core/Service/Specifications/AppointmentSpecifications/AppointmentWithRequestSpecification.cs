using Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AppointmentSpecifications
{
    public class AppointmentWithRequestSpecification
        : BaseSpecification<Appointment, int>
    {
        public AppointmentWithRequestSpecification(int appointmentId)
            : base(a => a.Id == appointmentId)
        {
            AddInclude(a => a.TreatmentRequest);
            AddInclude(a => a.TreatmentRequest.Case);
            AddInclude(a => a.TreatmentRequest.Case.Patient);
            AddInclude(a => a.TreatmentRequest.Student);
        }
    }
}
