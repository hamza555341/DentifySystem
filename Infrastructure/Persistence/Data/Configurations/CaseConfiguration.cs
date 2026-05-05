using Domain.Entites.CaseModule;
using Domain.Entites.TreatmentRequestModule;
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

            builder.Property(c => c.Disease)
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
            builder.Property(c => c.Status)
                .HasConversion<int>();

            builder.HasOne(c => c.Patient)
                   .WithMany(p => p.Cases)
                   .HasForeignKey(c => c.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.TreatmentRequests)
                   .WithOne(t=>t.Case)
                   .HasForeignKey(t=>t.CaseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x=>x.Images)
                   .WithOne(i=>i.Case)
                   .HasForeignKey(i=>i.CaseId)
                   .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
