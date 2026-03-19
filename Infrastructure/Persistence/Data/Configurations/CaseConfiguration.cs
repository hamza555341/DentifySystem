using Domain.Entites.CaseModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    internal class CaseConfiguration : IEntityTypeConfiguration<Case>
    {
        public void Configure(EntityTypeBuilder<Case> builder)
        {
            builder.ToTable("Cases");

            builder.Property(c => c.Description)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Description)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(c => c.City)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.AiAnalysisResult)
                   .HasMaxLength(4000);

            builder.HasOne(c => c.Patient)
                   .WithMany(p => p.Cases)
                   .HasForeignKey(c => c.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.AssignedStudent)
                   .WithMany(s => s.AcceptedCases)
                   .HasForeignKey(c => c.AssignedStudentId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
