using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.AppointmentDtos
{
    public class ProposeAppointmentsDTO
    {
        public int CaseId { get; set; }

        [MinLength(2, ErrorMessage = "At least two slots are required.")]
        [MaxLength(2, ErrorMessage = "At most two slots are allowed.")]
        public List<ProposedSlotDTO> Slots { get; set; } = [];
     

    }
}