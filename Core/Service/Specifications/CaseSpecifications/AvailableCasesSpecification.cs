using Domain.Entites.CaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.CaseSpecifications
{
    public class AvailableCasesSpecification : BaseSpecification<Case, int>
    {
        public AvailableCasesSpecification()
            : base(c => c.Status == CaseStatus.Approved && c.AssignedStudentId == null)
        {
            AddInclude(c => c.Images);
            AddInclude(c => c.Patient);
            AddOrderByDesc(c => c.CreatedAt);
        }
    }
}
