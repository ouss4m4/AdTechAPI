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

                var campaigns = new List<Campaign>
                    {
                        new Campaign
                        {
                            Name = "Health Plus Q2 Campaign",
                            AdvertiserId = 3,
                            LanderId = 1,
                            Notes = "Q2 health products promotion",
                            Status = CampaignStatus.Active,
                            Budget = 10000,
                            DailyBudget = 500,
                            Platforms = new List<int> { (int)Platform.Mobile, (int)Platform.Desktop },
                            Countries = new List<int> { 1, 2 }, // US and UK
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Campaign
                        {
                            Name = "Finance Direct Calculator Campaign",
                            AdvertiserId = 3,
                            LanderId = 2,
                            Notes = "Financial calculator promotion",
                            Status = CampaignStatus.Active,
                            Budget = 15000,
                            DailyBudget = 750,
                            Platforms = new List<int> { (int)Platform.Mobile, (int)Platform.Desktop },
                            Countries = new List<int> { 1, 2, 3 }, // US, UK, and Canada
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Campaign
                        {
                            Name = "Finance Direct Calculator Campaign - Mobile ",
                            AdvertiserId = 3,
                            LanderId = 2,
                            Notes = "Financial calculator promotion",
                            Status = CampaignStatus.Active,
                            Budget = 15000,
                            DailyBudget = 750,
                            Platforms = new List<int> { (int)Platform.Mobile },
                            Countries = new List<int> { 1, 2, 3 }, // US, UK, and Canada
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                        ,new Campaign
                        {
                            Name = "Finance Direct Calculator Campaign - Desktop",
                            AdvertiserId = 3,
                            LanderId = 2,
                            Notes = "Financial calculator promotion",
                            Status = CampaignStatus.Active,
                            Budget = 15000,
                            DailyBudget = 750,
                            Platforms = new List<int> { (int)Platform.Desktop },
                            Countries = new List<int> { 1, 2, 3 }, // US, UK, and Canada
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },new Campaign
                        {
                            Name = "Finance Direct Calculator Campaign- tablet",
                            AdvertiserId = 3,
                            LanderId = 2,
                            Notes = "Financial calculator promotion",
                            Status = CampaignStatus.Active,
                            Budget = 15000,
                            DailyBudget = 750,
                            Platforms = new List<int> { (int)Platform.Tablet },
                            Countries = new List<int> { 1, 2, 3 }, // US, UK, and Canada
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
