using Microsoft.EntityFrameworkCore;
using AdTechAPI.Models;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>()
            .Property(c => c.Type)
            .HasConversion<int>();

        modelBuilder.Entity<User>()
        .Property(c => c.Role)
        .HasConversion<int>();
    }
}