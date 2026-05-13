using AutoMapper;
using Domain.Entites.TreatmentRequestModule;
using Shared.DTOs.TreatmentRequestsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{

        public class TreatmentRequestMappingProfile : Profile
        {
            public TreatmentRequestMappingProfile()
            {
                CreateMap<TreatmentRequest, TreatmentRequestResponseDTO>()
                    .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.ApplicationUser.DisplayName))
                    .ForMember(dest => dest.StudentUniversity, opt => opt.MapFrom(src => src.Student.University))
                    .ForMember(dest => dest.StudentProfileImageUrl, opt => opt.MapFrom(src => src.Student.ProfileImageUrl))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                    .ForMember(dest => dest.InitiatedBy, opt => opt.MapFrom(src => src.InitiatedBy.ToString()));
            }
        }
}

