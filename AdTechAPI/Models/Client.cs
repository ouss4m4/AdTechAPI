using System.Text.Json.Serialization;
using AdTechAPI.Enums;

namespace AdTechAPI.Models  // Define the namespace here
{
    public class Client
    {
        public int Id
        {
            get; set;
        }
        public required string Name
        {
            get; set;
        }
        public ClientType Type
        {
            get; set;
        }

        public ClientStatus Status
        {
            get; set;
        } = ClientStatus.Inactive;
        public DateTime CreatedAt
        {
            get; set;
        }
        public DateTime UpdatedAt
        {
            get; set;
        }
        [JsonIgnore]
        public ICollection<User> Users { get; set; } = [];

    }
}

/*
INSERT INTO public."Clients" ("Name", "Type", "Status", "CreatedAt", "UpdatedAt")  
VALUES  
    -- Owner Client
    ('Admin Client', 3, 1, NOW(), NOW()),  

    -- Publishers
    ('Tech News Publisher', 1, 1, NOW(), NOW()),  
    ('Finance Insights Publisher', 1, 1, NOW(), NOW()),  
    ('Health Guide Publisher', 1, 1, NOW(), NOW()),  

    -- Advertisers
    ('E-Commerce Ads', 2, 1, NOW(), NOW()),  
    ('Travel Deals Advertiser', 2, 1, NOW(), NOW()),  
    ('Fitness Products Advertiser', 2, 1, NOW(), NOW());  
*/