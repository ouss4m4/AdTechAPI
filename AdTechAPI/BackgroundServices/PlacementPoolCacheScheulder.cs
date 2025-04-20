
using AdTechAPI.PlacementCache;
using Cronos;

namespace AdTechAPI.BackgroundServices
{
    class PlacementPoolCacheScheduler(IServiceScopeFactory scopeFactory, ILogger<PlacementPoolCacheScheduler> logger) : BackgroundService
    {


        private readonly CronExpression _cronExpression = CronExpression.Parse("*/3 * * * *");
        private DateTime _nextRunTime = DateTime.UtcNow;
        private readonly ILogger<PlacementPoolCacheScheduler> _logger = logger;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Placements cache started -----------");

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                if (_nextRunTime <= now)
                {
                    await UpdatePlacementCache();
                    _nextRunTime = _cronExpression.GetNextOccurrence(now, TimeZoneInfo.Utc) ?? now.AddMinutes(1);
                }

                // Sleep for a short time to avoid excessive CPU usage
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }


        public async Task UpdatePlacementCache()
        {

            using var scope = _scopeFactory.CreateScope();
            var placementCacheBuilder = scope.ServiceProvider.GetRequiredService<BuildPlacementCache>();

            await placementCacheBuilder.Run();

        }
    }
}
