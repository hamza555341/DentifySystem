using Domain.Entites.TreatmentRequestModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.TreatmentRequestSpecificaition
{
    public class TreatmentRequestByStudentAndCaseSpecification:BaseSpecification<TreatmentRequest,int>
    {
        public TreatmentRequestByStudentAndCaseSpecification(int studentId,int caseId):base(t=>t.StudentId==studentId &&t.CaseId ==caseId )
        {
            AddInclude(t => t.Student);
            AddInclude(t => t.Case);
        }
    }
}
