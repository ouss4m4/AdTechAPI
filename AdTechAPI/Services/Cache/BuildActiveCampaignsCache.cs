using Microsoft.EntityFrameworkCore;
using AdTechAPI.Services;

namespace AdTechAPI.CampaignsCache
{
    public class BuildActiveCampaignsCache
    {
        /*
        Get Active Campaigns from the DB
        datastructure it for easy access
        save it in redis
        */

        private readonly AppDbContext _context;
        private readonly ILogger<BuildActiveCampaignsCache> _logger;
        private readonly RedisService _redis;

        public BuildActiveCampaignsCache(AppDbContext context, ILogger<BuildActiveCampaignsCache> logger, RedisService redis)
        {
            _context = context;
            _logger = logger;
            _redis = redis;
        }


        public async Task<List<CampaignWithVerticalDTO>> FetchActiveCampaigns()
        {
            return await _context.Database
                .SqlQuery<CampaignWithVerticalDTO>($@"
                    WITH ActiveClients AS (
                        SELECT ""Id""
                        FROM ""Clients""
                        WHERE ""Status"" = 1
                    )
                    SELECT c.""Id"", c.""AdvertiserId"", c.""Budget"", c.""Countries"", c.""CountryId"", 
                           c.""CreatedAt"", c.""DailyBudget"", c.""LanderId"", c.""Name"", c.""Notes"", 
                           c.""Platforms"", c.""Status"", c.""UpdatedAt"",
                           cv.""VerticalsId"" AS ""VerticalId"",
                           l.""Url"" as ""LanderUrl""
                    FROM ""Campaigns"" AS c
                    JOIN ActiveClients AS ac ON c.""AdvertiserId"" = ac.""Id""
                    LEFT JOIN ""CampaignVerticals"" AS cv ON c.""Id"" = cv.""CampaignsId""
                    LEFT JOIN ""Landers"" AS l ON l.""Id"" = c.""LanderId""
                    WHERE c.""Status"" = 1
                    ORDER BY c.""Id"", cv.""VerticalsId""
                ")
                .ToListAsync();
        }

        public CampaignCacheStore FormatCampaignsToCacheStructure(List<CampaignWithVerticalDTO> campaigns)
        {
            // Create a nested dictionary to represent the cache structure
            CampaignCacheStore campaignsStore = new();

            foreach (var campaign in campaigns)
            {
                // start by [vertical][country] then [platform]
                foreach (var country in campaign.Countries)
                {

                    // check vertical exist or add it
                    if (!campaignsStore.Items.ContainsKey(campaign.VerticalId))
                    {
                        campaignsStore.Items[campaign.VerticalId] = [];
                    }

                    //  check country exist or add it
                    if (!campaignsStore.Items[campaign.VerticalId].ContainsKey(country))
                    {
                        campaignsStore.Items[campaign.VerticalId][country] = [];
                    }

                    foreach (var platform in campaign.Platforms)
                    {
                        // check if there is already a platform or add it
                        if (!campaignsStore.Items[campaign.VerticalId][country].ContainsKey(platform))
                        {
                            campaignsStore.Items[campaign.VerticalId][country][platform] = new HashSet<CampaignCacheData>(); ;
                        }
                        // Add campaign data to the cache with the proper keys
                        campaignsStore.Items[campaign.VerticalId][country][platform].Add(new CampaignCacheData
                        {
                            CampaignId = campaign.Id,
                            Name = campaign.Name,
                            Status = campaign.Status,
                            LanderUrl = campaign.LanderUrl
                        });
                    }
                }
            }
            return campaignsStore;
        }
        public async Task Run()
        {

            var activeCampaigns = await FetchActiveCampaigns();

            if (activeCampaigns.Count() > 0)
            {

                var activeCampaignsCacheStrucutre = FormatCampaignsToCacheStructure(activeCampaigns);

                var json = System.Text.Json.JsonSerializer.Serialize(activeCampaignsCacheStrucutre.Items, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // SET The new campaign pool in cache
                await _redis.Db.StringSetAsync("cache::campaigns_pool", json);
            }
        }

    }
}