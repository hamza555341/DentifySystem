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

            builder.Property(a => a.Location)
                   .IsRequired()
                   .HasColumnType("nvarchar")
                   .HasMaxLength(200);

            builder.Property(a => a.Status)
                   .HasConversion<int>();

            builder.HasOne(a => a.Case)
                   .WithMany(c=>c.Appointments)
                   .HasForeignKey(a => a.CaseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Student)
                   .WithMany()
                   .HasForeignKey(a => a.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Patient)
                   .WithMany()
                   .HasForeignKey(a => a.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
