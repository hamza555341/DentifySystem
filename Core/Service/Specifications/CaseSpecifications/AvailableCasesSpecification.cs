using Domain.Entites.CaseModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;

namespace Service.Specifications.CaseSpecifications
{
    public class AvailableCasesSpecification : BaseSpecification<Case, int>
    {
        public AvailableCasesSpecification(string? city, Specialization specialization )
         : base(c => 
                     !c.TreatmentRequests.Any(r => r.Status == TreatmentRequestStatus.Accepted) &&
                     (city == null || c.City == city) &&
                     ( specialization.HasFlag(c.RequiredSpecialization)))
        {
            AddInclude(c => c.Patient.ApplicationUser);
            AddInclude(c => c.TreatmentRequests);
            AddOrderByDesc(c => c.CreatedAt);
        }
    }
}
