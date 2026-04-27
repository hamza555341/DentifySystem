using Domain.Entites.CaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.CaseSpecifications
{
    public class StudentCasesSpecification : BaseSpecification<Case, int>
    {
        public StudentCasesSpecification(int studentId)
            : base(c => c.AssignedStudentId == studentId)
        {
            AddInclude(c => c.Images);
            AddInclude(c => c.Patient);
            AddOrderByDesc(c => c.CreatedAt);
        }
    }
}
