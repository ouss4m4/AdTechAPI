using AdTechAPI.Models;
using AdTechAPI.Enums;

namespace AdTechAPI.Data.Seeders
{
    public static class ClientSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new List<Client>
                {
                    new Client
                    {
                        Name = "Reset Digital",
                        Type = ClientType.Owner,
                        Status = ClientStatus.Active,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Client
                    {
                        Name = "Yahoo Pages - Pub",
                        Type = ClientType.Publisher,
                        Status = ClientStatus.Active,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Client
                    {
                        Name = "Global Media Ads",
                        Type = ClientType.Advertiser,
                        Status = ClientStatus.Active,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                await context.Clients.AddRangeAsync(clients);
                await context.SaveChangesAsync();
            }
        }
    }
}
