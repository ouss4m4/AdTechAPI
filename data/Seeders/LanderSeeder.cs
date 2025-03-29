using AdTechAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Data.Seeders
{
    public static class LanderSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Landers.Any())
            {
                // Make sure clients exist first
                var healthPlusClient = await context.Clients.FirstOrDefaultAsync(c => c.Name == "Health Plus Inc");
                var financeDirectClient = await context.Clients.FirstOrDefaultAsync(c => c.Name == "Finance Direct");

                if (healthPlusClient != null && financeDirectClient != null)
                {
                    var landers = new List<Lander>
                    {
                        new Lander 
                        { 
                            Name = "Health Plus Landing Page",
                            Url = "https://healthplus.example.com/offer1",
                            Notes = "Main health products landing page",
                            AdvertiserId = healthPlusClient.Id
                        },
                        new Lander 
                        { 
                            Name = "Finance Direct Calculator",
                            Url = "https://financedirect.example.com/calculator",
                            Notes = "Financial calculator landing page",
                            AdvertiserId = financeDirectClient.Id
                        }
                    };

                    await context.Landers.AddRangeAsync(landers);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
