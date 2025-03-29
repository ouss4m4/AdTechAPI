using AdTechAPI.Enums;

namespace AdTechAPI.Models
{
    public class TrafficSource
    {
        public int Id { get; set; }

        public Guid Uuid { get; set; }

        public required string Name { get; set; }
        public TrafficType TrafficType { get; set; }
        public int PublisherId { get; set; }

        public Client Publisher { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}