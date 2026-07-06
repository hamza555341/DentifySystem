using Domain.Entites.StudentModule;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ProfileDTOs
{
    public class UpdateProfileDTO
    {
        public IFormFile? ProfileImage { get; set; }
        public string? FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public List<Specialization>? Specializations { get; set; }
    }
}
