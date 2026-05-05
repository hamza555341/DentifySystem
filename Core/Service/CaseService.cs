using AutoMapper;
using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;
using Domain.Interfaces;
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
                return Error.NotFound("Patient.NotFound");

            if (dto.Images is null || !dto.Images.Any())
                return Error.Validation("Images.Required", "At least one image is required");

            var caseEntity = new Case
            {
                PatientId = patient.Id,
                Disease = dto.Disease,
                Description = dto.Description,
                City = dto.City,
                Status = CaseStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<Case, int>().AddAsync(caseEntity);
            await _unitOfWork.SaveChangesAsync();

            foreach (var file in dto.Images)
            {
                var path = await _attachmentService.UploadAsync("cases", file);
                if (path is null) continue;

                await _unitOfWork.GetRepository<CaseImage, int>().AddAsync(new CaseImage
                {
                    CaseId = caseEntity.Id,
                    ImageUrl = path,
                    ImageType = "fsdfsdf"
                });
            }

            await _unitOfWork.SaveChangesAsync();

            var result = await _unitOfWork.GetRepository<Case, int>()
                .GetByIdAsync(new CaseWithImagesSpecification(caseEntity.Id));


            var baseUrl = _configuration["URLs:BaseURL"];
            var caseResponse = _mapper.Map<CaseResponseDTO>(result!);

            caseResponse.Images = result!.Images
                .Select(i => $"{baseUrl}{i.ImageUrl}")
                .ToList();

            return Result<CaseResponseDTO>.Ok(caseResponse);

        }


        public async Task<Result<IEnumerable<CaseResponseDTO>>> GetAvailableCasesAsync(string? city)
        {
            var cases = await _unitOfWork.GetRepository<Case, int>()
                .GetAllAsync(new AvailableCasesSpecification(city));

            var baseUrl = _configuration["URLs:BaseURL"];

            var response = cases.Select(c =>
            {
                var dto = _mapper.Map<CaseResponseDTO>(c);
                dto.Images = c.Images
                    .Select(i => $"{baseUrl}{i.ImageUrl}")
                    .ToList();
                return dto;
            });

            return Result<IEnumerable<CaseResponseDTO>>.Ok(response);
        }

        public async Task<Result<IEnumerable<CaseResponseDTO>>> GetPatientCasesAsync(string userId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(userId));

            if (patient is null)
                return Error.NotFound("Patient.NotFound");

            var cases = await _unitOfWork.GetRepository<Case, int>()
                .GetAllAsync(new PatientCasesSpecification(patient.Id));

            var baseUrl = _configuration["URLs:BaseURL"];

            var response = cases.Select(c =>
            {
                var dto = _mapper.Map<CaseResponseDTO>(c);
                dto.Images = c.Images
                    .Select(i => $"{baseUrl}{i.ImageUrl}")
                    .ToList();
                return dto;
            });

            return Result<IEnumerable<CaseResponseDTO>>.Ok(response);
        }


        public async Task<Result<IEnumerable<CaseResponseDTO>>> GetStudentCasesAsync(string userId)
        {
            var student = await _unitOfWork.GetRepository<Student, int>()
                .GetByIdAsync(new StudentByUserIdSpecification(userId));

            if (student is null)
                return Error.NotFound("Student.NotFound");

            var cases = await _unitOfWork.GetRepository<Case, int>()
                .GetAllAsync(new StudentCasesSpecification(student.Id));

            var baseUrl = _configuration["URLs:BaseURL"];

            var response = cases.Select(c =>
            {
                var dto = _mapper.Map<CaseResponseDTO>(c);
                dto.Images = c.Images
                    .Select(i => $"{baseUrl}{i.ImageUrl}")
                    .ToList();
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
            dto.Images = caseEntity.Images
                .Select(i => $"{baseUrl}{i.ImageUrl}")
                .ToList();

            return Result<CaseResponseDTO>.Ok(dto);
        }

        public async Task<Result> AcceptTreatmentRequestAsync(int requestId, string patientUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(patientUserId));

            if (patient is null)
                return Result.Failure(Error.NotFound("Patient.NotFound"));

            var requestRepo = _unitOfWork.GetRepository<TreatmentRequest, int>();

            var request = await requestRepo.GetByIdAsync(
                new TreatmentRequestWithDetailsSpecification(requestId));

            if (request is null)
                return Result.Failure(Error.NotFound("Request.NotFound"));

            var caseEntity = request.Case;

            if (caseEntity.PatientId != patient.Id)
                return Result.Failure(Error.Unauthorized("Not.Allowed"));

            if (request.Status != TreatmentRequestStatus.Pending)
                return Result.Failure(Error.Validation("Invalid.Status", "Request already handled"));

            // ✅ 1. قبول الطلب
            request.Status = TreatmentRequestStatus.Accepted;

            // ✅ 2. رفض باقي الطلبات
            var allRequests = caseEntity.TreatmentRequests;

            foreach (var r in allRequests)
            {
                if (r.Id != request.Id)
                    r.Status = TreatmentRequestStatus.Rejected;
            }

            // ✅ 3. تحديث حالة الكيس
            caseEntity.Status = CaseStatus.Assigned;

            requestRepo.Update(request);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result> ApproveCaseAsync(int caseId)
        {
            var caseRepo = _unitOfWork.GetRepository<Case, int>();
            var caseEntity = await caseRepo.GetByIdAsync(caseId);

            if (caseEntity is null)
                return Result.Failure(Error.NotFound("Case.NotFound"));

            if (caseEntity.Status != CaseStatus.Pending)
                return Result.Failure(Error.Validation("Invalid.Status", "Case is not pending"));

            caseEntity.Status = CaseStatus.Approved;
            caseRepo.Update(caseEntity);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result> RejectCaseAsync(int caseId)
        {
            var caseRepo = _unitOfWork.GetRepository<Case, int>();
            var caseEntity = await caseRepo.GetByIdAsync(caseId);

            if (caseEntity is null)
                return Result.Failure(Error.NotFound("Case.NotFound"));

            caseEntity.Status = CaseStatus.Rejected;
            caseRepo.Update(caseEntity);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        } 
        #endregion


    }
}
