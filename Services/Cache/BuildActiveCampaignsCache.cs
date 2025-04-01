using AdTechAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdTechAPI.CacheBuildersServices
{
    public class CampaignWithVerticalDTO
    {
        public int Id
        {
            get; set;
        }
        public int AdvertiserId
        {
            get; set;
        }
        public decimal Budget
        {
            get; set;
        }
        public List<int>? Countries
        {
            get; set;
        }
        public int? CountryId
        {
            get; set;
        }
        public DateTime CreatedAt
        {
            get; set;
        }
        public decimal DailyBudget
        {
            get; set;
        }
        public int LanderId
        {
            get; set;
        }
        public string? Name
        {
            get; set;
        }
        public string? Notes
        {
            get; set;
        }
        public List<int>? Platforms
        {
            get; set;
        }
        public int Status
        {
            get; set;
        }
        public DateTime UpdatedAt
        {
            get; set;
        }
        public int? VerticalId
        {
            get; set;
        } // Matches the exact column name from the SQL query
    }

    public class BuildActiveCampaignsCache
    {
        /*
        Get Active Campaigns from the DB
        datastructure it for easy access
        save it in redis
        */

        private readonly AppDbContext _context;
        private readonly ILogger<BuildActiveCampaignsCache> _logger;

        public BuildActiveCampaignsCache(AppDbContext context, ILogger<BuildActiveCampaignsCache> logger)
        {
            _context = context;
            _logger = logger;

        }


        public async Task Run()
        {
            var activeCampaigns = await _context.Database
                .SqlQuery<CampaignWithVerticalDTO>($@"
                    WITH ActiveClients AS (
                        SELECT ""Id""
                        FROM ""Clients""
                        WHERE ""Status"" = 1
                    )
                    SELECT c.""Id"", c.""AdvertiserId"", c.""Budget"", c.""Countries"", c.""CountryId"", 
                           c.""CreatedAt"", c.""DailyBudget"", c.""LanderId"", c.""Name"", c.""Notes"", 
                           c.""Platforms"", c.""Status"", c.""UpdatedAt"",
                           cv.""VerticalsId"" AS ""VerticalId""
                    FROM ""Campaigns"" AS c
                    JOIN ActiveClients AS ac ON c.""AdvertiserId"" = ac.""Id""
                    LEFT JOIN ""CampaignVerticals"" AS cv ON c.""Id"" = cv.""CampaignsId""
                    WHERE c.""Status"" = 1
                    ORDER BY c.""Id"", cv.""VerticalsId""
                ")
                .ToListAsync();


        }


    }
}