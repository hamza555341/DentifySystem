using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.StudentRatingSpecifications
{
    public class RatingByRequestSpecification
    : BaseSpecification<StudentRating, int>
    {
        public RatingByRequestSpecification(int requestId)
            : base(r => r.TreatmentRequestId == requestId)
        {
        }
    }
}
