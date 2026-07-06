using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.TreatmentRequestSpecificaition
{
    public class TreatmentRequestByIdSpecification
         : BaseSpecification<TreatmentRequest, int>
    {
        public TreatmentRequestByIdSpecification(int requestId)
            : base(r => r.Id == requestId)
        {
            AddInclude(r => r.Student);

            AddInclude(r => r.Case);

            AddInclude(r => r.Case.Patient) ;
        }
    }
}
