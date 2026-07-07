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

        public async Task<Result<IEnumerable<AppointmentResponseDTO>>> GetPatientAppointmentsAsync(string patientUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(patientUserId));

            if (patient is null)
                return Error.NotFound("Patient.NotFound");

            var appointments = await _unitOfWork.GetRepository<Appointment, int>()
                .GetAllAsync(new AppointmentsByPatientSpecification(patient.Id));

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

        public async Task<Result<AppointmentResponseDTO>> ProposeAppointmentsAsync(
            string studentUserId, ProposeAppointmentsDTO dto)
        {
            var student = await _unitOfWork.GetRepository<Student, int>()
                .GetByIdAsync(new StudentByUserIdSpecification(studentUserId));

            if (student is null)
                return Error.NotFound("Student.NotFound");

            var request = await _unitOfWork.GetRepository<TreatmentRequest, int>()
                .GetByIdAsync(dto.TreatmentRequestId);

            if (request is null)
                return Error.NotFound("Request.NotFound");

            if (request.StudentId != student.Id)
                return Error.Validation("Request.NotBelongToStudent");

            if (request.Status != TreatmentRequestStatus.Accepted)
                return Error.Validation("Request.NotAccepted");

            var active = await _unitOfWork.GetRepository<Appointment, int>()
                .GetAllAsync(new ActiveAppointmentsByRequestSpecification(request.Id));

            if (active.Any())
                return Error.Validation("Appointment.AlreadyExists");

            if (dto.AppointmentDate <= DateTimeOffset.UtcNow)
                return Error.Validation("Appointment.InvalidDate");

            var appointment = new Appointment
            {
                TreatmentRequestId = request.Id,
                AppointmentDate = dto.AppointmentDate,
                Location = dto.Location,
                Status = AppointmentStatus.proposed
            };

            await _unitOfWork.GetRepository<Appointment, int>().AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            _backgroundJobService.ScheduleAppointmentCompletion(
                appointment.Id, appointment.AppointmentDate.UtcDateTime.AddHours(1));

            var saved = await _unitOfWork.GetRepository<Appointment, int>()
                .GetByIdAsync(new AppointmentWithRequestSpecification(appointment.Id));

            return Result<AppointmentResponseDTO>.Ok(
                _mapper.Map<AppointmentResponseDTO>(saved));
        }

        public async Task<Result> SelectAppointmentAsync(int appointmentId, string patientUserId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetByIdAsync(new PatientByUserIdSpecification(patientUserId));

            if (patient is null)
                return Error.NotFound("Patient.NotFound");

            var appointment = await _unitOfWork.GetRepository<Appointment, int>()
                .GetByIdAsync(new AppointmentWithRequestSpecification(appointmentId));

            if (appointment is null)
                return Error.NotFound("Appointment.NotFound");

            if (appointment.Status != AppointmentStatus.proposed)
                return Error.Validation("Appointment.InvalidStatus");

            if (appointment.TreatmentRequest.Case.PatientId != patient.Id)
                return Error.Unauthorized("Access.Denied");

            appointment.Status = AppointmentStatus.Confirmed;

            _unitOfWork.GetRepository<Appointment, int>().Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }
    }
}