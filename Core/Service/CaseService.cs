using AutoMapper;
using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Abstraction;
using Service.Specifications.CaseSpecifications;
using Shared.CommonResult;
using Shared.DTOs.CaseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CaseService : ICaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public CaseService(IUnitOfWork unitOfWork,
            IAttachmentService attachmentService, IMapper mapper,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
            _mapper = mapper;
            _configuration = configuration;
        }

        #region paitient
        public async Task<Result<CaseResponseDTO>> CreateCaseAsync(string userId, CreateCaseDTO dto)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(userId));

            if (patient is null) 
            { 
                return Error.NotFound("Patient.NotFound");
            }

            var hasActiveCase = await _unitOfWork.GetRepository<Case, int>()
                .GetAllAsync(new PatientActiveCaseSpecification(patient!.Id));

            if (hasActiveCase.Any())
                return Error.Validation("Case.ActiveExists", "You already have an Pending case");

            if (dto.Image is null)
                return Error.Validation(
                    "Image.Required",
                    "Image is required");

            var imagePath = await _attachmentService
                .UploadAsync("cases", dto.Image);

            if (imagePath is null)
                return Error.Validation(
                    "Image.Invalid",
                    "Invalid image");
            var caseEntity = new Case
            {
                PatientId = patient.Id,
                RequiredSpecialization = dto.RequiredSpecialization,
                Description = dto.Description,
                City = dto.City,
                Status = CaseStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = imagePath
            };

            await _unitOfWork.GetRepository<Case, int>().AddAsync(caseEntity);
            await _unitOfWork.SaveChangesAsync();

            //foreach (var file in dto.Images)
            //{
            //    var path = await _attachmentService.UploadAsync("cases", file);
            //    if (path is null) continue;

            //    await _unitOfWork.GetRepository<CaseImage, int>().AddAsync(new CaseImage
            //    {
            //        CaseId = caseEntity.Id,
            //        ImageUrl = path,
            //        ImageType = "fsdfsdf"
            //    });
            //}

            var result = await _unitOfWork.GetRepository<Case, int>()
                .GetByIdAsync(new CaseWithImagesSpecification(caseEntity.Id));


            var dtoResult = _mapper.Map<CaseResponseDTO>(result);

            dtoResult.Image =
                $"{_configuration["URLs:BaseURL"]}{result!.ImageUrl}";

            return Result<CaseResponseDTO>.Ok(dtoResult);

        }


        public async Task<Result<IEnumerable<CaseResponseDTO>>> GetAvailableCasesAsync(string? city, string identityUserId)
        {

            var student = await _unitOfWork.GetRepository<Student, int>()
                .GetByIdAsync(new StudentByUserIdSpecification(identityUserId));

            if (student is null)
                return Error.NotFound("Student.NotFound");

            var cases = await _unitOfWork.GetRepository<Case, int>()
                .GetAllAsync(new AvailableCasesSpecification(city, student.Specializations));

            var baseUrl = _configuration["URLs:BaseURL"];
            var response = cases.Select(c =>
            {
                var dto = _mapper.Map<CaseResponseDTO>(c);
                dto.Image = $"{baseUrl}{c.ImageUrl}";
                return dto;
            });

            return Result<IEnumerable<CaseResponseDTO>>.Ok(response);
        }

        public async Task<Result<IEnumerable<CaseResponseDTO>>> GetMyCasesAsync(
            string userId,
            string role)
        {
            IEnumerable<Case> cases;

            if (role == "Patient")
            {
                var patient = await _unitOfWork.GetRepository<Patient, int>()
                    .GetByIdAsync(new PatientByUserIdSpecification(userId));

                if (patient is null)
                    return Error.NotFound("Patient.NotFound");

                cases = await _unitOfWork.GetRepository<Case, int>()
                    .GetAllAsync(new PatientCasesSpecification(patient.Id));
            }
            else if (role == "Student")
            {
                var student = await _unitOfWork.GetRepository<Student, int>()
                    .GetByIdAsync(new StudentByUserIdSpecification(userId));

                if (student is null)
                    return Error.NotFound("Student.NotFound");

                cases = await _unitOfWork.GetRepository<Case, int>()
                    .GetAllAsync(new StudentCasesSpecification(student.Id));
            }
            else
            {
                return Error.Unauthorized("Invalid.Role");
            }

            var baseUrl = _configuration["URLs:BaseURL"];

            var response = cases.Select(c =>
            {
                var dto = _mapper.Map<CaseResponseDTO>(c);
                dto.Image = $"{baseUrl}{c.ImageUrl}";
                return dto;
            });

            return Result<IEnumerable<CaseResponseDTO>>.Ok(response);
        }

        public async Task<Result<CaseResponseDTO>> GetCaseByIdAsync(int caseId)
        {
            var caseEntity = await _unitOfWork.GetRepository<Case, int>()
                .GetByIdAsync(new CaseWithImagesSpecification(caseId));

            if (caseEntity is null)
                return Error.NotFound("Case.NotFound");

            var baseUrl = _configuration["URLs:BaseURL"];

            var dto = _mapper.Map<CaseResponseDTO>(caseEntity);
            dto.Image = $"{baseUrl}{caseEntity.ImageUrl}";

            return Result<CaseResponseDTO>.Ok(dto);
        }

  


        #endregion


    }
}
