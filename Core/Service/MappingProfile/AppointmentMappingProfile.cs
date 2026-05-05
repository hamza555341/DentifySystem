using AutoMapper;
using Domain.Entites.AppointmentModule;
using Shared.DTOs.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
    public class AppointmentMappingProfile : Profile
    {
        public AppointmentMappingProfile()
        {
            CreateMap<Appointment, AppointmentResponseDTO>()

                // Patient Name
                .ForMember(d => d.PatientName,
                    o => o.MapFrom(s => s.TreatmentRequest.Case.Patient.ApplicationUser.DisplayName))

                // Student Name
                .ForMember(d => d.StudentName,
                    o => o.MapFrom(s => s.TreatmentRequest.Student.ApplicationUser.DisplayName))

                // Status
                .ForMember(d => d.Status,
                    o => o.MapFrom(s => s.Status.ToString()));

            // Slot → Appointment
            CreateMap<ProposedSlotDTO, Appointment>()
                .ForMember(d => d.AppointmentDate,
                    o => o.MapFrom(s => s.AppointmentDate))

                .ForMember(d => d.Location,
                    o => o.MapFrom(s => s.Location))

                // الباقي يتحدد في السيرفيس
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
