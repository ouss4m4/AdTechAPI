using System.ComponentModel.DataAnnotations;
using AdTechAPI.Enums;

namespace AdTechAPI.Models.DTOs
{
    public class CreateTSourceRequest
    {
        [Required]
        [StringLength(100)]
        [MinLength(5)]
        public required string Name
        {
            get; set;
        }

        [Required]
        [EnumDataType(typeof(TrafficType))]
        public required TrafficType TrafficType
        {
            get; set;
        }

        public required int PublisherId
        {
            get; set;
        }
    }

    public class TSResponse
    {
        public int Id
        {
            get; set;
        }
        public Guid Uuid
        {
            get; set;
        }
        public string? Name
        {
            get; set;
        }
        public TrafficType TrafficType
        {
            get; set;
        }
        public int PublisherId
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