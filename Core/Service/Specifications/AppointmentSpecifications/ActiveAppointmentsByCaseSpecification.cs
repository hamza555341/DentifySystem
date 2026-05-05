using Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AppointmentSpecifications
{
    public class ActiveAppointmentsByRequestSpecification
        : BaseSpecification<Appointment, int>
    {
        public ActiveAppointmentsByRequestSpecification(int requestId)
            : base(a =>
                a.TreatmentRequestId == requestId &&
                (a.Status == AppointmentStatus.proposed ||
                 a.Status == AppointmentStatus.Confirmed)
            )
        {
            AddInclude(a => a.TreatmentRequest);
            AddOrderByDesc(a => a.AppointmentDate);
        }
    }
}
