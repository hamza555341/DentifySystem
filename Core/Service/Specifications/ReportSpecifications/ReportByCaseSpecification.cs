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
            : base(r => r.CaseId == caseId)
        {
            AddInclude(r => r.Images);
            AddInclude(r => r.Student);
        }
    }
}
