using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.AppointmentModule
{
    public enum AppointmentStatus
    {
        proposed = 0,
        Confirmed = 1,
        Cancelled = 2,
        Rejected = 3,
        Completed=4 
    }
}
