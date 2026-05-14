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
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ApplicationUser.DisplayName))
                .ForMember(dest => dest.Specializations, opt => opt.MapFrom(src =>
                    Enum.GetValues<Specialization>()
                        .Where(s => src.Specializations.HasFlag(s))
                        .Select(s => s.ToString())
                        .ToList()));
        }
    }
}
