using System.Text.Json.Serialization;

namespace AdTechAPI.Models
{
    public class Country
    {
        public int Id
        {
            get; set;
        }
        public required string Iso
        {
            get; set;
        }
        public required string Name
        {
            get; set;
        }

        public required string NiceName
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
