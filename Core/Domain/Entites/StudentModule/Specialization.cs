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
        None=0,
        DentalCaries = 1,
        PeriodontalDiseas = 2,
        Hypodontia = 4,
        MouthUlcer = 8,
        ToothDiscoloration = 16
    }
}
