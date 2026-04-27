using AutoMapper;
using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.ReportModule;
using Domain.Entites.StudentModule;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Service.Abstraction;
using Service.Specifications.CaseSpecifications;
using Service.Specifications.ReportSpecifications;
using Shared.CommonResult;
using Shared.DTOs.ReportDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ReportService(IUnitOfWork unitOfWork,
            IAttachmentService attachmentService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<Result<ReportResponseDTO>> CreateReportAsync(
            string userId, CreateReportDTO dto)
        {
            var student = await _unitOfWork.GetRepository<Student, int>()
                .GetByIdAsync(new StudentByUserIdSpecification(userId));

            if (student is null)
                return Error.NotFound("Student.NotFound");

            //if (!student.IsApproved)
            //    return Error.Validation("Student.NotApproved",
            //        "Your account is not approved yet");

            var caseEntity = await _unitOfWork.GetRepository<Case, int>()
                .GetByIdAsync(dto.CaseId);

            if (caseEntity is null)
                return Error.NotFound("Case.NotFound");

            if (caseEntity.AssignedStudentId != student.Id)
                return Error.Validation("Case.NotAssigned",
                    "This case is not assigned to you");

            if (caseEntity.Status != CaseStatus.Assigned)
                return Error.Validation("Case.InvalidStatus",
                    "Case must be assigned before submitting a report");

            var report = new Report
            {
                CaseId = dto.CaseId,
                StudentId = student.Id,
                Diagnosis = dto.Diagnosis,
                TreatmentPlan = dto.TreatmentPlan,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow,
                Images = new List<ReportImage>()
            };

            if (dto.Images is not null && dto.Images.Any())
            {
                foreach (var file in dto.Images)
                {
                    var path = await _attachmentService.UploadAsync("reports", file);
                    if (path is null) continue;

                    report.Images.Add(new ReportImage
                    {
                        ImageUrl = path,
                        UploadedAt = DateTime.UtcNow
                    });
                }
            }

            await _unitOfWork.GetRepository<Report, int>().AddAsync(report);

            caseEntity.Status = CaseStatus.Completed;
            _unitOfWork.GetRepository<Case, int>().Update(caseEntity);

            await _unitOfWork.SaveChangesAsync();

            var result = await _unitOfWork.GetRepository<Report, int>()
                .GetByIdAsync(new ReportWithDetailsSpecification(report.Id));

            var response = _mapper.Map<ReportResponseDTO>(result!);
            var baseUrl = _configuration["URLs:BaseURL"];
            response.Images = result!.Images
                .Select(i => $"{baseUrl}{i.ImageUrl}")
                .ToList();

            return Result<ReportResponseDTO>.Ok(response);
        }

        public async Task<Result<ReportResponseDTO>> GetReportByCaseAsync(int caseId,string userId)
        {
            var caseEntity = await _unitOfWork.GetRepository<Case, int>()
          .GetByIdAsync(caseId);

            if (caseEntity is null)
                return Error.NotFound("Case.NotFound");

            // تأكد إن الـ userId مرتبط بالحالة دي
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(userId));

            var student = await _unitOfWork.GetRepository<Student, int>()
                .GetByIdAsync(new StudentByUserIdSpecification(userId));

            bool isOwner = (patient is not null && caseEntity.PatientId == patient.Id) ||
                           (student is not null && caseEntity.AssignedStudentId == student.Id);

            if (!isOwner)
                return Error.Validation("Access.Denied", "You don't have access to this case");

            var report = await _unitOfWork.GetRepository<Report, int>()
                .GetByIdAsync(new ReportByCaseSpecification(caseId));

            if (report is null)
                return Error.NotFound("Report.NotFound");

            var baseUrl = _configuration["URLs:BaseURL"];
            var response = _mapper.Map<ReportResponseDTO>(report);
            response.Images = report.Images
                .Select(i => $"{baseUrl}/images/reports/{i.ImageUrl}")
                .ToList();

            return Result<ReportResponseDTO>.Ok(response);
        }

        public async Task<Result<IEnumerable<ReportResponseDTO>>> GetStudentReportsAsync(
            string userId)
        {
            var student = await _unitOfWork.GetRepository<Student, int>()
                .GetByIdAsync(new StudentByUserIdSpecification(userId));

            if (student is null)
                return Error.NotFound("Student.NotFound");

            var reports = await _unitOfWork.GetRepository<Report, int>()
                .GetAllAsync(new ReportsByStudentSpecification(student.Id));

            var baseUrl = _configuration["URLs:BaseURL"];

            var response = reports.Select(r =>
            {
                var dto = _mapper.Map<ReportResponseDTO>(r);
                dto.Images = r.Images
                    .Select(i => $"{baseUrl}{i.ImageUrl}")
                    .ToList();
                return dto;
            });

            return Result<IEnumerable<ReportResponseDTO>>.Ok(response);
        }
    }
}
