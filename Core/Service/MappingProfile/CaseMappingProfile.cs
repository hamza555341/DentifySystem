using AutoMapper;
using Domain.Entites.CaseModule;
using Shared.DTOs.CaseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{ 
       public class CaseMappingProfile : Profile
       {
            public CaseMappingProfile()
            {
            CreateMap<Case, CaseResponseDTO>()

        .ForMember(d => d.SpecidRequiredSpecialization,
            o => o.MapFrom(s => s.RequiredSpecialization))

        .ForMember(d => d.PatientName,
            o => o.MapFrom(s =>
                s.Patient.ApplicationUser.DisplayName))

        .ForMember(d => d.Status,
            o => o.MapFrom(s => s.Status.ToString()))

        .ForMember(d => d.Image,
            o => o.MapFrom(s => s.ImageUrl));

            CreateMap<EditCaseDTO, Case>()
                .ForMember(d => d.RequiredSpecialization,
            o => o.MapFrom(s => s.RequiredSpecialization))

        .ForMember(d => d.ImageUrl,
            o => o.MapFrom(s => s.Image));

        }
    }

}
