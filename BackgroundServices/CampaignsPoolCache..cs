
using Cronos;
using Microsoft.Extensions.Logging;
namespace AdTechAPI.BackgroundServices
{
    class CampaignsPool : BackgroundService
    {
        private readonly CronExpression _cronExpression = CronExpression.Parse("*/3 * * * *");
        // Removed duplicate declaration of _logger
        private DateTime _nextRunTime = DateTime.UtcNow;

        private readonly ILogger<CampaignsPool> _logger;

        public CampaignsPool(ILogger<CampaignsPool> logger)
        {
            _logger = logger;
            // Define the cron schedule (every 3rd minute)
            _cronExpression = CronExpression.Parse("*/3 * * * *");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                if (_nextRunTime <= now)
                {
                    UpdateCampaignCache();
                    _nextRunTime = _cronExpression.GetNextOccurrence(now, TimeZoneInfo.Utc) ?? now.AddMinutes(3);
                }

                // Sleep for a short time to avoid excessive CPU usage
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        // private async Task UpdateCampaignCache()
        private void UpdateCampaignCache()
        {
            _logger.LogInformation("Campaign cache updated at {Time}", DateTime.UtcNow);
        }
    }
}