using Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AppointmentSpecifications
{
    public class StudentConfirmedAppointmentsAtTimeSpecification : BaseSpecification<Appointment, int>
    {
        public StudentConfirmedAppointmentsAtTimeSpecification(int studentId, DateTimeOffset date)
            : base(a => a.TreatmentRequest.StudentId == studentId &&
                       a.AppointmentDate == date &&
                       a.Status == AppointmentStatus.Confirmed)
        {
            AddInclude(x=>x.TreatmentRequest);
            AddInclude(x=>x.TreatmentRequest.Student);
        }
    }
}
