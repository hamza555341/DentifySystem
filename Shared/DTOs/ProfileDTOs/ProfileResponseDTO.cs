using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ProfileDTOs
{
    public class ProfileResponseDTO
    {
        public string FullName { get; set; } = null!;
        public int age { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Role { get; set; } = null!;
        public List<Specialization>? Specializations { get; set; }
    }
}
