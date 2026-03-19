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
    internal class CaseImageConfiguration : IEntityTypeConfiguration<CaseImage>
    {
        public void Configure(EntityTypeBuilder<CaseImage> builder)
        {
            builder.ToTable("CaseImages");

            builder.Property(i => i.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(i => i.Case)
                   .WithMany(c => c.Images)
                   .HasForeignKey(i => i.CaseId)
                   .OnDelete(DeleteBehavior.Cascade);

        }

    }
 }