using Domain.Entites.StudentModule;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CaseDtos
{
    public class EditCaseDTO
    {
        public string Description { get; set; } = null!;
        public string City { get; set; } = null!;
        public IFormFile Image { get; set; } = null!;
        public Specialization RequiredSpecialization { get; set; }
    }
}
