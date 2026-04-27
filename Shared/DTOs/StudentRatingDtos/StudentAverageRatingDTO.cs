using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.StudentRatingDtos
{
    public class StudentAverageRatingDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
    }
}
