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
                // Make sure required entities exist
                var healthPlusClient = await context.Clients.FirstOrDefaultAsync(c => c.Name == "Health Plus Inc");
                var financeDirectClient = await context.Clients.FirstOrDefaultAsync(c => c.Name == "Finance Direct");
                var healthPlusLander = await context.Landers.FirstOrDefaultAsync(l => l.Name == "Health Plus Landing Page");
                var financeDirectLander = await context.Landers.FirstOrDefaultAsync(l => l.Name == "Finance Direct Calculator");

                if (healthPlusClient != null && financeDirectClient != null && 
                    healthPlusLander != null && financeDirectLander != null)
                {
                    var campaigns = new List<Campaign>
                    {
                        new Campaign 
                        { 
                            Name = "Health Plus Q2 Campaign",
                            AdvertiserId = healthPlusClient.Id,
                            LanderId = healthPlusLander.Id,
                            Notes = "Q2 health products promotion",
                            Status = CampaignStatus.Active,
                            Budget = 10000.00M,
                            DailyBudget = 500.00M,
                            Platforms = new List<int> { (int)Platform.Mobile, (int)Platform.Desktop },
                            Countries = new List<int> { 1, 2 }, // US and UK
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Campaign 
                        { 
                            Name = "Finance Direct Calculator Campaign",
                            AdvertiserId = financeDirectClient.Id,
                            LanderId = financeDirectLander.Id,
                            Notes = "Financial calculator promotion",
                            Status = CampaignStatus.Active,
                            Budget = 15000.00M,
                            DailyBudget = 750.00M,
                            Platforms = new List<int> { (int)Platform.Desktop },
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
}
