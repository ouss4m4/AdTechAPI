using AdTechAPI.Rollups;
using Cronos;

namespace AdTechAPI.BackgroundServices
{
    public class GenerateRollupsSchedule : BackgroundService
    {


        // private readonly CronExpression _cronExpression = CronExpression.Parse("*/3 * * * *"); // every 3 minutes
        private readonly CronExpression _cronExpression = CronExpression.Parse("*/3 * * * *"); // every 3 minutes
        private DateTime _nextRunTime = DateTime.UtcNow;

        private readonly ILogger<GenerateRollupsSchedule> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public GenerateRollupsSchedule(IServiceScopeFactory scopeFactory, ILogger<GenerateRollupsSchedule> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _cronExpression = CronExpression.Parse("*/3 * * * *");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                if (_nextRunTime <= now)
                {
                    await ExecuteRollupHour();
                    _nextRunTime = _cronExpression.GetNextOccurrence(now, TimeZoneInfo.Utc) ?? now.AddMinutes(1);
                }

                // Sleep for a short time to avoid excessive CPU usage
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        public async Task ExecuteRollupHour()
        {

            using var scope = _scopeFactory.CreateScope();
            var generateRollup = scope.ServiceProvider.GetRequiredService<GenerateRollupHour>();

            await generateRollup.Run();


        }
    }
}
