using Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AppointmentSpecifications
  {   
        public class AppointmentsByCaseSpecification : BaseSpecification<Appointment, int>
        {
            public AppointmentsByCaseSpecification(int caseId)
                : base(a => a.CaseId == caseId)
            {
                AddInclude(a => a.Student);
                AddInclude(a => a.Patient);
                AddOrderBy(a => a.AppointmentDate);
            }
        }
  }
