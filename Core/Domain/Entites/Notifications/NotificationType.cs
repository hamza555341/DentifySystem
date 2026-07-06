using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.Notifications
{
    public enum NotificationType
    {
        Message = 1,
        Request = 2,
        RequestAccepted = 3,
        RequestRejected = 4,
        AppointmentProposed = 5,

        AppointmentConfirmed = 6
    }
}
