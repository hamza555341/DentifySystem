using Domain.Entites.CaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.CaseSpecifications
{
    public class CaseWithPatientAndStudentSpecification : BaseSpecification<Case, int>
    {
        public CaseWithPatientAndStudentSpecification(int CaseId)
            : base(c=>c.Id == CaseId)
        {
            AddInclude(c => c.Patient);
            AddInclude(c => c.AssignedStudent!);
        }
    }
}

