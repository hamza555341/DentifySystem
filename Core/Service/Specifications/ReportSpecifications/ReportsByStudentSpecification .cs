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
            : base(r=>r.TreatmentRequest.StudentId==studentId)
        {
            AddInclude(r => r.Images);
            AddInclude(x => x.TreatmentRequest);
            AddInclude(x => x.TreatmentRequest.Student.ApplicationUser);
            
            AddOrderByDesc(r => r.CreatedAt);
        }
    }
}
