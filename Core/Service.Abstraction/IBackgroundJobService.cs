using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstraction
{
    public interface IBackgroundJobService
    {
        void ScheduleAppointmentCompletion(int appointmentId, DateTime runAt);

    }
}
