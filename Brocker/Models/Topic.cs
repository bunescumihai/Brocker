using Brocker.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Brocker.Models;

[EntityTypeConfiguration(typeof(TopicConfiguration))]
public class Topic
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Article> Articles { get; } = new();
    public List<Subscription> Subscriptions { get; set; } = new();  // Navigation property for users subscribing
}

