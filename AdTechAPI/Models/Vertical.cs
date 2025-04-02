using System.Text.Json.Serialization;

namespace AdTechAPI.Models
{
    public class Vertical
    {
        public int Id
        {
            get; set;
        }
        public required string Name
        {
            get; set;
        }
        public string? Description
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