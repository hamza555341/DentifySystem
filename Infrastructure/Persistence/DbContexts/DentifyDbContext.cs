using Domain.Entites.AppointmentModule;
using Domain.Entites.CaseModule;
using Domain.Entites.ChatModule;
using Domain.Entites.IdentityModule;
using Domain.Entites.PatientModule;
using Domain.Entites.ReportModule;
using Domain.Entites.StudentModule;
using Domain.Entites.TreatmentRequestModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.IdentityData.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DbContexts
{
    public class DentifyDbContext : IdentityDbContext<ApplicationUser>
    {
        public DentifyDbContext(DbContextOptions<DentifyDbContext> Options)
            : base(Options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Apply configurations from the current assembly
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }


        // Business Tables
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<CaseImage> CaseImages { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportImage> ReportImages { get; set; }
        public DbSet<StudentRating> StudentRatings { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<TreatmentRequest> TreatmentRequests { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
