using Brocker.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Brocker.Models;

[EntityTypeConfiguration(typeof(SubscriptionConfiguration))]

public class Subscription
{
    public int Id { get; set; }
    public int TopicId { get; set; }
    public Topic Topic { get; set; }  // Navigation property

    public int UserId { get; set; }
    public User User { get; set; }  // Navigation property
    
    public Subscription(){}

    public Subscription(int userId, int topicId)
    {
        UserId = userId;
        TopicId = topicId;
    }
}