using System.Text.Json;
using AdTechAPI.PlacementCache;
using AdTechAPI.Services;


namespace AdTechAPI.ClickServices
{


    public class ClickPlacementService(RedisService redis, ILogger<ClickPlacementService> logger, AppDbContext db)
    {
        private readonly RedisService _redis = redis;
        private readonly ILogger<ClickPlacementService> _logger = logger;
        private readonly AppDbContext _db = db;

        public async Task<PlacementCacheData?> GetPlacementByUuidAsync(string placementUuid)
        {
            try
            {
                var placementsCacheData = await _redis.Db.StringGetAsync(PlacementCacheKeys.Pool);

                if (placementsCacheData.IsNullOrEmpty)
                {
                    _logger.LogWarning("Placement cache is empty or missing");
                    throw new Exception("Cache miss");
                }

                Dictionary<string, PlacementCacheData>? placements;

                try
                {
                    placements = JsonSerializer.Deserialize<Dictionary<string, PlacementCacheData>>(placementsCacheData.ToString());
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "Failed to deserialize placement cache");
                    throw;
                }

                if (placements == null || !placements.TryGetValue(placementUuid, out var placement) || placement == null)
                {
                    return null;
                }

                return placement;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get placement from cache. Falling back to DB.");

                if (!Guid.TryParse(placementUuid, out var placementGuid))
                {
                    _logger.LogWarning("Invalid UUID: {Uuid}", placementUuid);
                    return null;
                }

                var dbPlacement = _db.Placements.FirstOrDefault(p => p.Uuid == placementGuid);
                if (dbPlacement == null)
                {
                    return null;
                }

                return new PlacementCacheData
                {
                    PlacementId = dbPlacement.Id,
                    PlacementUuid = dbPlacement.Uuid.ToString(),
                    TrafficSourceId = dbPlacement.TrafficSourceId,
                    PublisherId = dbPlacement.PublisherId,
                    Verticals = dbPlacement.Verticals.ToArray()
                };
            }
        }
    }
}
