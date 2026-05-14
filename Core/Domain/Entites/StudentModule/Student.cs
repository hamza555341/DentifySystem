using Domain.Entites.CaseModule;
using Domain.Entites.IdentityModule;
using Domain.Entites.ReportModule;
using Domain.Entites.TreatmentRequestModule;
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
        public string City { get; set; } = null!;
        public string UniEmail { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }

        public Specialization Specializations { get; set; }

        public ApplicationUser ApplicationUser { get; set; } = null!;


        public ICollection<TreatmentRequest> TreatmentRequests { get; set; } = new List<TreatmentRequest>();

        public ICollection<Report> Reports { get; set; } = new List<Report>();

        public ICollection<StudentRating> Ratings { get; set; } = new List<StudentRating>();


    }
}
