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
                    .ForMember(d => d.Disease, o => o.MapFrom(s => s.Condition))
                    .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient.FullName))
                    .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                 .ForMember(d => d.Images, o => o.Ignore());

        }
    }

}
