using AdTechAPI.Models;
using AdTechAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AdTechAPI.PlacementCache;

public class BuildPlacementCache(
    RedisService redis,
    AppDbContext db,
    ILogger<BuildPlacementCache> logger)
{
    private readonly RedisService _redis = redis;
    private readonly AppDbContext _db = db;
    private readonly ILogger<BuildPlacementCache> _logger = logger;

    public async Task<List<Placement>> FetchPlacements()
    {
        return await _db.Placements
            .Include(p => p.Publisher)
            .Include(p => p.TrafficSource)
            .Where(p => p.Publisher != null && p.Publisher.Status == Enums.ClientStatus.Active)
            .ToListAsync();
    }

    public Dictionary<int, PlacementCacheData> BuildPlacementCacheData(List<Placement> placements)
    {
        var dict = new Dictionary<int, PlacementCacheData>();

        foreach (var placement in placements)
        {
            dict[placement.Id] = new PlacementCacheData
            {
                PlacementId = placement.Id,
                PublisherId = placement.PublisherId,
                TrafficSourceId = placement.TrafficSourceId
            };
        }

        return dict;
    }

    public async Task Run()
    {
        var activePlacements = await FetchPlacements();

        if (activePlacements.Count == 0)
        {
            _logger.LogInformation("No active placements found.");
            return;
        }

        var placementCache = BuildPlacementCacheData(activePlacements);

        var json = JsonSerializer.Serialize(placementCache, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await _redis.Db.StringSetAsync("cache::placements_pool", json);

        _logger.LogInformation("Placement cache updated at {Time}", DateTime.UtcNow);
    }
}
