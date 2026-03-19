using Domain.Entites.CaseModule;
using Domain.Entites.ReportModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.StudentModule
{
    public class Student:BaseEntity<int>
    {
        public string IdentityUserId { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        public string FullName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string University { get; set; } = null!;

        public int AcademicYear { get; set; }

        public bool IsApproved { get; set; } = true;

        public string? ProfileImageUrl { get; set; }

        public ICollection<Case> AcceptedCases { get; set; } = new List<Case>();

        public ICollection<Report> Reports { get; set; } = new List<Report>();

        public ICollection<StudentRating> Ratings { get; set; } = new List<StudentRating>();


    }
}
