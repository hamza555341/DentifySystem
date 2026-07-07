using Shared.CommonResult;
using Shared.DTOs.AppointmentDtos;

namespace Service.Abstraction
{
    public interface IAppointmentService
    {
        // Student
        Task<Result<AppointmentResponseDTO>> ProposeAppointmentsAsync(
            string studentUserId,
            ProposeAppointmentsDTO dto);

        // Patient
        Task<Result> SelectAppointmentAsync(
            int appointmentId,
            string patientUserId);

        // Get
        Task<Result<IEnumerable<AppointmentResponseDTO>>> GetPatientAppointmentsAsync(
            string patientUserId);

        Task<Result<IEnumerable<AppointmentResponseDTO>>> GetStudentAppointmentsAsync(
            string studentUserId);

        // Background Job
        Task AutoCompleteAppointmentAsync(int appointmentId);
    }
}