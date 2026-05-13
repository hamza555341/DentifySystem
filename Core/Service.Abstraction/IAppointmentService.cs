using Shared.CommonResult;
using Shared.DTOs.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface IAppointmentService
    {
        // =============================
        // Student
        // =============================
        Task<Result<IEnumerable<AppointmentResponseDTO>>> ProposeAppointmentsAsync(
            string studentUserId,
            ProposeAppointmentsDTO dto);
        // الطالب يقترح موعدين لنفس الـ TreatmentRequest


        // =============================
        // Patient Actions
        // =============================
        Task<Result> SelectAppointmentAsync(
            int appointmentId,
            string patientUserId);
        // المريض يختار موعد → الباقي يتكنسل تلقائي


        Task<Result> RejectAllAppointmentsAsync(
            int treatmentRequestId,
            string patientUserId);
        // المريض يرفض كل المقترحات


        // =============================
        // Get By Context
        // =============================
        
        // يجيب مواعيد Request معين (للمريض أو الطالب)


        Task<Result<IEnumerable<AppointmentResponseDTO>>> GetPatientAppointmentsAsync(
            string patientUserId);
        // كل مواعيد المريض


        Task<Result<IEnumerable<AppointmentResponseDTO>>> GetStudentAppointmentsAsync(
            string studentUserId);
        // كل مواعيد الطالب


        // =============================
        // Background Job
        // =============================
        Task AutoCompleteAppointmentAsync(int appointmentId);
        // Hangfire
    }
}
