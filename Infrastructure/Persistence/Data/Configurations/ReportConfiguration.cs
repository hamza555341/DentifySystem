using Domain.Entites.ReportModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Reports");

            builder.Property(r => r.Diagnosis)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(r => r.TreatmentPlan)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(r => r.Notes)
                   .HasMaxLength(2000);

            builder.HasOne(r => r.Case)
                   .WithMany(c=>c.Reports)
                   .HasForeignKey(r => r.CaseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Student)
                   .WithMany(s => s.Reports)
                   .HasForeignKey(r => r.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
