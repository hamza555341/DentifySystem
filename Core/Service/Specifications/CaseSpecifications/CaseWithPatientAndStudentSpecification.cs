using Domain.Entites.CaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.CaseSpecifications
{
    public class CaseWithPatientAndStudentSpecification : BaseSpecification<Case, int>
    {
        public CaseWithPatientAndStudentSpecification(int caseId)
       : base(c => c.Id == caseId)
        {
            // Patient + Identity
            AddInclude(c => c.Patient);
            AddInclude(c => c.Patient.ApplicationUser);

            // Requests + Student + Identity
            AddInclude(c => c.TreatmentRequests);
            AddInclude(c => c.TreatmentRequests.Select(r => r.Student));
            AddInclude(c => c.TreatmentRequests.Select(r => r.Student.ApplicationUser));
        }
    }
}

