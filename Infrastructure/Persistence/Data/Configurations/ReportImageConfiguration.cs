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
    public class ReportImageConfiguration : IEntityTypeConfiguration<ReportImage>
    {
        public void Configure(EntityTypeBuilder<ReportImage> builder)
        {
            builder.ToTable("ReportImages");

            builder.Property(i => i.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            //builder.Property(i => i.ImageUrl)
            //       .HasConversion<int>(); 

            builder.HasOne(i => i.Report)
                   .WithMany(r => r.Images)
                   .HasForeignKey(i => i.ReportId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
