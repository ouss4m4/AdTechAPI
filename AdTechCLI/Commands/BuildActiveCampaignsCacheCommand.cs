using AdTechAPI.CampaignsCache;
using Microsoft.Extensions.DependencyInjection;

namespace AdTechCLI.Commands;

public class BuildActiveCampaignsCacheCommand : ICommandHandler
{
    private readonly BuildActiveCampaignsCache _service;

    public BuildActiveCampaignsCacheCommand(BuildActiveCampaignsCache service)
    {
        _service = service;
    }

    public async Task RunAsync()
    {
        Console.WriteLine("Running cache update...");
        await _service.Run();
        Console.WriteLine("Campaign cache updated.");
    }
}