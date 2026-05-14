using Domain.Entites.CaseModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;

namespace Service.Specifications.CaseSpecifications
{
    public class AvailableCasesSpecification : BaseSpecification<Case, int>
    {
        public AvailableCasesSpecification(string? city, Specialization specialization )
         : base(c => c.Status == CaseStatus.Approved &&
                     !c.TreatmentRequests.Any(r => r.Status == TreatmentRequestStatus.Accepted) &&
                     (string.IsNullOrEmpty(city) || c.City.ToLower() == city!.ToLower()) &&
                     ( specialization.HasFlag(c.RequiredSpecialization)))
        {
            AddInclude(c => c.Images);
            AddInclude(c => c.Patient.ApplicationUser);
            AddInclude(c => c.TreatmentRequests);
            AddOrderByDesc(c => c.CreatedAt);
        }
    }
}
