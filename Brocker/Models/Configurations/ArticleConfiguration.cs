using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Brocker.Models.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder
            .HasOne<User>(article => article.Sender)
            .WithMany(sender => sender.Articles)
            .IsRequired();

        builder
            .HasOne<Topic>(article => article.Topic)
            .WithMany(topic => topic.Articles)
            .IsRequired();
    }
}