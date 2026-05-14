using Domain.Entites.StudentModule;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Shared.DTOs.CaseDtos
{
    public class CreateCaseDTO
    {
        public Specialization RequiredSpecialization { get; set; }
        public string Description { get; set; } = null!;
        public string City { get; set; } = null!;
        public List<IFormFile> Images { get; set; } = new();
    }
}
