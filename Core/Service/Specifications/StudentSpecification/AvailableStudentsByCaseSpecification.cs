using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.StudentSpecification
{
    public class AvailableStudentsByCaseSpecification : BaseSpecification<Student, int>
    {
        public AvailableStudentsByCaseSpecification(
            Specialization specialization,
            string? universityName = null)
            : base(s =>
                s.IsActive &&
                s.Specializations.HasFlag(specialization) &&
                (universityName == null || s.UniversityName == universityName))
        {
            AddInclude(s => s.ApplicationUser);
        }
    }
}
