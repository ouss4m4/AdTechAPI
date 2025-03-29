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

                var landers = new List<Lander>
                    {
                        new Lander
                        {
                            Name = "Health Plus Landing Page",
                            Url = "https://healthplus.example.com/offer1",
                            Notes = "Main health products landing page",
                            AdvertiserId = 3
                        },
                        new Lander
                        {
                            Name = "Finance Direct Calculator",
                            Url = "https://financedirect.example.com/calculator",
                            Notes = "Financial calculator landing page",
                            AdvertiserId = 3
                        }
                    };

                await context.Landers.AddRangeAsync(landers);
                await context.SaveChangesAsync();

            }
        }
    }
}
