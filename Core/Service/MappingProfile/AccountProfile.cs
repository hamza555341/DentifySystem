using AutoMapper;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Shared.DTOs.ProfileDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Patient, ProfileResponseDTO>()
                .ForMember(
                    d => d.FullName,
                    o => o.MapFrom(
                        s => s.ApplicationUser.DisplayName))
                .ForMember(
                    d => d.Email,
                    o => o.MapFrom(
                        s => s.ApplicationUser.Email))
                .ForMember(
                    d => d.PhoneNumber,
                    o => o.MapFrom(
                        s => s.ApplicationUser.PhoneNumber))
                .ForMember(
                    d => d.Role,
                    o => o.MapFrom(_ => "Patient"))
      
                .ForMember(
                    d => d.Specializations,
                    o => o.Ignore());

            CreateMap<Student, ProfileResponseDTO>()
                .ForMember(
                    d => d.FullName,
                    o => o.MapFrom(
                        s => s.ApplicationUser.DisplayName))
                .ForMember(
                    d => d.Email,
                    o => o.MapFrom(
                        s => s.ApplicationUser.Email))
                .ForMember(
                    d => d.PhoneNumber,
                    o => o.MapFrom(
                        s => s.ApplicationUser.PhoneNumber))
                .ForMember(
                    d => d.Role,
                    o => o.MapFrom(_ => "Student"))
                .ForMember(
                    d => d.Specializations,
                    o => o.MapFrom(s =>
                        Enum.GetValues<Specialization>()
                            .Where(x =>
                                x != Specialization.None &&
                                s.Specializations.HasFlag(x))
                            .ToList()));
        }
    }
}
