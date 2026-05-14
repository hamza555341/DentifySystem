using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.StudentDTOs
{
    public class StudentResponseDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string UniEmail { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public List<string> Specializations { get; set; } = new();
    }
}
