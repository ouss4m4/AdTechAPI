using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AdTechAPI.Models
{
    public class Campaign
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public int AdvertiserId { get; set; }
        public Client Advertiser { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal Budget { get; set; }
        public decimal DailyBudget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        [Column(TypeName = "jsonb")] // Ensure it's stored as JSONB in PostgreSQL
        public List<Platform> Platforms { get; set; } = [];
        // Vertical relationship (many-to-many)

        [Column(TypeName = "jsonb")]
        public List<int> Countries { get; set; } = [];

        public ICollection<Vertical> Verticals { get; set; } = [];

        public void Validate()
        {
            if (Platforms.Distinct().Count() != Platforms.Count)
                throw new ArgumentException("Duplicate platforms are not allowed.");

            if (Platforms.Any(p => !Enum.IsDefined(typeof(Platform), p)))
                throw new ArgumentException("Invalid platform value.");
        }

    }
}