using AdTechAPI.Models;
using System.Globalization;

namespace AdTechAPI.Data.Seeders
{
    public static class CountrySeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Countries.Any())
                return;

            var countries = new List<Country>();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "data/Seeders", "countries.csv");
            if (!File.Exists(filePath))
                throw new FileNotFoundException("countries.csv file not found", filePath);

            var lines = await File.ReadAllLinesAsync(filePath);

            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');

                if (parts.Length < 4)
                    continue;

                var country = new Country
                {
                    Id = int.Parse(parts[0]),
                    Iso = parts[1].Trim(),
                    Name = parts[2].Trim(),
                    NiceName = parts[3].Trim(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                countries.Add(country);
            }

            await context.Countries.AddRangeAsync(countries);
            await context.SaveChangesAsync();
        }
    }
}
