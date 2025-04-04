using System.ComponentModel.DataAnnotations;

namespace AdTechAPI.Models
{
    public class Click
    {
        [Required]
        public int Id
        {
            get; set;
        }
        [Required]
        public Guid Uuid
        {
            get; set;
        }

        [Required]
        public int PublisherId
        {
            get; set;
        }

        public virtual Client Publisher
        {
            get; set;
        }

        [Required]
        public int TrafficSourceId
        {
            get; set;
        }

        public virtual TrafficSource TrafficSource
        {
            get; set;
        }

        public int AdvertiserId
        {
            get; set;
        }

        public virtual Client Advertiser
        {
            get; set;
        }

        public int CampaignId
        {
            get; set;
        }
        public virtual Campaign Campaign
        {
            get; set;
        }

        public int LanderId
        {
            get; set;
        }

        public virtual Lander Lander
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