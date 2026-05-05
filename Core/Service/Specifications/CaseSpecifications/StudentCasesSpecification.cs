using Domain.Entites.CaseModule;
using Domain.Entites.TreatmentRequestModule;
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
            : base(c=>c.TreatmentRequests.Any(x=>x.StudentId==studentId && x.Status==TreatmentRequestStatus.Accepted ))
        {
            AddInclude(c => c.Images);
            AddInclude(c => c.Patient);
            AddInclude(c=>c.TreatmentRequests);
            AddInclude(c => c.Patient.ApplicationUser);
            AddInclude(c => c.TreatmentRequests.Select(x=>x.Student));
            AddInclude(c => c.TreatmentRequests.Select(tr => tr.Student.ApplicationUser));

            AddOrderByDesc(c => c.CreatedAt);
        }
    }
}
