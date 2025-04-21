using System.Text.Json;
using AdTechAPI.CampaignsCache;
using AdTechAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.ClickServices
{
    public class ClickCampaignService(RedisService redis, ILogger<ClickPlacementService> logger, AppDbContext db)
    {
        private readonly RedisService _redis = redis;
        private readonly ILogger<ClickPlacementService> _logger = logger;
        private readonly AppDbContext _db = db;

        public async Task<List<CampaignCacheData>> GetCandidateCampaigns(int[] verticalsIds, int countryId, int deviceId)
        {

            try
            {
                var campaignsCacheData = await _redis.Db.StringGetAsync(CampaignCacheKeys.Pool);

                if (campaignsCacheData.IsNullOrEmpty)
                {
                    _logger.LogWarning("Placement cache is empty or missing");
                    throw new Exception("Cache miss");
                }

                CampaignsCachePool? campaignsPool;

                try
                {
                    campaignsPool = JsonSerializer.Deserialize<CampaignsCachePool>(campaignsCacheData.ToString());
                    if (campaignsPool == null)
                    {
                        throw new Exception("Campaign cache is null");
                    }
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "Failed to deserialize campaigns cache");
                    throw;
                }

                Dictionary<int, CampaignCacheData> candidateCampaigns = new Dictionary<int, CampaignCacheData>();
                foreach (var verticalId in verticalsIds)
                {
                    campaignsPool.TryGetValue(verticalId, out var verticalCampaigns);
                    if (verticalCampaigns == null)
                    {
                        continue;
                    }
                    verticalCampaigns.TryGetValue(countryId, out var countryCampaigns);
                    if (countryCampaigns == null)
                    {
                        continue;
                    }

                    countryCampaigns.TryGetValue(deviceId, out var deviceCampaigns);
                    if (deviceCampaigns == null)
                    {
                        continue;
                    }
                    foreach (var campaign in deviceCampaigns)
                    {
                        candidateCampaigns.Add(campaign.CampaignId, campaign);
                    }

                }


                // no duplicate campaigns needed here.
                return [.. candidateCampaigns.Values];
            }
            catch (Exception ex)
            {
                // fallback to the db and grab the campaigns if cache fails.
                _logger.LogError(ex, "Failed to get placement from cache. Falling back to DB.");

                var campaigns = await _db.Campaigns
                    .Where(c => c.Verticals.Any(v => verticalsIds.Contains(v.Id)))
                    .Where(c => c.Countries.Any(c => c == countryId))
                    .Where(c => c.Platforms.Any(p => p == deviceId))
                    .Include(c => c.Verticals)
                    .Include(c => c.Lander)
                    .ToListAsync();

                return campaigns.Select(camp => new CampaignCacheData
                {
                    LanderId = camp.LanderId,
                    CampaignId = camp.Id,
                    LanderUrl = camp.Lander?.Url ?? "",
                    Name = camp.Name,
                    Status = (int)camp.Status
                }).ToList();
            }

        }
    }
}
