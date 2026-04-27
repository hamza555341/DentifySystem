using Hangfire;
using Service.Abstraction;

namespace DentifySystem.BackgroundJobs
{
    public class HangfireJobService : IBackgroundJobService
    {
        public void ScheduleAppointmentCompletion(int appointmentId, DateTime runAt)
        {
            BackgroundJob.Schedule<IAppointmentService>(
                service => service.AutoCompleteAppointmentAsync(appointmentId),
                runAt);
        }
    }
}
