using Brocker.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Brocker.Models;

[EntityTypeConfiguration(typeof(SendingConfiguration))]
public class Sending
{
    public int Id { get; set; }
    public int ArticleId { get; set; }
    public Article Article { get; set; }  // Navigation property

    public int UserId { get; set; }
    public User Receiver { get; set; }  // Navigation property
}
