using AdTechAPI.CacheBuildersServices;

namespace AdTechAPI.CLI
{
    public class BuildActiveCampaignsCacheCommand
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BuildActiveCampaignsCacheCommand> _logger;

        public BuildActiveCampaignsCacheCommand(IServiceScopeFactory scopeFactory, ILogger<BuildActiveCampaignsCacheCommand> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task RunCommand(string[] args)
        {
            if (args.Length < 2) return;

            string command = args[1].ToLower(); // Get second argument as command

            if (command == "cache:build-active-campaigns-cache")
            {
                using var scope = _scopeFactory.CreateScope();
                var cacheBuilder = scope.ServiceProvider.GetRequiredService<BuildActiveCampaignsCache>();

                _logger.LogInformation("Running cache update from CLI...");
                await cacheBuilder.Run();
                _logger.LogInformation("Campaign cache updated via CLI.");
            }
        }
    }
}