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
                .ForMember(d => d.StudentName, o => o.MapFrom(s => s.Student.FullName))
                .ForMember(d => d.Images, o => o.Ignore());
        }
    }
}
