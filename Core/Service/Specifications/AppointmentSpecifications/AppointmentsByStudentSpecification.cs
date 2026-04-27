using Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AppointmentSpecifications
{
    public class AppointmentsByStudentSpecification : BaseSpecification<Appointment, int>
    {
        public AppointmentsByStudentSpecification(int studentId)
            : base(a => a.StudentId == studentId)
        {
            AddInclude(a => a.Patient);
            AddInclude(a => a.Case);
        AddOrderByDesc(a => a.AppointmentDate);
    }
}
}
