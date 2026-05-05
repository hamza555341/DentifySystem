using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.StudentRatingDtos
{
    public class CreateRatingDTO
    {
        public int TreatmentRequestId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
