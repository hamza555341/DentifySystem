using Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AppointmentSpecifications
{
    public class ProposedAppointmentsByCaseSpecification : BaseSpecification<Appointment, int>
    {
        public ProposedAppointmentsByCaseSpecification(int caseId)

            : base(a => a.CaseId == caseId && a.Status == AppointmentStatus.proposed)
                     
        {

        }
    }
}
