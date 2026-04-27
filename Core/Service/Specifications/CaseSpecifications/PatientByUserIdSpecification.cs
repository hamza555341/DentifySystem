using Domain.Entites.PatientModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.CaseSpecifications
{
    public class PatientByUserIdSpecification : BaseSpecification<Patient, int>
    {
        public PatientByUserIdSpecification(string userId)
            : base(p => p.IdentityUserId == userId) { }
    }
}
