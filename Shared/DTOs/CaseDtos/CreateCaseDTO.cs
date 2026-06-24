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
        public IFormFile? Image { get; set; }
        public string? SymptomsText { get; set; }
        public string? PainDuration { get; set; }
        public string? ChronicDiseases { get; set; }
        //public string Description { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
