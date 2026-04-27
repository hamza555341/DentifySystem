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
        Task<Result<IEnumerable<AppointmentResponseDTO>>> ProposeAppointmentsAsync(string studentUserId, ProposeAppointmentsDTO dto);
        // الطالب يقترح موعدين لنفس الـ Case

        Task<Result> SelectAppointmentAsync(int appointmentId, string patientUserId);
        // المريض يختار موعد → التاني يتكنسل تلقائي

        Task<Result> RejectAllAppointmentsAsync(int caseId, string patientUserId);
            
        // المريض يرفض الاتنين → Status = Rejected

        Task<Result<IEnumerable<AppointmentResponseDTO>>> GetCaseAppointmentsAsync( int caseId,string userId);
           
        // يجيب الموعدين المقترحين لحالة معينة

        Task<Result<IEnumerable<AppointmentResponseDTO>>> GetPatientAppointmentsAsync(string patientUserId);
            
        // كل مواعيد المريض

        Task<Result<IEnumerable<AppointmentResponseDTO>>> GetStudentAppointmentsAsync(string studentUserId);
        // كل مواعيد الطالب

        Task AutoCompleteAppointmentAsync(int appointmentId);
        // Hangfire يناديها تلقائي







    }
}
