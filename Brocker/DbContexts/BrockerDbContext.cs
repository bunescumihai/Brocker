using Brocker.Models;
using Microsoft.EntityFrameworkCore;

namespace Brocker.DbContexts;

public class BrockerDbContext: DbContext
{

    private static BrockerDbContext _brockerDbContext = new BrockerDbContext();

    public static BrockerDbContext GetBrockerDbContext() => _brockerDbContext;

    public DbSet<User> Users { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Sending> Sendings { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use your database provider here (SQL Server example)
        optionsBuilder.UseSqlServer("Server=DESKTOP-5RRADE6;Database=brocker;User Id=brocker;Password=brocker;Trusted_Connection=False;TrustServerCertificate=True;");
    }
    
}