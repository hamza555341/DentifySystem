using Domain.Entites.ReportModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.ReportSpecifications
{
    public class ReportsByStudentSpecification : BaseSpecification<Report, int>
    {
        public ReportsByStudentSpecification(int studentId)
            : base(r => r.StudentId == studentId)
        {
            AddInclude(r => r.Images);
            AddInclude(r => r.Case);
            AddOrderByDesc(r => r.CreatedAt);
        }
    }
}
