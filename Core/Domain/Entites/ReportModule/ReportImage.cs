using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.ReportModule
{
    public class ReportImage :BaseEntity<int>
    {
        public int ReportId { get; set; }

        public string ImageUrl { get; set; } = null!;

        public DateTime UploadedAt { get; set; }

        public Report Report { get; set; } = null!;
    }
}
