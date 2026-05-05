using Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AppointmentSpecifications
  {
    public class AppointmentsByCaseSpecification
    : BaseSpecification<Appointment, int>
    {
        public AppointmentsByCaseSpecification(int caseId)
            : base(a => a.TreatmentRequest.CaseId == caseId)
        {
            AddInclude(a => a.TreatmentRequest);
            AddInclude(a => a.TreatmentRequest.Case);
            AddInclude(a => a.TreatmentRequest.Case.Patient);
            AddInclude(a => a.TreatmentRequest.Student);

            AddOrderBy(a => a.AppointmentDate);
        }
    }
}
