using System.Text.Json;
using AdTechAPI.CampaignsCache;
using AdTechAPI.Models;
using AdTechAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class ClickController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly RedisService _redis;
        public ClickController(AppDbContext db, RedisService redis)
        {
            _db = db;
            _redis = redis;
        }

        [HttpGet("in/{placementUuid}")]
        public async Task<dynamic> HandleClick(string placementUuid)
        {
            try
            {
                // 1. Find the placement by UUID
                // TODO: Redis Cached placementss (flat structure)
                var placementUuidGuid = Guid.Parse(placementUuid);
                var placement = await _db.Placements.Include(p => p.Publisher).Include(p => p.TrafficSource).FirstOrDefaultAsync(p => p.Uuid == placementUuidGuid);
                if (placement == null)
                {
                    // _logger.LogWarning("Click attempted on invalid placement UUID: {PlacementUuid}", placementUuid);
                    return NotFound("Placement not found");
                }

                // 2. Get placement details needed for campaign selection
                // int trafficSourceId = placement.TrafficSourceId;
                // int publisherId = placement.PublisherId;
                // int placementId = placement.Id;
                int[] verticals = placement.Verticals.ToArray();

                // 3. Find the best matching campaign based on placement data
                // TODO: Redis Cached Campaigns
                // var campaign = await _campaignService.FindBestMatchingCampaignAsync(
                //     trafficSourceId,
                //     placementId,
                //     verticals);
                var cachedCampaignsJson = await _redis.Db.StringGetAsync("cache::campaigns_pool");
                var campaigns = JsonSerializer.Deserialize<Dictionary<int, Dictionary<int, Dictionary<int, HashSet<CampaignCacheData>>>>>(cachedCampaignsJson.ToString());

                var validCampaigns = campaigns[2][1][3].ToArray();
                // pick a random one
                var random = new Random();
                var bestFit = validCampaigns[random.Next(validCampaigns.Length)];


                // TODO: we get campaign id from cache. query it (later: Have a campaigns cache, no db calls. and only ids in campaignsStore)
                var campaign = await _db.Campaigns
                    .Include(c => c.Lander)
                    .Include(c => c.Advertiser)
                    .FirstOrDefaultAsync(c => c.Id == bestFit.CampaignId);
                if (campaign == null)
                {
                    return NotFound("No matching campaign");
                }

                // 4. Log the click event
                // await _campaignService.LogClickAsync(placement.Id, campaign.Id);

                // 5. Redirect to the campaign landing URL
                // return campaign;
                var click = new Click
                {
                    Uuid = Guid.NewGuid(),
                    PublisherId = placement.PublisherId,
                    TrafficSourceId = placement.TrafficSourceId,
                    AdvertiserId = campaign.Advertiser.Id,
                    CampaignId = campaign.Id,
                    LanderId = campaign.LanderId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _db.Clicks.Add(click);

                await _db.SaveChangesAsync();
                return Redirect(campaign.Lander.Url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // _logger.LogError(ex, "Error processing click for placement {PlacementUuid}", placementUuid);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }



    }
}