using Brocker.Models;
using Brocker.Models.Configurations;
using Microsoft.EntityFrameworkCore;

[EntityTypeConfiguration(typeof(ArticleConfiguration))]
public class Article
{
    public int Id { get; set; }
    public int TopicId { get; set; }
    public Topic Topic { get; set; }  // Navigation property

    public string Content { get; set; }

    public int SenderId { get; set; }
    public User Sender { get; set; }  // Navigation property

    public List<Sending> Sendings { get; set; } = new();  // Navigation property for Receivers

    public Article(){};
    
    
}