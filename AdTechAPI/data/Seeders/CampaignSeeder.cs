using AdTechAPI.Models;
using AdTechAPI.Enums;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Data.Seeders
{
    public static class CampaignSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Campaigns.Any())
            {
                // Get the advertiser client
                var advertiser = await context.Clients
                    .FirstOrDefaultAsync(c => c.Type == ClientType.Advertiser);

                if (advertiser == null)
                {
                    throw new Exception("Advertiser client not found. Please run ClientSeeder first.");
                }

                // Get all landers for this advertiser
                var landers = await context.Landers
                    .Where(l => l.AdvertiserId == advertiser.Id)
                    .ToListAsync();

                if (!landers.Any())
                {
                    throw new Exception("Landers not found. Please run LanderSeeder first.");
                }

                // Get all verticals
                var verticals = await context.Verticals.ToListAsync();
                var random = new Random();

                var campaigns = new List<Campaign>
                {
                    new() {
                        Name = "Health Plus Q2 Campaign",
                        AdvertiserId = advertiser.Id,
                        LanderId = landers[0].Id, // First lander
                        Notes = "Q2 health products promotion",
                        Status = CampaignStatus.Active,
                        Budget = 10000,
                        DailyBudget = 500,
                        Platforms = new List<int> { (int)Platform.Mobile, (int)Platform.Desktop },
                        Countries = new List<int> { 226, 227 },
                        Verticals = verticals.Where(v => v.Name.Contains("Health")).ToList(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new() {
                        Name = "Finance Direct Calculator Campaign",
                        AdvertiserId = advertiser.Id,
                        LanderId = landers[1].Id, // Second lander
                        Notes = "Financial calculator promotion",
                        Status = CampaignStatus.Active,
                        Budget = 15000,
                        DailyBudget = 750,
                        Platforms = new List<int> { (int)Platform.Mobile, (int)Platform.Desktop },
                        Countries = new List<int> { 226, 227, 39 }, // US, UK, and Canada
                        Verticals = verticals.Where(v => v.Name.Contains("Finance")).ToList(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new() {
                        Name = "Finance Direct Calculator Campaign - Mobile",
                        AdvertiserId = advertiser.Id,
                        LanderId = landers[1].Id, // Second lander
                        Notes = "Financial calculator promotion - Mobile only",
                        Status = CampaignStatus.Active,
                        Budget = 15000,
                        DailyBudget = 750,
                        Platforms = new List<int> { (int)Platform.Mobile },
                        Countries = new List<int> { 226, 227, 39 }, // US, UK, and Canada
                        Verticals = verticals.Where(v => v.Name.Contains("Finance")).ToList(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new() {
                        Name = "Finance Direct Calculator Campaign - Desktop",
                        AdvertiserId = advertiser.Id,
                        LanderId = landers[1].Id, // Second lander
                        Notes = "Financial calculator promotion - Desktop only",
                        Status = CampaignStatus.Active,
                        Budget = 15000,
                        DailyBudget = 750,
                        Platforms = new List<int> { (int)Platform.Desktop },
                        Countries = new List<int> { 226, 227, 39 }, // US, UK, and Canada
                        Verticals = verticals.Where(v => v.Name.Contains("Finance")).ToList(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new() {
                        Name = "Finance Direct Calculator Campaign - Tablet",
                        AdvertiserId = advertiser.Id,
                        LanderId = landers[1].Id, // Second lander
                        Notes = "Financial calculator promotion - Tablet only",
                        Status = CampaignStatus.Active,
                        Budget = 15000,
                        DailyBudget = 750,
                        Platforms = new List<int> { (int)Platform.Tablet },
                        Countries = new List<int> { 226, 227, 39 }, // US, UK, and Canada
                        Verticals = verticals.Where(v => v.Name.Contains("Finance")).ToList(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                await context.Campaigns.AddRangeAsync(campaigns);
                await context.SaveChangesAsync();
            }
        }
    }
}
