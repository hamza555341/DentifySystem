using Domain.Entites.StudentModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    internal class StudentRatingConfiguration : IEntityTypeConfiguration<StudentRating>
    {
        public void Configure(EntityTypeBuilder<StudentRating> builder)
        {
            builder.ToTable("StudentRatings");

            builder.Property(r => r.Rating)
                   .IsRequired();

            builder.Property(r => r.Comment)
                   .HasMaxLength(1000);

            builder.HasOne(r => r.Student)
                   .WithMany(s => s.Ratings)
                   .HasForeignKey(r => r.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Patient)
                   .WithMany(p => p.Ratings)
                   .HasForeignKey(r => r.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => new { r.CaseId, r.PatientId })
                   .IsUnique();
        }



    }
    
}
