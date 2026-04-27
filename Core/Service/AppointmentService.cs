using AutoMapper;
using Domain.Entites.AppointmentModule;
using Domain.Entites.CaseModule;
using Domain.Entites.PatientModule;
using Domain.Entites.StudentModule;
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

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper,IBackgroundJobService backgroundJobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _backgroundJobService = backgroundJobService;
        }



        public async Task AutoCompleteAppointmentAsync(int appointmentId)
        {
            var appointmentRepo = _unitOfWork.GetRepository<Appointment, int>();

            var appointment = await appointmentRepo.GetByIdAsync(appointmentId);


            if (appointment is null)
            {
                return;
            }

            // check Appointment status is confirmed before marking it as completed
            if (appointment.Status == AppointmentStatus.Confirmed)
                
            {
                appointment.Status = AppointmentStatus.Completed;
                appointmentRepo.Update(appointment);
                await _unitOfWork.SaveChangesAsync();

            }
        }

        public async Task<Result<IEnumerable<AppointmentResponseDTO>>> GetCaseAppointmentsAsync(
      int caseId, string userId)
        {
            var caseEntity = await _unitOfWork.GetRepository<Case, int>()
                .GetByIdAsync(new CaseWithPatientAndStudentSpecification(caseId));

            if (caseEntity is null)
                return Error.NotFound("Case.NotFound");

            // check ownership بشكل مباشر
            var isOwner = caseEntity.Patient.IdentityUserId == userId;
            var isAssignedStudent = caseEntity.AssignedStudent != null &&
                                    caseEntity.AssignedStudent.IdentityUserId == userId;

            if (!isOwner && !isAssignedStudent)
                return Error.Unauthorized("Access.Denied");

            var appointments = await _unitOfWork.GetRepository<Appointment, int>()
                .GetAllAsync(new AppointmentsByCaseSpecification(caseId));

            return Result<IEnumerable<AppointmentResponseDTO>>.Ok(
                _mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments));
        }

        public async Task<Result<IEnumerable<AppointmentResponseDTO>>> GetPatientAppointmentsAsync(string patientUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
           .GetByIdAsync(new PatientByUserIdSpecification(patientUserId));


            if (patient is null)
                return Error.NotFound("Patient.NotFound");

            var appointments = await _unitOfWork.GetRepository<Appointment, int>()
                .GetAllAsync(new AppointmentsByPatientSpecification(patient.Id));

            if (appointments.Any(a => a.PatientId != patient.Id))
                     return Error.Unauthorized("Access.Denied");

            return Result<IEnumerable<AppointmentResponseDTO>>.Ok(
                _mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments));    
        }

        public async Task<Result<IEnumerable<AppointmentResponseDTO>>> GetStudentAppointmentsAsync(string studentUserId)
        {
            var student = await _unitOfWork.GetRepository<Student, int>()
            .GetByIdAsync(new StudentByUserIdSpecification(studentUserId));

            if (student is null)
                return Error.NotFound("Student.NotFound");

            var appointments = await _unitOfWork.GetRepository<Appointment, int>()
                .GetAllAsync(new AppointmentsByStudentSpecification(student.Id));

            return Result<IEnumerable<AppointmentResponseDTO>>.Ok(
                _mapper.Map<IEnumerable<AppointmentResponseDTO>>(appointments));
        }

        public async Task<Result<IEnumerable<AppointmentResponseDTO>>> ProposeAppointmentsAsync(
      string studentUserId, ProposeAppointmentsDTO dto)
        {
            var studentRepo = _unitOfWork.GetRepository<Student, int>();
            var caseRepo = _unitOfWork.GetRepository<Case, int>();
            var appointmentRepo = _unitOfWork.GetRepository<Appointment, int>();

            // 1. Get Student
            var student = await studentRepo
                .GetByIdAsync(new StudentByUserIdSpecification(studentUserId));

            if (student is null)
                return Error.NotFound("Student.NotFound");

            //if (!student.IsApproved)
            //    return Error.Validation("Student.NotApproved");

            // 2. Get Case
            var caseEntity = await caseRepo.GetByIdAsync(dto.CaseId);

            if (caseEntity is null)
                return Error.NotFound("Case.NotFound");

            if (caseEntity.AssignedStudentId != student.Id)
                return Error.Validation("Case.NotAssigned");

            if (caseEntity.Status != CaseStatus.Assigned)
                return Error.Validation("Case.InvalidStatus", "Case must be assigned");

            // 3. Prevent duplicate active appointments
            var hasActiveAppointments = await appointmentRepo
                .GetAllAsync(new ActiveAppointmentsByCaseSpecification(dto.CaseId));

            if (hasActiveAppointments.Any())
                return Error.Validation("Appointments.Exists",
                    "Active appointments already exist for this case");

            // 4. Validate slots count
            if (dto.Slots is null || dto.Slots.Count != 2)
                return Error.Validation("Slots.Invalid", "Exactly 2 slots required");

            var slot1 = dto.Slots[0];
            var slot2 = dto.Slots[1];

            // 5. Validate distinct dates
            if (slot1.AppointmentDate == slot2.AppointmentDate)
                return Error.Validation("Slots.DuplicateDates");

            // 6. Validate future dates
            if (slot1.AppointmentDate <= DateTimeOffset.UtcNow ||
                slot2.AppointmentDate <= DateTimeOffset.UtcNow)
                return Error.Validation("Slots.InvalidDate", "Dates must be in the future");

            // 7. Conflict check (student already has confirmed appointment)
            var conflict1 = await appointmentRepo.GetAllAsync(
                new StudentConfirmedAppointmentsAtTimeSpecification(student.Id, slot1.AppointmentDate));

            var conflict2 = await appointmentRepo.GetAllAsync(
                new StudentConfirmedAppointmentsAtTimeSpecification(student.Id, slot2.AppointmentDate));

            if (conflict1.Any() || conflict2.Any())
                return Error.Validation("Slots.Conflict",
                    "You already have an appointment at one of these times");

            // 8. Create appointments (NO await in loop)
            var appointment1 = new Appointment
            {
                CaseId = dto.CaseId,
                StudentId = student.Id,
                PatientId = caseEntity.PatientId,
                AppointmentDate = slot1.AppointmentDate,
                Location = slot1.Location,
                Status = AppointmentStatus.proposed
            };

            var appointment2 = new Appointment
            {
                CaseId = dto.CaseId,
                StudentId = student.Id,
                PatientId = caseEntity.PatientId,
                AppointmentDate = slot2.AppointmentDate,
                Location = slot2.Location,
                Status = AppointmentStatus.proposed
            };

            await appointmentRepo.AddAsync(appointment1);
            await appointmentRepo.AddAsync(appointment2);

            await _unitOfWork.SaveChangesAsync();

            // 9. Schedule Hangfire ONLY after save (Ids are generated now)

            //  Schedule auto-complete after appointment time
            _backgroundJobService.ScheduleAppointmentCompletion(
                appointment1.Id,
                appointment1.AppointmentDate.UtcDateTime.AddHours(1));

            _backgroundJobService.ScheduleAppointmentCompletion(
                appointment2.Id,
                appointment2.AppointmentDate.UtcDateTime.AddHours(1));

            // 10. Return only created
            var result = new List<Appointment> { appointment1, appointment2 };

            return Result<IEnumerable<AppointmentResponseDTO>>.Ok(
                _mapper.Map<IEnumerable<AppointmentResponseDTO>>(result));
        }


        public async Task<Result> RejectAllAppointmentsAsync(int caseId, string patientUserId)
        {
         var patientRepo= _unitOfWork.GetRepository<Patient,int>();
         var appointmentRepo =  _unitOfWork.GetRepository<Appointment, int>();
           
          var Patient= await patientRepo.GetByIdAsync(new PatientByUserIdSpecification(patientUserId));
            if (Patient is null)
                return Result.Failure(Error.NotFound("Patient.NotFound"));

            var appointments = await appointmentRepo
                .GetAllAsync(new ProposedAppointmentsByCaseSpecification(caseId));
            foreach (var appointment in appointments)
            {
                appointment.Status = AppointmentStatus.Rejected;
                appointmentRepo.Update(appointment);
            }
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();

        }

        public async Task<Result> SelectAppointmentAsync(int appointmentId, string patientUserId)
        {
            var patientRepo = _unitOfWork.GetRepository<Patient, int>();
            var appointmentRepo = _unitOfWork.GetRepository<Appointment, int>();

            var patient = await patientRepo
                .GetByIdAsync(new PatientByUserIdSpecification(patientUserId));

            if (patient is null)
                return Result.Failure(Error.NotFound("Patient.NotFound"));

            var appointment = await appointmentRepo
                .GetByIdAsync(new AppointmentByIdForPatientSpecification(
                    appointmentId, patient.Id));

            if (appointment is null)
                return Result.Failure(Error.NotFound("Appointment.NotFound"));

            if (appointment.Status != AppointmentStatus.proposed)
                return Result.Failure(Error.Validation("Appointment.InvalidStatus",
                    "Only proposed appointments can be selected"));

            var proposedAppointments = await appointmentRepo
                .GetAllAsync(new ProposedAppointmentsByCaseSpecification(appointment.CaseId));

            appointment.Status = AppointmentStatus.Confirmed;
            appointmentRepo.Update(appointment);

            foreach (var other in proposedAppointments)
            {
                if (other.Id != appointment.Id)
                {
                    other.Status = AppointmentStatus.Cancelled;
                    appointmentRepo.Update(other);
                }
            }

            await _unitOfWork.SaveChangesAsync();

              return Result.Ok();
        }

 
    }
}
