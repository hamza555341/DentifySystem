using Domain.Entites.CaseModule;
using Domain.Entites.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.PatientModule
{
    public class Patient :BaseEntity<int>
    {
        public string IdentityUserId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Governorate { get; set; } = null!;

        public string? NationalId { get; set; }

        public string? ProfileImageUrl { get; set; }

        public string? ChronicDiseases { get; set; }

        public ICollection<Case> Cases { get; set; } = new List<Case>();

        public ICollection<StudentRating> Ratings { get; set; } = new List<StudentRating>();



    }
}
