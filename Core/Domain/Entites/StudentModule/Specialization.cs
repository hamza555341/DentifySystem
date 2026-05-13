using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.StudentModule
{
    [Flags]
    public enum Specialization
    {
        DentalCaries = 1,
        PeriodontalDiseas = 2,
        Hypodontia = 4,
        Healthy = 8,
        MouthUlcer = 16,
        ToothDiscoloration = 32
    }
}
