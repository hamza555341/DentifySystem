using Domain.Entites.CaseModule;
using Domain.Interfaces;
using Hangfire;
using Service.Abstraction;

namespace DentifySystem.BackgroundJobs
{
    public class HangfireJobService : IBackgroundJobService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HangfireJobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task ExpireOldCasesAsync()
        {
            var expiredCases = await _unitOfWork.GetRepository<Case, int>()
                .GetAllAsync(new ExpiredCasesSpecification());

            foreach (var case_ in expiredCases)
            {
                case_.Status = CaseStatus.Expired;
                _unitOfWork.GetRepository<Case, int>().Update(case_);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public void ScheduleAppointmentCompletion(int appointmentId, DateTime runAt)
        {
            BackgroundJob.Schedule<IAppointmentService>(
                service => service.AutoCompleteAppointmentAsync(appointmentId),
                runAt);
        }
    }
}
