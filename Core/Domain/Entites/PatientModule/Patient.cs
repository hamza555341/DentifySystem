using Domain.Entites.CaseModule;
using Domain.Entites.IdentityModule;
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


        public string City { get; set; } = null!;

        public string? ProfileImageUrl { get; set; }

        public ICollection<Case> Cases { get; set; } = new List<Case>();
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public ICollection<StudentRating> Ratings { get; set; } = new List<StudentRating>();



    }
}
