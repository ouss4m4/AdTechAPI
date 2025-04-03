using System.Text.Json.Serialization;

namespace AdTechAPI.Models
{
    public class Lander
    {
        public int Id
        {
            get; set;
        }
        public required string Name
        {
            get; set;
        }
        public required string Url
        {
            get; set;
        }

        public string? Notes
        {
            get; set;
        }
        public required int AdvertiserId
        {
            get; set;
        }

        // Configure foreign key relationship
        [JsonIgnore]

        public Client? Advertiser
        {
            get; set;
        }

        public DateTime CreatedAt
        {
            get; set;
        }
        public DateTime UpdatedAt
        {
            get; set;
        }


    }
}

/* 
INSERT INTO public."Landers" ("Name", "Url", "Notes", "AdvertiserId")  
VALUES  
    -- Landers for E-Commerce Ads
    ('E-Commerce Deals', 'https://ecommerce-deals.com', 'Main landing page for promotions', 5),  
    ('Fashion Discounts', 'https://fashion.ecommerce-deals.com', 'Seasonal fashion discounts', 5),  

    -- Landers for Travel Deals Advertiser
    ('Cheap Flights', 'https://cheapflights.com', 'Landing page for flight deals', 6),  
    ('Luxury Hotels', 'https://luxuryhotels.com', 'Premium hotel deals', 6),  

    -- Landers for Fitness Products Advertiser
    ('Fitness Gear', 'https://fitnessgear.com', 'Landing page for fitness accessories', 7),  
    ('Healthy Supplements', 'https://healthysupplements.com', 'Supplement store page', 7);  
*/