using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.CaseModule
{
    public class CaseImage :BaseEntity<int>
    {
        public int CaseId { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string? ImageType { get; set; } = null!;

        public Case Case { get; set; } = null!;

    }
}
