using AdTechAPI.Data.Seeders;

namespace AdTechAPI.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Seed in order of dependencies
            await VerticalSeeder.SeedAsync(context);
            await CountrySeeder.SeedAsync(context);
            await ClientSeeder.SeedAsync(context);
            await LanderSeeder.SeedAsync(context);
            await CampaignSeeder.SeedAsync(context);
        }
    }
}
