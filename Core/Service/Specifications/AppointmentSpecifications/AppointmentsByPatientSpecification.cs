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
            : base(a => a.PatientId == patientId)
        {
            AddInclude(a => a.Student);
            AddInclude(a => a.Case);
            AddOrderByDesc(a => a.AppointmentDate);
        }
    }
}
