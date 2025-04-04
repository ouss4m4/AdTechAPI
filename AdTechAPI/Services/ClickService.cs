using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Services
{
    public class ClickService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ClickService> _logger;

        public ClickService(AppDbContext db, ILogger<ClickService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task UpdateClickRevenue(Guid uuid, decimal revenue)
        {
            var click = await _db.Clicks.FirstOrDefaultAsync(c => c.Uuid == uuid);
            if (click == null)
            {
                _logger.LogWarning("Click not found for UUID: {uuid}", uuid);
                return;
            }

            click.Revenue = revenue;
            click.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            _logger.LogInformation("Click revenue updated: UUID={uuid}, Revenue={revenue}", uuid, revenue);
        }
    }
}