using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.StudentRatingSpecifications
{
    public class RatingByCaseAndPatientSpecification : BaseSpecification<StudentRating, int>
    {
        public RatingByCaseAndPatientSpecification(int caseId, int patientId)
            : base(r => r.CaseId == caseId && r.PatientId == patientId)
        {
            AddInclude(r => r.Student);
            AddInclude(r => r.Patient);
        }
    }
}
