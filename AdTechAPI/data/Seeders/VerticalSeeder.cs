using AdTechAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Data.Seeders
{
    public static class VerticalSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Verticals.Any())
            {
                var verticals = new List<Vertical>
                {
                    new Vertical
                    {
                        Name = "Health & Wellness",
                        Description = "Health, fitness, and wellness products",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Vertical
                    {
                        Name = "Finance",
                        Description = "Financial services and products",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Vertical
                    {
                        Name = "E-commerce",
                        Description = "Online retail and shopping",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                await context.Verticals.AddRangeAsync(verticals);
                await context.SaveChangesAsync();
            }
        }
    }
}
