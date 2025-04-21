namespace AdTechAPI.CampaignsCache
{
    public static class CampaignCacheKeys
    {
        public const string Pool = "cache::campaigns_pool";
    }

    public class CampaignCacheData : IEquatable<CampaignCacheData>
    {
        public int CampaignId
        {
            get; set;
        }
        public required string Name
        {
            get; set;
        }
        public int Status
        {
            get; set;
        }
        public int LanderId
        {
            get; set;
        }
        public required string LanderUrl
        {
            get; set;
        }

        public override bool Equals(object? obj)
        {
            return obj is CampaignCacheData other && Equals(other);
        }

        public bool Equals(CampaignCacheData? other)
        {
            return other is not null && CampaignId == other.CampaignId;
        }

        public override int GetHashCode()
        {
            return CampaignId.GetHashCode();
        }
    }

    public class CampaignsCachePool : Dictionary<int, Dictionary<int, Dictionary<int, HashSet<CampaignCacheData>>>>
    {
    }

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
        public required List<int> Countries
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

        public required string LanderUrl
        {
            get; set;
        }
        public required string Name
        {
            get; set;
        }
        public string? Notes
        {
            get; set;
        }
        public required List<int> Platforms
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
        public int VerticalId
        {
            get; set;
        } // Matches the exact column name from the SQL query
    }

}
