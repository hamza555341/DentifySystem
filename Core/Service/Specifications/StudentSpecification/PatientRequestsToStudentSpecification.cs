using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.StudentSpecification
{
    public class PatientRequestsToStudentSpecification : BaseSpecification<TreatmentRequest, int>
    {
        public PatientRequestsToStudentSpecification(int studentId)
            : base(r => r.StudentId == studentId &&
                        r.InitiatedBy == RequestInitiator.Patient &&
                        r.Status == TreatmentRequestStatus.Pending)
        {
            AddInclude(r => r.Case);
            AddInclude(r=>r.Case.Patient.ApplicationUser);
            
        }
}

}
