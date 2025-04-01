using System.Text.Json.Serialization;

namespace AdTechAPI.Models
{
    public class Country
    {
        public int Id
        {
            get; set;
        }
        public required string Name
        {
            get; set;
        }
        public required string Code
        {
            get; set;
        }  // ISO 2-letter country code
        public string? Region
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

        [JsonIgnore]
        public ICollection<Campaign> Campaigns { get; set; } = [];
    }
}
