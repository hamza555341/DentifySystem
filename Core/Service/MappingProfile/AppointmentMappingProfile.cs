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
                .ForMember(d => d.PatientName,
                    o => o.MapFrom(s => s.Patient.FullName))

                .ForMember(d => d.StudentName,
                    o => o.MapFrom(s => s.Student.FullName))

                .ForMember(d => d.Status,
                    o => o.MapFrom(s => s.Status.ToString()));

            // لو احتجت تحول Slot → Appointment (اختياري)
            CreateMap<ProposedSlotDTO, Appointment>()
                .ForMember(d => d.AppointmentDate,
                    o => o.MapFrom(s => s.AppointmentDate))

                .ForMember(d => d.Location,
                    o => o.MapFrom(s => s.Location))

                // دول هيتحددوا في السيرفيس مش من الـ DTO
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
