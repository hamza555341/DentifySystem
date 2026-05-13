using Domain.Entites.PatientModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    internal class patientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");

          
            builder.Property(p => p.City)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Governorate)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.NationalId)
                   .HasMaxLength(20);

            builder.Property(p => p.ProfileImageUrl)
                   .HasMaxLength(500);

            builder.HasOne(p => p.ApplicationUser)
            .WithOne()
            .HasForeignKey<Patient>(p => p.IdentityUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.NationalId)
                .IsUnique()
                .HasFilter("[NationalId] IS NOT NULL");

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
