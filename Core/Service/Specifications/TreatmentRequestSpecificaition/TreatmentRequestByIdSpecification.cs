using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.TreatmentRequestSpecificaition
{
    public class TreatmentRequestByIdSpecification : BaseSpecification<TreatmentRequest,int>
    {
        public TreatmentRequestByIdSpecification(int requestId)
            : base(tr => tr.Id == requestId)
        {
            AddInclude(tr => tr.Case);
            AddInclude(tr => tr.Student);
        }
    }
}
