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
               o => o.MapFrom(s =>
                   s.Student.ApplicationUser.DisplayName))

           .ForMember(d => d.PatientName,
               o => o.MapFrom(s =>
                   s.Patient.ApplicationUser.DisplayName))

           .ForMember(d => d.Rating,
               o => o.MapFrom(s => s.Rating))

           .ForMember(d => d.Comment,
               o => o.MapFrom(s => s.Comment))

           .ForMember(d => d.CreatedAt,
               o => o.MapFrom(s => s.CreatedAt))

           .ForMember(d => d.TreatmentRequestId,
               o => o.MapFrom(s => s.TreatmentRequestId));
        }
    }

}
