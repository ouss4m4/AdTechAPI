using AdTechAPI.Models;
using AdTechAPI.Enums;

namespace AdTechAPI.Data
{
    public static class SeedData
    {
        public static List<Vertical> GetVerticals()
        {
            return new List<Vertical>
            {
                new Vertical
                {
                    Id = 1,
                    Name = "Health & Wellness",
                    Description = "Health, fitness, and wellness products",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Vertical
                {
                    Id = 2,
                    Name = "Finance",
                    Description = "Financial services and products",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Vertical
                {
                    Id = 3,
                    Name = "E-commerce",
                    Description = "Online retail and shopping",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        public static List<Country> GetCountries()
        {
            return new List<Country>
            {
                new Country
                {
                    Id = 1,
                    Name = "United States",
                    Code = "US",
                    Region = "North America",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Country
                {
                    Id = 2,
                    Name = "United Kingdom",
                    Code = "GB",
                    Region = "Europe",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Country
                {
                    Id = 3,
                    Name = "Canada",
                    Code = "CA",
                    Region = "North America",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        public static List<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    Id = 1,
                    Name = "Health Plus Inc",
                    Type = ClientType.Advertiser,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Client
                {
                    Id = 2,
                    Name = "Finance Direct",
                    Type = ClientType.Advertiser,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Client
                {
                    Id = 3,
                    Name = "Global Media Group",
                    Type = ClientType.Publisher,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }

        public static List<Lander> GetLanders()
        {
            return new List<Lander>
            {
                new Lander
                {
                    Id = 1,
                    Name = "Health Plus Landing Page",
                    Url = "https://healthplus.example.com/offer1",
                    Notes = "Main health products landing page",
                    AdvertiserId = 1
                },
                new Lander
                {
                    Id = 2,
                    Name = "Finance Direct Calculator",
                    Url = "https://financedirect.example.com/calculator",
                    Notes = "Financial calculator landing page",
                    AdvertiserId = 2
                }
            };
        }

        public static List<Campaign> GetCampaigns()
        {
            return new List<Campaign>
            {
                new Campaign
                {
                    Id = 1,
                    Name = "Health Plus Q2 Campaign",
                    AdvertiserId = 1,
                    LanderId = 1,
                    Notes = "Q2 health products promotion",
                    Status = CampaignStatus.Active,
                    Budget = 10000,
                    DailyBudget = 500,
                    Platforms = new List<int> { (int)Platform.Mobile, (int)Platform.Desktop },
                    Countries = new List<int> { 1, 2 }, // US and UK
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Campaign
                {
                    Id = 2,
                    Name = "Finance Direct Calculator Campaign",
                    AdvertiserId = 2,
                    LanderId = 2,
                    Notes = "Financial calculator promotion",
                    Status = CampaignStatus.Active,
                    Budget = 15000,
                    DailyBudget = 750,
                    Platforms = new List<int> { (int)Platform.Desktop },
                    Countries = new List<int> { 1, 2, 3 }, // US, UK, and Canada
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }
    }
}
