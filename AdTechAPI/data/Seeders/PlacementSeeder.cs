using AdTechAPI.Models;
using AdTechAPI.Enums;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Data.Seeders
{
    public static class PlacementSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Placements.Any())
            {
                // Get the publisher client
                var publisher = await context.Clients
                    .FirstOrDefaultAsync(c => c.Type == ClientType.Publisher);

                if (publisher == null)
                {
                    throw new Exception("Publisher client not found. Please run ClientSeeder first.");
                }

                // Get all traffic sources for this publisher
                var trafficSources = await context.TrafficSources
                    .Where(ts => ts.PublisherId == publisher.Id)
                    .ToListAsync();

                if (!trafficSources.Any())
                {
                    throw new Exception("Traffic sources not found. Please run TrafficSourceSeeder first.");
                }

                var placements = new List<Placement>();

                // Create placements for each traffic source
                foreach (var trafficSource in trafficSources)
                {
                    placements.Add(new Placement
                    {
                        Uuid = Guid.NewGuid(),
                        Name = $"{trafficSource.Name} - Main Placement",
                        PublisherId = publisher.Id,
                        TrafficSourceId = trafficSource.Id,
                        Verticals = new List<int> { 1, 2, 3 }, // All verticals
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });

                    placements.Add(new Placement
                    {
                        Uuid = Guid.NewGuid(),
                        Name = $"{trafficSource.Name} - Secondary Placement",
                        PublisherId = publisher.Id,
                        TrafficSourceId = trafficSource.Id,
                        Verticals = new List<int> { 1, 2 }, // Health and Finance
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                await context.Placements.AddRangeAsync(placements);
                await context.SaveChangesAsync();
            }
        }
    }
}