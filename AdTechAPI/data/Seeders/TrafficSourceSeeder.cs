using AdTechAPI.Models;
using AdTechAPI.Enums;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Data.Seeders
{
    public static class TrafficSourceSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.TrafficSources.Any())
            {
                // Get the publisher client
                var publisher = await context.Clients
                    .FirstOrDefaultAsync(c => c.Type == ClientType.Publisher);

                if (publisher == null)
                {
                    throw new Exception("Publisher client not found. Please run ClientSeeder first.");
                }

                var trafficSources = new List<TrafficSource>
                {
                    new() {
                        Uuid = Guid.NewGuid(),
                        Name = "Yahoo Push",
                        TrafficType = TrafficType.Push,
                        PublisherId = publisher.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new() {
                        Uuid = Guid.NewGuid(),
                        Name = "Yahoo Display",
                        TrafficType = TrafficType.Display,
                        PublisherId = publisher.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new() {
                        Uuid = Guid.NewGuid(),
                        Name = "Yahoo Native",
                        TrafficType = TrafficType.Native,
                        PublisherId = publisher.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                await context.TrafficSources.AddRangeAsync(trafficSources);
                await context.SaveChangesAsync();
            }
        }
    }
}