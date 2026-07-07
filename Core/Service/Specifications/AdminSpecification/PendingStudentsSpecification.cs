using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AdminSpecification
{
    public class PendingStudentsSpecification : BaseSpecification<Student, int>
{
    public PendingStudentsSpecification()
        : base(s => s.IsActive)
    {
        AddInclude(s => s.ApplicationUser);
    }
}
}
