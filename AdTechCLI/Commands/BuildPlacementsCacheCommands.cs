using AdTechAPI.PlacementCache;

namespace AdTechCLI.Commands;

public class BuildPlacementsCacheCommands(BuildPlacementCache service) : ICommandHandler
{
    private readonly BuildPlacementCache _service = service;

    public async Task RunAsync()
    {
        Console.WriteLine("Running cache update...");
        await _service.Run();
        Console.WriteLine("Placements cache updated.");
    }
}