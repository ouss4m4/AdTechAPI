using Microsoft.EntityFrameworkCore;
using AdTechAPI.Models;
using System.Text.Json;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Vertical> Verticals { get; set; }

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

        modelBuilder.Entity<Campaign>()
            .HasOne(c => c.Advertiser)  // Campaign has one Advertiser (Client)
            .WithMany()                 // Client can have many Campaigns
            .HasForeignKey(c => c.AdvertiserId) // Foreign key is AdvertiserId
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Campaign>()
            .Property(c => c.Platforms)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<Platform>>(v, (JsonSerializerOptions?)null) ?? new List<Platform>()
            )
            .HasColumnType("jsonb");

        // Configure many-to-many relationship for Verticals
        modelBuilder.Entity<Campaign>()
            .HasMany(c => c.Verticals)
            .WithMany(v => v.Campaigns)
            .UsingEntity(j => j.ToTable("CampaignVerticals"));

        // Configure JSON columns
        modelBuilder.Entity<Campaign>()
            .Property(c => c.Platforms)
            .HasColumnType("jsonb");

        modelBuilder.Entity<Campaign>()
            .Property(c => c.Countries)
            .HasColumnType("jsonb");
    }
}