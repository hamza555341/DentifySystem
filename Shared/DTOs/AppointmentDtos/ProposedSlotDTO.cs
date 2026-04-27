using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.AppointmentDtos
{
    public class ProposedSlotDTO
    {
            public DateTimeOffset AppointmentDate { get; set; }
            public string Location { get; set; } = null!;     
    }
}
