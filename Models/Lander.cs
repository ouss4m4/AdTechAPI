using System.Text.Json.Serialization;

namespace AdTechAPI.Models
{
    public class Lander
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }

        public string? Notes { get; set; }
        public required int AdvertiserId { get; set; }

        // Configure foreign key relationship
        [JsonIgnore]

        public Client? Advertiser { get; set; }


    }
}