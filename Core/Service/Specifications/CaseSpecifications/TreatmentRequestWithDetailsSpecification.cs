using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.CaseSpecifications
{
    public class TreatmentRequestWithDetailsSpecification
        : BaseSpecification<TreatmentRequest,int>
    {
        public TreatmentRequestWithDetailsSpecification(int requestId)
            : base(r => r.Id == requestId)
        {
            AddInclude(t => t.Case);
            AddInclude(t => t.Case.Patient);
            AddInclude(t => t.Student);
            
        }
    }
}
