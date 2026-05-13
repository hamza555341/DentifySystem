using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.TreatmentRequestSpecificaition
{
    public class TreatmentRequestsByCaseSpecification : BaseSpecification<TreatmentRequest,int>
    {
        public TreatmentRequestsByCaseSpecification(int caseId)
            : base(tr => tr.CaseId == caseId)
        {
            AddInclude(tr => tr.Student);
        }
    }
}
