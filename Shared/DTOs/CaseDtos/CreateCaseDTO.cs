using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace Shared.DTOs.CaseDtos
{
    public class CreateCaseDTO
    {
        public string Disease { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string City { get; set; } = null!;
        public List<IFormFile> Images { get; set; } = new();
    }
}
