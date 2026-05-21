using AutoMapper;
using Domain.Entites.TreatmentRequestModule;
using Shared.DTOs.TreatmentRequestsDTOs;
using Shared.DTOs.TreatmentRequestsDTOs.Shared.DTOs.TreatmentRequests;
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
     .ForMember(d => d.StudentId,
         o => o.MapFrom(s => s.Student.Id))
     .ForMember(d => d.StudentName,
         o => o.MapFrom(s => s.Student.ApplicationUser.DisplayName))
     .ForMember(d => d.StudentCity,
         o => o.MapFrom(s => s.Student.City))
     .ForMember(d => d.StudentPhoneNumber,
         o => o.MapFrom(s => s.Student.ApplicationUser.PhoneNumber))
     .ForMember(d => d.StudentProfileImageUrl,
         o => o.MapFrom(s => s.Student.ProfileImageUrl))
     .ForMember(d => d.Status,
         o => o.MapFrom(s => s.Status.ToString()))
     .ForMember(d => d.InitiatedBy,
         o => o.MapFrom(s => s.InitiatedBy.ToString()))
     .ForMember(d => d.AverageRating, o => o.Ignore())
     .ForMember(d => d.TotalRatings, o => o.Ignore());



            CreateMap<TreatmentRequest, StudentRequestResponseDTO>()
    .ForMember(d => d.RequestId,
        o => o.MapFrom(s => s.Id))

    .ForMember(d => d.CaseId,
        o => o.MapFrom(s => s.CaseId))

    .ForMember(d => d.PatientName,
        o => o.MapFrom(
            s => s.Case.Patient.ApplicationUser.DisplayName))

    .ForMember(d => d.CaseDescription,
        o => o.MapFrom(s => s.Case.Description))

    .ForMember(d => d.City,
        o => o.MapFrom(s => s.Case.City))

    .ForMember(d => d.CaseStatus,
        o => o.MapFrom(s => s.Case.Status))

    .ForMember(d => d.RequestStatus,
        o => o.MapFrom(s => s.Status))

    .ForMember(d => d.CreatedAt,
        o => o.MapFrom(s => s.CreatedAt));



        }
    }
}

