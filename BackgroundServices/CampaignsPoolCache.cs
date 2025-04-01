
using AdTechAPI.CacheBuildersServices;
using Cronos;
namespace AdTechAPI.BackgroundServices
{
    class CampaignsPool : BackgroundService
    {
        private readonly CronExpression _cronExpression = CronExpression.Parse("*/3 * * * *");
        // Removed duplicate declaration of _logger
        private DateTime _nextRunTime = DateTime.UtcNow;

        private readonly ILogger<CampaignsPool> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CampaignsPool(IServiceScopeFactory scopeFactory, ILogger<CampaignsPool> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _cronExpression = CronExpression.Parse("*/3 * * * *");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Campaign cache started -----------");

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                if (_nextRunTime <= now)
                {
                    UpdateCampaignCache();
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

            await campaignCacheBuilder.Run(); // Safe scoped execution

            _logger.LogInformation("Campaign cache updated at {Time}", DateTime.UtcNow);
        }
    }
}