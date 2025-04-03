namespace AdTechAPI.Models
{
    public class Placement
    {

        public int Id
        {
            get; set;
        }

        public required string Name
        {
            get; set;
        }

        public int PublisherId
        {
            get; set;
        }

        public Client? Publisher
        {
            get; set;
        }
        public int TrafficSourceId
        {
            get; set;
        }

        public TrafficSource? TrafficSource
        {
            get; set;
        }

        public required List<int> Verticals
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