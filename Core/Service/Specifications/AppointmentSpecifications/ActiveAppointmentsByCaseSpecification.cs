using Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AppointmentSpecifications
{
    public class ActiveAppointmentsByCaseSpecification : BaseSpecification<Appointment, int>
    {
        public ActiveAppointmentsByCaseSpecification(int caseId)
            : base(a => a.CaseId == caseId &&
                       (a.Status == AppointmentStatus.proposed ||
                        a.Status == AppointmentStatus.Confirmed))
        {
        }
    }
}
