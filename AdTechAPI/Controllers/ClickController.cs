using System.Text.Json;
using AdTechAPI.CampaignsCache;
using AdTechAPI.ClickServices;
using AdTechAPI.Models;
using AdTechAPI.PlacementCache;
using AdTechAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly ILogger<ClickController> _logger;
        private readonly ClickPlacementService _placementService;
        private readonly ClickCampaignService _campaignService;
        public ClickController(AppDbContext db, RedisService redis, ILogger<ClickController> logger, ClickPlacementService placementService, ClickCampaignService campaignService)
        {
            _db = db;
            _redis = redis;
            _logger = logger;
            _placementService = placementService;
            _campaignService = campaignService;
        }

        [HttpGet("in/{placementUuid}")]
        public async Task<dynamic> HandleClick(string placementUuid)
        {

            // Step 1: get the placement
            var placement = await _placementService.GetPlacementByUuidAsync(placementUuid);
            if (placement == null)
            {
                return NotFound("Placement not found");
            }
            // [vertical][country]
            // step2: Get the Verticals - Country - Platform
            int[] verticalsIds = placement.Verticals;

            int deviceId = 3;
            int countryId = 1;

            // step3: get active campaigns for Vertical, Country, DEvice
            List<CampaignCacheData> candidateCampaigns = await _campaignService.GetCandidateCampaigns(verticalsIds, countryId, deviceId);
            if (candidateCampaigns.Count == 0)
            {
                return NotFound("No campaign available");

            }


            var random = new Random();
            var bestFit = candidateCampaigns[random.Next(candidateCampaigns.Count)];


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
                AdvertiserId = campaign.Advertiser?.Id ?? 0,
                CampaignId = campaign.Id,
                LanderId = campaign.LanderId,
                Revenue = Math.Round((decimal)(new Random().NextDouble() * (1.80 - 0.05) + 0.05), 2),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _db.Clicks.Add(click);

            await _db.SaveChangesAsync();

            return Ok(campaign);
        }


    }
}




