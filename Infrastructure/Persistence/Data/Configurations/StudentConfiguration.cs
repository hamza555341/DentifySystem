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
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");

            builder.Property(s => s.FullName)
                   .IsRequired()
                   .HasColumnType("nvarchar")
                   .HasMaxLength(200);

            builder.Property(s => s.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.ToTable(S =>
            {
                S.HasCheckConstraint("CK_Student_Phone", "(PhoneNumber LIKE '010________' OR PhoneNumber LIKE '011________' OR PhoneNumber LIKE '012________')");
            });
            builder.HasIndex(e => e.PhoneNumber)
                   .IsUnique();

            builder.Property(S => S.Email)
                .IsRequired();

            builder.ToTable(S =>
            {
                S.HasCheckConstraint("CK_Student_Email", "Email LIKE '%_@__%.__%'");
            });

            builder.HasIndex(e => e.Email)
                   .IsUnique();

            builder.Property(s => s.University)
                   .IsRequired()
                   .HasColumnType("nvarchar")
                   .HasMaxLength(200);

            builder.Property(s => s.AcademicYear)
                   .IsRequired();

            builder.Property(s => s.ProfileImageUrl)
                   .HasMaxLength(500);

            builder.Property(s => s.CreatedAt)
                   .HasColumnName("JoinDate")
                   .HasDefaultValueSql("GETDATE()");

        }
    }
}
