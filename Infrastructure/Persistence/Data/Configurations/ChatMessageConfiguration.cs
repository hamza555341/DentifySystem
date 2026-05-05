using Domain.Entites.ChatModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasOne(cm => cm.TreatmentRequest)
                   .WithMany(tr => tr.Messages)
                   .HasForeignKey(cm => cm.TreatmentRequestId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cm => cm.Sender)
                   .WithMany()
                   .HasForeignKey(cm => cm.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(cm => cm.Content)
                   .IsRequired();

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
