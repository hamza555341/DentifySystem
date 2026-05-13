using AutoMapper;
using Domain.Entites.AppointmentModule;
using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;
using Domain.Interfaces;
using Service.Abstraction;
using Service.Specifications.AppointmentSpecifications;
using Service.Specifications.CaseSpecifications;
using Shared.CommonResult;
using Shared.DTOs.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobService _backgroundJobService;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IBackgroundJobService backgroundJobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _backgroundJobService = backgroundJobService;
        }

        // =============================
        // Auto Complete
        // =============================
        public async Task AutoCompleteAppointmentAsync(int appointmentId)
        {
            var repo = _unitOfWork.GetRepository<Appointment, int>();
            var appointment = await repo.GetByIdAsync(appointmentId);

            if (appointment is null) return;

            if (appointment.Status == AppointmentStatus.Confirmed)
            {
                appointment.Status = AppointmentStatus.Completed;
                repo.Update(appointment);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        // =============================
        // Get Patient Appointments
        // =============================
        public async Task<Result<IEnumerable<AppointmentResponseDTO>>> GetPatientAppointmentsAsync(string patientUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(patientUserId));

            if (patient is null)
                return Result<IEnumerable<AppointmentResponseDTO>>
                    .Fail(Error.NotFound("Patient.NotFound"));

            var appointments = await _unitOfWork.GetRepository<Appointment, int>()
                .GetAllAsync(new AppointmentsByPatientSpecification(patient.Id));

            return Result<IEnumerable<AppointmentResponseDTO>>.Ok(
                _mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments));
        }

        // =============================
        // Get Student Appointments
        // =============================
        public async Task<Result<IEnumerable<AppointmentResponseDTO>>> GetStudentAppointmentsAsync(string studentUserId)
        {
            var student = await _unitOfWork.GetRepository<Student, int>()
                .GetByIdAsync(new StudentByUserIdSpecification(studentUserId));

            if (student is null)
                return Result<IEnumerable<AppointmentResponseDTO>>
                    .Fail(Error.NotFound("Student.NotFound"));

            var appointments = await _unitOfWork.GetRepository<Appointment, int>()
                .GetAllAsync(new AppointmentsByStudentSpecification(student.Id));

            return Result<IEnumerable<AppointmentResponseDTO>>.Ok(
                _mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments));
        }

        // =============================
        // Propose Appointments
        // =============================
        public async Task<Result<IEnumerable<AppointmentResponseDTO>>> ProposeAppointmentsAsync(
            string studentUserId, ProposeAppointmentsDTO dto)
        {
            var studentRepo = _unitOfWork.GetRepository<Student, int>();
            var requestRepo = _unitOfWork.GetRepository<TreatmentRequest, int>();
            var appointmentRepo = _unitOfWork.GetRepository<Appointment, int>();

            var student = await studentRepo.GetByIdAsync(new StudentByUserIdSpecification(studentUserId));

            if (student is null)
                return Result<IEnumerable<AppointmentResponseDTO>>
                    .Fail(Error.NotFound("Student.NotFound"));

            var request = await requestRepo.GetByIdAsync(dto.TreatmentRequestId);

            if (request is null)
                return Result<IEnumerable<AppointmentResponseDTO>>
                    .Fail(Error.NotFound("Request.NotFound"));

            if (request.StudentId != student.Id)
                return Result<IEnumerable<AppointmentResponseDTO>>
                    .Fail(Error.Validation("Request.NotBelongToStudent"));

            if (request.Status != TreatmentRequestStatus.Accepted)
                return Result<IEnumerable<AppointmentResponseDTO>>
                    .Fail(Error.Validation("Request.NotAccepted"));

            var active = await appointmentRepo
                .GetAllAsync(new ActiveAppointmentsByRequestSpecification(request.Id));

            if (active.Any())
                return Result<IEnumerable<AppointmentResponseDTO>>
                    .Fail(Error.Validation("Appointments.Exists"));

            if (dto.Slots is null || dto.Slots.Count != 2)
                return Result<IEnumerable<AppointmentResponseDTO>>
                    .Fail(Error.Validation("Slots.Invalid"));

            var s1 = dto.Slots[0];
            var s2 = dto.Slots[1];

            if (s1.AppointmentDate == s2.AppointmentDate)
                return Result<IEnumerable<AppointmentResponseDTO>>
                    .Fail(Error.Validation("Slots.Duplicate"));

            if (s1.AppointmentDate <= DateTimeOffset.UtcNow ||
                s2.AppointmentDate <= DateTimeOffset.UtcNow)
                return Result<IEnumerable<AppointmentResponseDTO>>
                    .Fail(Error.Validation("Slots.InvalidDate"));

            var a1 = new Appointment
            {
                TreatmentRequestId = request.Id,
                AppointmentDate = s1.AppointmentDate,
                Location = s1.Location,
                Status = AppointmentStatus.proposed
            };

            var a2 = new Appointment
            {
                TreatmentRequestId = request.Id,
                AppointmentDate = s2.AppointmentDate,
                Location = s2.Location,
                Status = AppointmentStatus.proposed
            };

            await appointmentRepo.AddAsync(a1);
            await appointmentRepo.AddAsync(a2);

            await _unitOfWork.SaveChangesAsync();

            _backgroundJobService.ScheduleAppointmentCompletion(
                a1.Id, a1.AppointmentDate.UtcDateTime.AddHours(1));

            _backgroundJobService.ScheduleAppointmentCompletion(
                a2.Id, a2.AppointmentDate.UtcDateTime.AddHours(1));

            return Result<IEnumerable<AppointmentResponseDTO>>.Ok(
                _mapper.Map<IEnumerable<AppointmentResponseDTO>>(new[] { a1, a2 }));
        }

        // =============================
        // Reject All (Patient)
        // =============================
        public async Task<Result> RejectAllAppointmentsAsync(int requestId, string patientUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(patientUserId));

            if (patient is null)
                return Result.Fail(Error.NotFound("Patient.NotFound"));

            var appointments = await _unitOfWork.GetRepository<Appointment, int>()
                .GetAllAsync(new ProposedAppointmentsByRequestSpecification(requestId));

            foreach (var a in appointments)
                a.Status = AppointmentStatus.Rejected;

            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        // =============================
        // Confirm Appointment
        // =============================
        public async Task<Result> SelectAppointmentAsync(int appointmentId, string patientUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(patientUserId));

            if (patient is null)
                return Result.Fail(Error.NotFound("Patient.NotFound"));

            var appointmentRepo = _unitOfWork.GetRepository<Appointment, int>();

            var appointment = await appointmentRepo
                .GetByIdAsync(new AppointmentWithRequestSpecification(appointmentId));

            if (appointment is null)
                return Result.Fail(Error.NotFound("Appointment.NotFound"));

            if (appointment.Status != AppointmentStatus.proposed)
                return Result.Fail(Error.Validation("Invalid.Status"));

            
            if (appointment.TreatmentRequest.Case.PatientId != patient.Id)
                return Result.Fail(Error.Unauthorized("Access.Denied"));

            var others = await appointmentRepo.GetAllAsync(
                new ProposedAppointmentsByRequestSpecification(appointment.TreatmentRequestId));

            appointment.Status = AppointmentStatus.Confirmed;

            foreach (var o in others)
            {
                if (o.Id != appointment.Id)
                    o.Status = AppointmentStatus.Cancelled;
            }

            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
