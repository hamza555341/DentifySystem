using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.StudentRatingSpecifications
{
    public class RatingWithDetailsSpecification
   : BaseSpecification<StudentRating, int>
    {
        public RatingWithDetailsSpecification(int ratingId)
            : base(r => r.Id == ratingId)
        {
            // Student + name
            AddInclude(r => r.Student);
            AddInclude(r => r.Student.ApplicationUser);

            // Patient + name
            AddInclude(r => r.Patient);
            AddInclude(r => r.Patient.ApplicationUser);

            // Optional: لو محتاج تربط بالـ request أو case في الـ DTO
            AddInclude(r => r.TreatmentRequest);
            // AddInclude(r => r.TreatmentRequest.Case);
        }
    }
}

