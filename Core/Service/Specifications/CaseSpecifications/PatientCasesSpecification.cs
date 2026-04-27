using Domain.Entites.CaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.CaseSpecifications
{
    public class PatientCasesSpecification : BaseSpecification<Case, int>
    {
        public PatientCasesSpecification(int patientId)
            : base(c => c.PatientId == patientId)
        {
            AddInclude(c => c.Images);
            AddOrderByDesc(c => c.CreatedAt);
        }
    }
}
