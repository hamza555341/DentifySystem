using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.StudentRatingDtos
{
    public class RatingResponseDTO
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public string StudentName { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
