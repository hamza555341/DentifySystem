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

            // =============================
            // Relations
            // =============================
            builder.HasOne(s => s.ApplicationUser)
                   .WithOne()
                   .HasForeignKey<Student>(s => s.IdentityUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // =============================
            // Properties
            // =============================
            builder.Property(s => s.City)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(s => s.UniEmail)
                   .IsRequired();

            builder.Property(s => s.ProfileImageUrl)
                   .HasMaxLength(500);

            builder.Property(s => s.CreatedAt)
                   .HasColumnName("JoinDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(s => s.IsActive)
                   .HasDefaultValue(false);

            builder.Property(s => s.IsActive)
                   .HasDefaultValue(true);

            builder.Property(s => s.Specializations)
                .HasConversion<int>();
        }
    }
}
