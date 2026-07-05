using AutoMapper;
using Domain.Entites.CaseModule;
using Domain.Entites.Notifications;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Service.Abstraction;
using Service.Specifications.CaseSpecifications;
using Service.Specifications.StudentRatingSpecifications;
using Service.Specifications.StudentSpecification;
using Service.Specifications.TreatmentRequestSpecificaition;
using Shared.CommonResult;
using Shared.DTOs.TreatmentRequestsDTOs;
using Shared.DTOs.TreatmentRequestsDTOs.Shared.DTOs.TreatmentRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TreatmentRequestService : ITreatmentRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ILogger<TreatmentRequestService> _logger;
        private readonly INotificationHubService _notificationHubService;

        public TreatmentRequestService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService, ILogger<TreatmentRequestService> logger,INotificationHubService notificationHubService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _logger = logger;
            _notificationHubService = notificationHubService;
        }

        public async Task<Result> AcceptRequestAsync(int requestId, string identityUserId)
        {
            var request = await _unitOfWork.GetRepository<TreatmentRequest, int>().GetByIdAsync(new TreatmentRequestWithDetailsSpecification(requestId));

            if (request is null) return Error.NotFound("Request.Notfound");

            if (request.Status != TreatmentRequestStatus.Pending) return Error.Failure("Request.NotPending");

            if (request.InitiatedBy == RequestInitiator.Student)
            {
                var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(identityUserId));

                if (patient is null || request.Case.PatientId != patient.Id)
                    return Error.Unauthorized("Request.Unauthorized");
            }

            else
            {
                var student = await _unitOfWork.GetRepository<Student, int>()
                    .GetByIdAsync(new StudentByUserIdSpecification(identityUserId));

                if (student is null || request.StudentId != student.Id)
                    return Error.Unauthorized("Request.Unauthorized");
            }

            request.Status = TreatmentRequestStatus.Accepted;


            request.Case.Status = CaseStatus.Assigned;


            var others = await _unitOfWork.GetRepository<TreatmentRequest, int>()
                .GetAllAsync(new TreatmentRequestsByCaseSpecification(request.CaseId));

            foreach (var r in others)
            {
                if (r.Id != request.Id && r.Status == TreatmentRequestStatus.Pending)
                    r.Status = TreatmentRequestStatus.Rejected;
            }

            _unitOfWork.GetRepository<TreatmentRequest, int>().Update(request);
            await _unitOfWork.SaveChangesAsync();

            var receiverId = request.InitiatedBy == RequestInitiator.Student
    ? request.Student.IdentityUserId
    : request.Case.Patient.IdentityUserId;

            var notificationResult = await _notificationService.CreateNotificationAsync(
                receiverId: receiverId,
                senderId: request.InitiatedBy == RequestInitiator.Student
                    ? request.Case.Patient.IdentityUserId
                    : request.Student.IdentityUserId,
                type: NotificationType.RequestAccepted,
                referenceId: request.Id);

            if (notificationResult.IsFailure)
            {
                _logger.LogError(
                "Failed to create notification for request {RequestId}",
                request.Id);
            }
            else
            {
                var n = notificationResult.Value;
                await _notificationHubService.SendAsync(
                    receiverId,
                    n.Title,
                    n.Message,
                    (int)n.Type,
                    n.ReferenceId,
                    n.CreatedAt);
            }

            return Result.Ok();

        }

        public async Task<Result<IEnumerable<TreatmentRequestResponseDTO>>> GetRequestsByCaseAsync(string identityUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(identityUserId));

            if (patient is null)
                return Error.NotFound("Patient.NotFound");

            var activeCase = (await _unitOfWork
       .GetRepository<Case, int>()
       .GetAllAsync(
           new PatientActiveCaseSpecification(patient.Id)))
       .FirstOrDefault();

            if (activeCase is null)
                return Error.NotFound("Case.NoActiveCase");

            if (activeCase.Status == CaseStatus.Assigned)
                return Result<IEnumerable<TreatmentRequestResponseDTO>>
                    .Ok([]);

            var caseEntity = await _unitOfWork.GetRepository<Case, int>()
                .GetByIdAsync(new CaseWithImagesSpecification(activeCase.Id));

            if (caseEntity is null)
                return Error.NotFound("Case.NotFound");

            if (caseEntity.PatientId != patient.Id)
                return Error.Unauthorized("Case.Unauthorized");

            var requests = await _unitOfWork.GetRepository<TreatmentRequest, int>()
                .GetAllAsync(new TreatmentRequestsByCaseSpecification(activeCase.Id));

            var result = new List<TreatmentRequestResponseDTO>();

            foreach (var request in requests)
            {
                var ratings = await _unitOfWork.GetRepository<StudentRating, int>()
                    .GetAllAsync(new RatingsByStudentSpecification(request.StudentId));

                var ratingList = ratings.ToList();
                var average = ratingList.Any()
                    ? Math.Round(ratingList.Average(r => r.Rating), 1)
                    : 0.0;

                var dto = _mapper.Map<TreatmentRequestResponseDTO>(request);
                dto.AverageRating = average;
                dto.TotalRatings = ratingList.Count;

                result.Add(dto);
            }

            return Result<IEnumerable<TreatmentRequestResponseDTO>>.Ok(result);
        }

        public async Task<Result> PatientSendRequestAsync(int studentId, int caseId, string identityUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>().GetByIdAsync(new PatientByUserIdSpecification(identityUserId));

            if (patient == null) return Error.NotFound("Patient.NotFound");

            var case0 = await _unitOfWork.GetRepository<Case, int>().GetByIdAsync(caseId);

            if (case0 == null) return Error.NotFound("Case.NotFound");

            if (case0.Status != CaseStatus.Pending)
                return Error.Failure("Case.NotAvailable");

            

            if (case0.PatientId != patient.Id) return Error.Unauthorized("It's not your case ");

            var student = await _unitOfWork.GetRepository<Student, int>().GetByIdAsync(studentId);

            if (student is null) return Error.NotFound("Student.NotFound");

            var existingRequest = await _unitOfWork.GetRepository<TreatmentRequest, int>()
                .GetByIdAsync(new TreatmentRequestByStudentAndCaseSpecification(studentId, caseId));

            if (existingRequest is not null) return Error.Failure("Request.AlreadySent");


            var request = new TreatmentRequest
            {
                CaseId = caseId,
                StudentId = studentId,
                InitiatedBy = RequestInitiator.Patient,
                Status = TreatmentRequestStatus.Pending
            };

            await _unitOfWork.GetRepository<TreatmentRequest, int>().AddAsync(request);
            await _unitOfWork.SaveChangesAsync();

            var receiverId = student.IdentityUserId;

            var notificationResult = await _notificationService.CreateNotificationAsync(
                receiverId: receiverId,
                senderId: patient.IdentityUserId,
                type: NotificationType.Request,
                referenceId: request.Id);

            if (notificationResult.IsFailure)
            {
                _logger.LogError(
                "Failed to create notification for request {RequestId}",
                request.Id);
            }
            else
            {
                var n = notificationResult.Value;
                await _notificationHubService.SendAsync(
                    receiverId,
                    n.Title,
                    n.Message,
                    (int)n.Type,
                    n.ReferenceId,
                    n.CreatedAt);
            }


            return Result.Ok();

        }

        public async Task<Result> RejectUserAsync(int requestId, string identityUserId)
        {
            var request = await _unitOfWork.GetRepository<TreatmentRequest, int>()
               .GetByIdAsync(new TreatmentRequestWithDetailsSpecification(requestId));

            if (request is null)
                return Error.NotFound("Request.NotFound");

            if (request.Status != TreatmentRequestStatus.Pending)
                return Error.NotFound("Request.NotPending");

            if (request.InitiatedBy == RequestInitiator.Student)
            {
                var patient = await _unitOfWork.GetRepository<Patient, int>()
                    .GetByIdAsync(new PatientByUserIdSpecification(identityUserId));

                if (patient is null || request.Case.PatientId != patient.Id)
                    return Error.Unauthorized("Request.Unauthorized");
            }
            else
            {
                var student = await _unitOfWork.GetRepository<Student, int>()
                    .GetByIdAsync(new StudentByUserIdSpecification(identityUserId));

                if (student is null || request.StudentId != student.Id)
                    return Error.Unauthorized("Request.Unauthorized");
            }

            request.Status = TreatmentRequestStatus.Rejected;

            _unitOfWork.GetRepository<TreatmentRequest, int>().Update(request);
            await _unitOfWork.SaveChangesAsync();

            var receiverId = request.InitiatedBy == RequestInitiator.Student
            ? request.Student.IdentityUserId
            : request.Case.Patient.IdentityUserId;

            var notificationResult = await _notificationService.CreateNotificationAsync(
                receiverId: receiverId,
                senderId: request.InitiatedBy == RequestInitiator.Student
                    ? request.Case.Patient.IdentityUserId
                    : request.Student.IdentityUserId,
                type: NotificationType.RequestRejected,
                referenceId: request.Id);

            if (notificationResult.IsFailure)
            {
                _logger.LogError(
                "Failed to create notification for request {RequestId}",
                request.Id);
            }
            else
            {
                var n = notificationResult.Value;
                await _notificationHubService.SendAsync(
                    receiverId,
                    n.Title,
                    n.Message,
                    (int)n.Type,
                    n.ReferenceId,
                    n.CreatedAt);
            }

            return Result.Ok();
        }

        public async Task<Result> StudentSendRequestAsync(int caseId, string IdentityUserId)
        {
            var student = await _unitOfWork.GetRepository<Student, int>().GetByIdAsync(new StudentByUserIdSpecification(IdentityUserId));

            if (student is null)
                return Error.NotFound("Student.NotFound");

            if (!student.IsActive)
                return Error.Failure("Student.NotApproved");

            var case0 = await _unitOfWork.GetRepository<Case, int>()
                    .GetByIdAsync(new CaseWithImagesSpecification(caseId));

            if (case0 is null)
                return Error.NotFound("Case.NotFound");

            if (case0.Status != CaseStatus.Pending)
                return Error.Failure("The case Is not approved ");

            var existingRequest = await _unitOfWork.GetRepository<TreatmentRequest, int>()
                .GetByIdAsync(new TreatmentRequestByStudentAndCaseSpecification(student.Id, caseId));

            if (existingRequest is not null)
                return Error.Failure("Request Is already exist");

            var request = new TreatmentRequest
            {
                CaseId = caseId,
                StudentId = student.Id,
                InitiatedBy = RequestInitiator.Student,
                Status = TreatmentRequestStatus.Pending
            };

            await _unitOfWork.GetRepository<TreatmentRequest, int>().AddAsync(request);
            await _unitOfWork.SaveChangesAsync();
            var receiverId = case0.Patient.IdentityUserId;

            var notificationResult = await _notificationService.CreateNotificationAsync(
                receiverId: receiverId,
                senderId: student.IdentityUserId,
                type: NotificationType.Request,
                referenceId: request.Id);

            if (notificationResult.IsFailure)
            {
                _logger.LogError(
                "Failed to create notification for request {RequestId}",
                request.Id);
            }
            else
            {
                var n = notificationResult.Value;
                await _notificationHubService.SendAsync(
                    receiverId,
                    n.Title,
                    n.Message,
                    (int)n.Type,
                    n.ReferenceId,
                    n.CreatedAt);
            }

            return Result.Ok();




        }

        public async Task<Result<IEnumerable<StudentRequestResponseDTO>>> GetStudentRequestsAsync(string identityUserId)
        {
            var student = await _unitOfWork
                .GetRepository<Student, int>()
                .GetByIdAsync(
                    new StudentByUserIdSpecification(
                        identityUserId));

            if (student is null)
                return Error.NotFound(
                    "Student.NotFound");

            var requests = await _unitOfWork
                .GetRepository<TreatmentRequest, int>()
                .GetAllAsync(
                    new StudentPendingRequestsSpecification(
                        student.Id));

            var result = _mapper.Map
                <IEnumerable<StudentRequestResponseDTO>>
                (requests);

            return Result<
                IEnumerable<StudentRequestResponseDTO>>
                .Ok(result);
        }

        public async Task<Result<IEnumerable<StudentRequestResponseDTO>>> GetPatientRequestsToStudentAsync(string identityUserId)
        {
            var student = await _unitOfWork.GetRepository<Student, int>()
                .GetByIdAsync(new StudentByUserIdSpecification(identityUserId));

            if (student is null)
                return Error.NotFound("Student.NotFound");

            var requests = await _unitOfWork.GetRepository<TreatmentRequest, int>()
                .GetAllAsync(new PatientRequestsToStudentSpecification(student.Id));

            var result = _mapper.Map<IEnumerable<StudentRequestResponseDTO>>(requests);
            return Result<IEnumerable<StudentRequestResponseDTO>>.Ok(result);
        }

        public async Task<Result<IEnumerable<TreatmentRequestResponseDTO>>> GetPatientSentRequestsAsync(string identityUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(identityUserId));

            if (patient is null)
                return Error.NotFound("Patient.NotFound");

            var requests = await _unitOfWork.GetRepository<TreatmentRequest, int>()
                .GetAllAsync(new PatientSentRequestsSpecification(patient.Id));

            var result = new List<TreatmentRequestResponseDTO>();

            foreach (var request in requests)
            {
                var ratings = await _unitOfWork.GetRepository<StudentRating, int>()
                    .GetAllAsync(new RatingsByStudentSpecification(request.StudentId));

                var ratingList = ratings.ToList();
                var average = ratingList.Any()
                    ? Math.Round(ratingList.Average(r => r.Rating), 1)
                    : 0.0;

                var dto = _mapper.Map<TreatmentRequestResponseDTO>(request);
                dto.AverageRating = average;
                dto.TotalRatings = ratingList.Count;

                result.Add(dto);
            }

            return Result<IEnumerable<TreatmentRequestResponseDTO>>.Ok(result);
        }

    }
}
