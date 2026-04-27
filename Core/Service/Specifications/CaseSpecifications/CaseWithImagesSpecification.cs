using Domain.Entites.CaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.CaseSpecifications
{
    public class CaseWithImagesSpecification : BaseSpecification<Case, int>
    {
        public CaseWithImagesSpecification(int caseId)
            : base(c => c.Id == caseId)
        {
            AddInclude(c => c.Images);
            AddInclude(c => c.Patient);
        }
    }
}
