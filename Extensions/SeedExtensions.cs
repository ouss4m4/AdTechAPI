using AdTechAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AdTechAPI.Extensions
{
    public static class SeedExtensions
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Ensure database is created and migrations are applied
            await context.Database.MigrateAsync();

            // Seed the data
            await DatabaseSeeder.SeedAsync(context);
        }
    }
}
