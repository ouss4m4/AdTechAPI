using AdTechAPI.CampaignsCache;
using AdTechAPI.ClickServices;
using AdTechAPI.Models;
using AdTechAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AdTechAPI.Helpers;
using AdTechAPI.CachedData;

namespace AdTechAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class ClickController(AppDbContext db, RedisService redis, ILogger<ClickController> logger, ClickPlacementService placementService, ClickCampaignService campaignService, GeoIPService geoIp, CountriesCache countriesCache) : ControllerBase
    {
        private readonly AppDbContext _db = db;
        private readonly RedisService _redis = redis;
        private readonly ILogger<ClickController> _logger = logger;
        private readonly ClickPlacementService _placementService = placementService;
        private readonly ClickCampaignService _campaignService = campaignService;
        private readonly GeoIPService _geoIp = geoIp;
        private readonly CountriesCache _countriesCache = countriesCache;

        [HttpGet("in/{placementUuid}")]
        public async Task<dynamic> HandleClick(string placementUuid)
        {

            /**
            * Validate User Agent. Device (phone, tablet, desktop)
            * get DeviceId
            */
            var userAgent = Request.Headers.UserAgent.ToString();
            if (userAgent == null)
            {
                return BadRequest("User Agent not found");
            }

            int DeviceId = DeviceHelper.GetDeviceIdFromUserAgent(userAgent);
            var forwardedIp = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var ip = !string.IsNullOrEmpty(forwardedIp)
                ? forwardedIp
                : HttpContext.Connection.RemoteIpAddress?.ToString();

            if (ip == null)
            {
                return BadRequest("User IP not found");
            }

            /**
            * GeoLocate using IP.
            * Get CountryId
            */
            var CountryIso = _geoIp.GetCountryIso(ip) ?? "US";
            if (string.IsNullOrEmpty(CountryIso))
            {
                return BadRequest($"Can not locate user, ip: {ip}, countryIso {CountryIso}");
            }


            var CountryId = await _countriesCache.GetCountryIdByIso(CountryIso) ?? 0;


            // STEP 1: Get The Placement
            var placement = await _placementService.GetPlacementByUuidAsync(placementUuid);
            if (placement == null)
            {
                return NotFound("Placement not found");
            }

            // step2: Get the Verticals from placement
            int[] VerticalsIds = placement.Verticals;

            // step3: get active campaigns for Vertical, Country, Device
            List<CampaignCacheData> candidateCampaigns = await _campaignService.GetCandidateCampaigns(VerticalsIds, CountryId, DeviceId);
            if (candidateCampaigns.Count == 0)
            {
                return NotFound("No campaign available");

            }


            var random = new Random();
            var bestFit = candidateCampaigns[random.Next(candidateCampaigns.Count)];

            var campaign = await _db.Campaigns
                .Include(c => c.Lander)
                .Include(c => c.Advertiser)
                .FirstOrDefaultAsync(c => c.Id == bestFit.CampaignId);
            if (campaign == null)
            {
                return NotFound("No matching campaign");
            }

            // 4. Log the click event
            _logger.LogInformation("Click from Placement Id: {placement.Id} matched with campaign ID: {campaign.Id}", placement.PlacementId, campaign.Id);

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

            // Raise click to Kafka. instead of going to the db. so we can Batch
            // await _db.SaveChangesAsync();


            // return Ok(campaign);
            return Redirect(campaign.Lander.Url);
        }


    }
}




