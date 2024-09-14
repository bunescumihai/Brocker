using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Brocker.Models.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasIndex(sub => new {sub.TopicId, sub.UserId}).IsUnique();
        
        
        builder
            .HasOne(s => s.Topic)
            .WithMany(t => t.Subscriptions)
            .HasForeignKey(s => s.TopicId);


        builder
            .HasOne(s => s.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}