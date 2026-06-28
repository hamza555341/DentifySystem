using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.StudentSpecification
{
    public class StudentByIdWithUserSpecification : BaseSpecification<Student, int>
    {
        public StudentByIdWithUserSpecification(int studentId)
            : base(s => s.Id == studentId)
        {
            AddInclude(s => s.ApplicationUser);
        }
    }
}
