using Domain.Entites.ReportModule;
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
    public class TreatmentRequestConfiguration : IEntityTypeConfiguration<TreatmentRequest>
    {
        public void Configure(EntityTypeBuilder<TreatmentRequest> builder)
        {
            builder.HasOne(tr => tr.Case)
                   .WithMany(c => c.TreatmentRequests)
                   .HasForeignKey(tr => tr.CaseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tr => tr.Student)
                   .WithMany(s => s.TreatmentRequests)
                   .HasForeignKey(tr => tr.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Appointments)
                .WithOne(x => x.TreatmentRequest)
                .HasForeignKey(x =>x.TreatmentRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Messages)
                .WithOne(x => x.TreatmentRequest)
                .HasForeignKey(x => x.TreatmentRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Report)
                .WithOne(x => x.TreatmentRequest)
                .HasForeignKey<Report>(x => x.TreatmentRequestId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.Property(tr => tr.Status)
                   .HasConversion<string>();
        }
    }
}
