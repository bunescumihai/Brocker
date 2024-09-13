using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Brocker.Models.Configurations;


public class SendingConfiguration : IEntityTypeConfiguration<Sending>
{
    public void Configure(EntityTypeBuilder<Sending> builder)
    {
        builder
            .HasOne(e => e.Article)
            .WithMany(e => e.Sendings)
            .HasForeignKey(e => e.ArticleId);

        // Configuring many-to-many relationship between User and Article via Sending
        builder
            .HasOne(s => s.Receiver)
            .WithMany(u => u.Sendings)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}