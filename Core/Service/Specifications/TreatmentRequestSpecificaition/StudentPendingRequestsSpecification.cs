using Domain.Entites.CaseModule;
using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.TreatmentRequestSpecificaition
{
    public class StudentPendingRequestsSpecification
      : BaseSpecification<TreatmentRequest, int>
    {
        public StudentPendingRequestsSpecification(int studentId)
            : base(r =>
                r.StudentId == studentId
                
                )
        {
            AddInclude(r => r.Case);
            AddInclude(r => r.Case.Patient.ApplicationUser);

            AddOrderByDesc(r => r.CreatedAt);
        }
    }
}
