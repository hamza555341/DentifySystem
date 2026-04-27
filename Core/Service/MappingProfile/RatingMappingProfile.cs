using AutoMapper;
using Domain.Entites.StudentModule;
using Shared.DTOs.StudentRatingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
    public class RatingMappingProfile : Profile
    {
        public RatingMappingProfile()
        {
            CreateMap<StudentRating, RatingResponseDTO>()
                .ForMember(d => d.StudentName,
                    o => o.MapFrom(s => s.Student.FullName))
                .ForMember(d => d.PatientName,
                    o => o.MapFrom(s => s.Patient.FullName));
        }
    }

}
