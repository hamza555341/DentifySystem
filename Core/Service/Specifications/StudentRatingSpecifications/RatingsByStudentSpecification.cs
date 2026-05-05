using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.StudentRatingSpecifications
{
    public class RatingsByStudentSpecification
    : BaseSpecification<StudentRating, int>
    {
        public RatingsByStudentSpecification(int studentId)
            : base(r => r.StudentId == studentId)
        {
            AddInclude(r => r.Patient);
            AddInclude(r => r.Patient.ApplicationUser);

            AddInclude(r => r.TreatmentRequest);
            AddInclude(r => r.TreatmentRequest.Case);
        }
    }
}
