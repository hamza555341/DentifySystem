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
        private readonly IAiDiagnosisService  _aiService;
        private readonly IConfiguration _configuration;

        public CaseService(IUnitOfWork unitOfWork,
            IAttachmentService attachmentService, IMapper mapper,IConfiguration configuration,
           IAiDiagnosisService aiService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
            _mapper = mapper;
            _aiService = aiService;
            _configuration = configuration;
        }

        #region paitient
        public async Task<Result<CaseResponseDTO>> CreateCaseAsync(string userId, CreateCaseDTO dto)
        {
            //      var patient = await _unitOfWork.GetRepository<Patient, int>()
            //          .GetByIdAsync(new PatientByUserIdSpecification(userId));

            //      if (patient is null) 
            //      { 
            //          return Error.NotFound("Patient.NotFound");
            //      }

            //      var hasActiveCase = await _unitOfWork.GetRepository<Case, int>()
            //          .GetAllAsync(new PatientActiveCaseSpecification(patient!.Id));

            //      if (hasActiveCase.Any())
            //          return Error.Validation("Case.ActiveExists", "You already have an Pending case");

            //      if (dto.Image is null)
            //          return Error.Validation(
            //              "Image.Required",
            //              "Image is required");

            //      var imagePath = await _attachmentService
            //          .UploadAsync("cases", dto.Image);

            //      if (imagePath is null)
            //          return Error.Validation(
            //              "Image.Invalid",
            //              "Invalid image");
            //      var caseEntity = new Case
            //      {
            //          PatientId = patient.Id,
            ////          RequiredSpecialization = dto.RequiredSpecialization,
            ////         Description = dto.Description,
            //          City = dto.City,
            //          Status = CaseStatus.Pending,
            //          CreatedAt = DateTime.UtcNow,
            //          ImageUrl = imagePath
            //      };

            //      await _unitOfWork.GetRepository<Case, int>().AddAsync(caseEntity);
            //      await _unitOfWork.SaveChangesAsync();

            //      var result = await _unitOfWork.GetRepository<Case, int>()
            //          .GetByIdAsync(new CaseWithImagesSpecification(caseEntity.Id));


            //      var dtoResult = _mapper.Map<CaseResponseDTO>(result);

            //      dtoResult.Image =
            //          $"{_configuration["URLs:BaseURL"]}{result!.ImageUrl}";

            //      return Result<CaseResponseDTO>.Ok(dtoResult);

            // 1. جيب المريض
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(userId));

            if (patient is null)
                return Error.NotFound("Patient.NotFound");

            // 2. تأكد مفيش case active
            var hasActiveCase = await _unitOfWork.GetRepository<Case, int>()
                .GetAllAsync(new PatientActiveCaseSpecification(patient.Id));

            if (hasActiveCase.Any())
                return Error.Validation("Case.ActiveExists",
                    "You already have an active case");

            // 3. تأكد إن فيه صورة أو تيكست
            if (dto.Image is null && string.IsNullOrWhiteSpace(dto.SymptomsText))
                return Error.Validation("Input.Required",
                    "Please provide an image or describe your symptoms");

            // 4. كلم الـ AI
            AiResult aiResult;

            if (dto.Image is not null)
            {
                aiResult = await _aiService.AnalyzeImageAsync(
                    dto.Image,
                    dto.PainDuration,
                    dto.ChronicDiseases);
            }
            else
            {
                aiResult = await _aiService.AnalyzeTextAsync(dto.SymptomsText!);
            }

            // 5. صورة مش أسنان
            if (!aiResult.IsValidDentalImage)
                return Error.Validation("Image.Invalid",
                    "Please upload a clear dental image");

            // 6. أسنان سليمة — عرض التقرير بس بدون إنشاء Case
            if (aiResult.IsHealthy)
                return Error.Validation("Case.Healthy",
                    aiResult.FullReport);

            // 7. حدد الـ Specialization
            var specialization = MapDiagnosis(aiResult.Diagnosis!);

            if (specialization == Specialization.None)
                return Error.Validation("Diagnosis.Unsupported",
                    "Could not determine the required specialization");

            // 8. رفع الصورة لو موجودة
            string? imagePath = null;

            if (dto.Image is not null)
            {
                imagePath = await _attachmentService
                    .UploadAsync("cases", dto.Image);

                if (imagePath is null)
                    return Error.Validation("Image.UploadFailed",
                        "Failed to upload image");
            }

            // 9. إنشاء الـ Case
            var caseEntity = new Case
            {
                PatientId = patient.Id,
                RequiredSpecialization = specialization,
    //            Description = dto.Description,
                City = dto.City,
                Status = CaseStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = imagePath ?? string.Empty,
                AiAnalysisResult = aiResult.FullReport
            };

            await _unitOfWork.GetRepository<Case, int>().AddAsync(caseEntity);
            await _unitOfWork.SaveChangesAsync();

            var result = await _unitOfWork.GetRepository<Case, int>()
                .GetByIdAsync(new CaseWithImagesSpecification(caseEntity.Id));

            var dtoResult = _mapper.Map<CaseResponseDTO>(result!);

            if (!string.IsNullOrEmpty(result!.ImageUrl))
                dtoResult.Image = $"{_configuration["URLs:BaseURL"]}{result.ImageUrl}";

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

        public async Task<Result> EditCase(int caseId, EditCaseDTO dto)
        {
            var case0=await _unitOfWork.GetRepository<Case, int>().GetByIdAsync(new CaseWithImagesSpecification(caseId));
            if (case0 is null) return Error.NotFound("Case.NotFound");
            if (case0.Status != CaseStatus.Pending)
                return Error.Validation("Case.NotEditable", "Only pending cases can be edited");
            if (!string.IsNullOrEmpty(dto.Description) && dto.Description != "string")
                case0.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.City) && dto.City != "string")
                case0.City = dto.City;
           case0.RequiredSpecialization = dto.RequiredSpecialization;
            if (dto.Image is not null)
            {
                var imagePath = await _attachmentService.UploadAsync("cases", dto.Image);
                if (imagePath is not null)
                    case0.ImageUrl = imagePath;
            }
            _unitOfWork.GetRepository<Case, int>().Update(case0);
             await _unitOfWork.SaveChangesAsync();
            return Result.Ok();


        }

        public async Task<Result> DeleteCase(int caseId)
        {
            var Case0=await _unitOfWork.GetRepository<Case,int>().GetByIdAsync(caseId);
            if (Case0 is null) return Error.NotFound("Case Not Found");
            if(Case0.Status != CaseStatus.Pending) return Error.Validation("Case.NotDeletable", "Only pending cases can be deleted");
            _unitOfWork.GetRepository<Case, int>().Delete(Case0);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }






        #endregion


    }
}
