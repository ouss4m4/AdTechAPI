using AdTechAPI.Models;

namespace AdTechAPI.Data.Seeders
{
    public static class CountrySeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Countries.Any())
            {
                var countries = new List<Country>
                {
                    new Country
                    {
                        Name = "United States",
                        Code = "US",
                        Region = "North America",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Country
                    {
                        Name = "United Kingdom",
                        Code = "GB",
                        Region = "Europe",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Country
                    {
                        Name = "Canada",
                        Code = "CA",
                        Region = "North America",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                await context.Countries.AddRangeAsync(countries);
                await context.SaveChangesAsync();
            }
        }
    }
}
