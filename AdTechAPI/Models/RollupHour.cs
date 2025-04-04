using System.ComponentModel.DataAnnotations.Schema;

namespace AdTechAPI.Models
{
    public class RollupHour
    {
        public int Id
        {
            get; set;
        }

        public DateOnly StatDate
        {
            get; set;
        }
        public DateTime StatHour
        {
            get; set;
        }

        public required int PublisherId
        {
            get; set;
        }

        public required int TrafficSourceId
        {
            get; set;
        }

        public required int AdvertiserId
        {
            get; set;
        }

        public required int CampaignId
        {
            get; set;
        }

        public required int LanderId
        {
            get; set;
        }

        public required int Clicks
        {
            get; set;
        }

        [Column(TypeName = "numeric(10, 2)")]
        public decimal Revenue
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

