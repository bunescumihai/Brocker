using Brocker.Models;
using Brocker.Models.Configurations;
using Microsoft.EntityFrameworkCore;

[EntityTypeConfiguration(typeof(UserConfiguration))]
public class User
{
    public int? Id { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public UserRole? UserRole { get; set; }

    public List<Article> Articles { get; } = new();
    public List<Sending> Sendings { get; set; } = new();  
    public List<Subscription> Subscriptions { get; set; } = new();

    public User(string userName, string password, UserRole userRole)
    {
        UserName = userName;
        Password = password;
        UserRole = userRole;
    }

    public User(){}
}