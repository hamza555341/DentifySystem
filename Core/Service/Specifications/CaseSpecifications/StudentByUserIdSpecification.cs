using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.CaseSpecifications
{
    public class StudentByUserIdSpecification : BaseSpecification<Student, int>
    {
        public StudentByUserIdSpecification(string userId)
            : base(s => s.IdentityUserId == userId) { }
    }
}
