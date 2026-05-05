using Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AppointmentSpecifications
{
    public class ProposedAppointmentsByRequestSpecification
        : BaseSpecification<Appointment, int>
    {
        public ProposedAppointmentsByRequestSpecification(int requestId)
            : base(a =>
                a.TreatmentRequestId == requestId &&
                a.Status == AppointmentStatus.proposed
            )
        {
            AddInclude(a => a.TreatmentRequest);
            AddOrderByDesc(a => a.AppointmentDate);
        }
    }
}
