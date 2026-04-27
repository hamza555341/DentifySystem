using Domain.Entites.ReportModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.ReportSpecifications
{
    public class ReportWithDetailsSpecification : BaseSpecification<Report, int>
    {
        public ReportWithDetailsSpecification(int reportId)
            : base(r => r.Id == reportId)
        {
            AddInclude(r => r.Images);
            AddInclude(r => r.Student);
            AddInclude(r => r.Case);
        }
    }
}
