
using AdTechAPI.CampaignsCache;
using Cronos;
namespace AdTechAPI.BackgroundServices
{
    class CampaignsPoolCacheScheduler(IServiceScopeFactory scopeFactory, ILogger<CampaignsPoolCacheScheduler> logger) : BackgroundService
    {
        private readonly CronExpression _cronExpression = CronExpression.Parse("*/3 * * * *");
        private DateTime _nextRunTime = DateTime.UtcNow;

        private readonly ILogger<CampaignsPoolCacheScheduler> _logger = logger;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Campaign cache started -----------");

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                if (_nextRunTime <= now)
                {
                    await UpdateCampaignCache();
                    _nextRunTime = _cronExpression.GetNextOccurrence(now, TimeZoneInfo.Utc) ?? now.AddMinutes(1);
                }

                // Sleep for a short time to avoid excessive CPU usage
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        // private async Task UpdateCampaignCache()
        public async Task UpdateCampaignCache()
        {

            using var scope = _scopeFactory.CreateScope();
            var campaignCacheBuilder = scope.ServiceProvider.GetRequiredService<BuildActiveCampaignsCache>();

            await campaignCacheBuilder.Run();

        }
    }
}
