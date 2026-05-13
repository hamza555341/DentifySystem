using Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.AppointmentSpecifications
{
    public class AppointmentByIdForPatientSpecification
    : BaseSpecification<Appointment, int>
    {
        public AppointmentByIdForPatientSpecification(int appointmentId, int patientId)
            : base(a =>
                a.Id == appointmentId &&
                a.TreatmentRequest.Case.PatientId == patientId)
        {
            AddInclude(a => a.TreatmentRequest);
            AddInclude(a => a.TreatmentRequest.Case);
            AddInclude(a => a.TreatmentRequest.Case.Patient); 
        }
    }
}
