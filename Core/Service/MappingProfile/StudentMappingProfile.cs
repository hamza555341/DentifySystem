using AutoMapper;
using Domain.Entites.StudentModule;
using Shared.DTOs.StudentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<Student, StudentResponseDTO>()
                
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.ProfileImageUrl))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ApplicationUser.DisplayName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber))
                .ForMember(dest => dest.Specializations, opt => opt.MapFrom(src =>
                    Enum.GetValues<Specialization>()
                        .Where(s => s != Specialization.None && src.Specializations.HasFlag(s))
                        .Select(s => s.ToString())
                        .ToList()));

        }
    }
}
