using Domain.Entites.CaseModule;
using Domain.Interfaces;
using Service.Specifications;

namespace DentifySystem.BackgroundJobs
{
    public class ExpiredCasesSpecification : BaseSpecification<Case, int>
    {
        public ExpiredCasesSpecification()
            : base(c=>
                        c.CreatedAt < DateTime.UtcNow.AddDays(-7))
        {
        }
    }
}