using AutoMapper;
using Domain.Entites.ReportModule;
using Shared.DTOs.ReportDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
    public class ReportMappingProfile : Profile
    {
        public ReportMappingProfile()
        {
            CreateMap<Report, ReportResponseDTO>()

             .ForMember(d => d.StudentName,
                 o => o.MapFrom(s =>
                     s.TreatmentRequest.Student.ApplicationUser.DisplayName))

             .ForMember(d => d.Images,
                 o => o.MapFrom(s =>
                     s.Images.Select(i => i.ImageUrl)))

             .ForMember(d => d.TreatmentRequestId,
                 o => o.MapFrom(s => s.TreatmentRequestId))

             .ForMember(d => d.CaseId,
                 o => o.MapFrom(s => s.TreatmentRequest.CaseId));
        }
    }
}
