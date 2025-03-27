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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<int> Platforms { get; set; } = new();
        public List<int> Countries { get; set; } = new();
        public ICollection<Vertical> Verticals { get; set; } = [];

        public void Validate()
        {
            if (Platforms.Distinct().Count() != Platforms.Count())
                throw new ArgumentException("Duplicate platforms are not allowed.");

            if (Platforms.Any(p => !Enum.IsDefined(typeof(Platform), p)))
                throw new ArgumentException("Invalid platform value.");
        }

    }

}