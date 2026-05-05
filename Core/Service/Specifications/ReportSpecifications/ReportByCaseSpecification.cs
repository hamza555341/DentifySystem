using Domain.Entites.ReportModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.ReportSpecifications
{
    public class ReportByCaseSpecification : BaseSpecification<Report, int>
    {
        public ReportByCaseSpecification(int caseId)
            : base(r=>r.TreatmentRequest.CaseId==caseId)
        {
            AddInclude(r => r.Images);
            AddInclude(r => r.TreatmentRequest);
            AddInclude(r => r.TreatmentRequest.Student);
            AddInclude(r => r.TreatmentRequest.Student.ApplicationUser);

            AddOrderByDesc(r => r.CreatedAt);

        }
    }
}
