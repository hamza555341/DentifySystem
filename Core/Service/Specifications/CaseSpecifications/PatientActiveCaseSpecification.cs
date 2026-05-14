using Domain.Entites.CaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.CaseSpecifications
{
    public class PatientActiveCaseSpecification : BaseSpecification<Case, int>
    {
        public PatientActiveCaseSpecification(int patientId)
            : base(c => c.PatientId == patientId &&
                        c.Status != CaseStatus.Completed &&
                        c.Status != CaseStatus.Rejected)
        {
        }
}

}
