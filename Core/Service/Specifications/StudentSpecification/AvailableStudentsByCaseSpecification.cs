using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.StudentSpecification
{
    public class AvailableStudentsByCaseSpecification : BaseSpecification<Student, int>
    {
        public AvailableStudentsByCaseSpecification( int caseId, Specialization specialization)
            : base(s =>
             s.IsActive &&
             s.Specializations.HasFlag(specialization) &&
             !s.TreatmentRequests.Any(
             tr =>
                 tr.CaseId == caseId &&
                 tr.Status != TreatmentRequestStatus.Rejected))
        {
            AddInclude(s => s.ApplicationUser);
        }
    }
}
