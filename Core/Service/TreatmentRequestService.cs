using AutoMapper;
using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;
using Domain.Interfaces;
using Service.Abstraction;
using Service.Specifications.CaseSpecifications;
using Service.Specifications.TreatmentRequestSpecificaition;
using Shared.CommonResult;
using Shared.DTOs.TreatmentRequestsDTOs;
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

        public TreatmentRequestService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result> AcceptRequestAsync(int requestId, string identityUserId)
        {
            var request =await _unitOfWork.GetRepository<TreatmentRequest, int>().GetByIdAsync(new TreatmentRequestWithDetailsSpecification(requestId));

            if (request is null) return Error.NotFound("Request.Notfound");

            if (request.Status != TreatmentRequestStatus.Pending) return Error.Failure("Request.NotPending");

            if(request.InitiatedBy == RequestInitiator.Student)
            {
                var patient = await _unitOfWork.GetRepository<Patient,int>()
                .GetByIdAsync(new PatientByUserIdSpecification(identityUserId));

                if (patient is null || request.Case.PatientId != patient.Id)
                    return  Error.Unauthorized("Request.Unauthorized");

            }
            else
            {
                var student = await _unitOfWork.GetRepository<Student,int>()
                    .GetByIdAsync(new StudentByUserIdSpecification(identityUserId));

                if (student is null || request.StudentId != student.Id)
                    return Error.Unauthorized("Request.Unauthorized");
            }
            request.Status = TreatmentRequestStatus.Accepted;
            request.Case.Status = CaseStatus.Assigned;

            _unitOfWork.GetRepository<TreatmentRequest,int>().Update(request);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();

        }

        public async Task<Result<IEnumerable<TreatmentRequestResponseDTO>>> GetRequestsByCaseAsync(int caseId, string identityUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient,int>()
            .GetByIdAsync(new PatientByUserIdSpecification(identityUserId));

            if (patient is null)
                return Error.NotFound("Patient.NotFound");

            var case_ = await _unitOfWork.GetRepository<Case,int>()
                .GetByIdAsync(new CaseWithImagesSpecification(caseId));

            if (case_ is null)
                return Error.NotFound("Case.NotFound");

            if (case_.PatientId != patient.Id)
                return Error.Unauthorized("Case.Unauthorized");

            var requests = await _unitOfWork.GetRepository<TreatmentRequest,int>()
                .GetAllAsync(new TreatmentRequestsByCaseSpecification(caseId));

            var result = _mapper.Map<IEnumerable<TreatmentRequestResponseDTO>>(requests);

            return Result<IEnumerable<TreatmentRequestResponseDTO>>.Ok(result);
        }

        public async Task<Result> PatientSendRequestAsync(int studentId, int caseId, string identityUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>().GetByIdAsync(new PatientByUserIdSpecification(identityUserId));

            if (patient == null) return Error.NotFound("Patient.NotFound");

            var case0 = await _unitOfWork.GetRepository<Case, int>().GetByIdAsync( caseId);

            if (case0 == null) return Error.NotFound("Case.NotFound");

            if (case0.PatientId != patient.Id) return Error.Unauthorized("It's not your case ");

            var student = await _unitOfWork.GetRepository<Student, int>().GetByIdAsync(studentId);

            if (student is null) return Error.NotFound("Student.NotFound");

            var existingRequest = await _unitOfWork.GetRepository<TreatmentRequest,int>()
                .GetByIdAsync(new TreatmentRequestByStudentAndCaseSpecification(studentId, caseId));

            if (existingRequest is not null) return Error.Failure("Request.AlreadySent");


            var request = new TreatmentRequest
            {
                CaseId = caseId,
                StudentId = studentId,
                InitiatedBy = RequestInitiator.Patient,
                Status = TreatmentRequestStatus.Pending
            };

            await _unitOfWork.GetRepository<TreatmentRequest,int>().AddAsync(request);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();

        }

        public async Task<Result> RejectUserAsync(int requestId, string identityUserId)
        {
            var request = await _unitOfWork.GetRepository<TreatmentRequest,int>()
               .GetByIdAsync(requestId);

            if (request is null)
                return Error.NotFound("Request.NotFound");

            if (request.Status != TreatmentRequestStatus.Pending)
                return Error.NotFound("Request.NotPending");

            if (request.InitiatedBy == RequestInitiator.Student)
            {
                var patient = await _unitOfWork.GetRepository<Patient,int>()
                    .GetByIdAsync(new PatientByUserIdSpecification(identityUserId));

                if (patient is null || request.Case.PatientId != patient.Id)
                    return Error.Unauthorized("Request.Unauthorized");
            }
            else
            {
                var student = await _unitOfWork.GetRepository<Student,int>()
                    .GetByIdAsync(new StudentByUserIdSpecification(identityUserId));

                if (student is null || request.StudentId != student.Id)
                    return Error.Unauthorized("Request.Unauthorized");
            }

            request.Status = TreatmentRequestStatus.Rejected;

            _unitOfWork.GetRepository<TreatmentRequest,int>().Update(request);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result> StudentSendRequestAsync(int caseId, string IdentityUserId)
        {
            var student =await _unitOfWork.GetRepository<Student,int>().GetByIdAsync(new StudentByUserIdSpecification(IdentityUserId));

            if(student is null)
                return Error.NotFound("Student.NotFound");

            if (!student.IsApproved)
                return  Error.Failure("Student.NotApproved");

            var case0 = await _unitOfWork.GetRepository<Case,int>()
                    .GetByIdAsync (new CaseWithImagesSpecification(caseId));

            if (case0 is null)
                return Error.NotFound("Case.NotFound");

            if (case0.Status != CaseStatus.Approved)
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

            await _unitOfWork.GetRepository<TreatmentRequest,int>().AddAsync(request);
           await _unitOfWork.SaveChangesAsync();

            return Result.Ok();




        }
    }
}
