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

            builder.Property(p => p.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.ToTable(S =>
            {
                S.HasCheckConstraint("CK_Patient_Phone", "(PhoneNumber LIKE '010________' OR PhoneNumber LIKE '011________' OR PhoneNumber LIKE '012________')");
            });
            builder.HasIndex(e => e.PhoneNumber)
                   .IsUnique();

            builder.Property(S => S.Email)
                   .IsRequired();

            builder.ToTable(S =>
            {
                S.HasCheckConstraint("CK_Patient_Email", "Email LIKE '%_@__%.__%'");
            });

            builder.HasIndex(e => e.Email)
                   .IsUnique();

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
        }
    }
}
