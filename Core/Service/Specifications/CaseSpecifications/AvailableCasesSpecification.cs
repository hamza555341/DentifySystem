using Domain.Entites.CaseModule;
using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.CaseSpecifications
{
    public class AvailableCasesSpecification : BaseSpecification<Case, int>
    {
        public AvailableCasesSpecification(string?city)
            : base(c =>c.Status == CaseStatus.Approved && !c.TreatmentRequests.Any(r => r.Status == TreatmentRequestStatus.Accepted)
              && (string.IsNullOrEmpty(city) || c.City.ToLower() == city!.ToLower()))
        {
            AddInclude(c => c.Images);
            AddInclude(c => c.Patient);
            AddInclude(c => c.TreatmentRequests);
            AddOrderByDesc(c => c.CreatedAt);
        }
    }
}
