using Microsoft.EntityFrameworkCore;
using AdTechAPI.Models;
using System.Text.Json;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients
    {
        get; set;
    }
    public DbSet<User> Users
    {
        get; set;
    }
    public DbSet<Campaign> Campaigns
    {
        get; set;
    }
    public DbSet<Vertical> Verticals
    {
        get; set;
    }
    public DbSet<Country> Countries
    {
        get; set;
    }
    public DbSet<Lander> Landers
    {
        get; set;
    }
    public DbSet<TrafficSource> TrafficSources
    {
        get; set;
    }
    public DbSet<Placement> Placements
    {
        get; set;
    }

    public DbSet<Click> Clicks
    {
        get; set;
    }

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
            .HasOne(c => c.Advertiser)
            .WithMany()
            .HasForeignKey(c => c.AdvertiserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Campaign>()
                    .Property(c => c.Status)
                    .HasConversion<int>();

        // Configure many-to-many relationship for Verticals
        modelBuilder.Entity<Campaign>()
            .HasMany(c => c.Verticals)
            .WithMany(v => v.Campaigns)
            .UsingEntity(j => j.ToTable("CampaignVerticals"));

        // Configure JSON columns (POSTGRES JSONB)
        modelBuilder.Entity<Campaign>()
            .Property(c => c.Platforms)
            .HasColumnType("jsonb");

        modelBuilder.Entity<Campaign>()
            .Property(c => c.Countries)
            .HasColumnType("jsonb");
    }
}