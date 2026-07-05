using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.TreatmentRequestSpecificaition
{
    public class PatientSentRequestsSpecification : BaseSpecification<TreatmentRequest, int>
    {
        public PatientSentRequestsSpecification(int patientId)
            : base(r => r.Case.PatientId == patientId &&
                        r.InitiatedBy == RequestInitiator.Patient)
        {
            AddInclude(r => r.Student);
            AddInclude(r=>r.Student.ApplicationUser);
            AddInclude(r => r.Case);
            AddOrderByDesc(r => r.CreatedAt);
        }
    }
}
