using Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AppointmentSpecifications
{
    
        public class AppointmentsByPatientSpecification : BaseSpecification<Appointment, int>
        {
            public AppointmentsByPatientSpecification(int patientId)
                : base(a => a.TreatmentRequest.Case.PatientId == patientId)
            {
                AddInclude(a => a.TreatmentRequest);
                AddInclude(a => a.TreatmentRequest.Case);
                AddInclude(a => a.TreatmentRequest.Student);

                AddOrderByDesc(a => a.AppointmentDate);
            }
        }
    
}
