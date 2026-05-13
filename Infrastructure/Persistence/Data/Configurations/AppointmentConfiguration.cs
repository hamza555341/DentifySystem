using Domain.Entites.AppointmentModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            // =============================
            // Properties
            // =============================
            builder.Property(a => a.Location)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(a => a.Status)
                   .HasConversion<int>();

            builder.Property(a => a.AppointmentDate)
                   .IsRequired();

            // =============================
            // Relationship (IMPORTANT)
            // =============================
            builder.HasOne(a => a.TreatmentRequest)
                   .WithMany(r => r.Appointments)
                   .HasForeignKey(a => a.TreatmentRequestId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

