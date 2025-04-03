using System.ComponentModel.DataAnnotations;

namespace AdTechAPI.Models.DTOs
{
    public class CreatePlacementRequest
    {
        public required string Name
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

        public required int[] Verticals
        {
            get; set;
        } = [];

        public DateTime CreatedAt
        {
            get; set;
        }
        public DateTime UpdatedAt
        {
            get; set;
        }

    }

    public class UpdatePlacementRequest
    {
        public string? Name
        {
            get; set;
        }

        public int? PublisherId
        {
            get; set;
        }

        public int? TrafficSourceId
        {
            get; set;
        }

        public int[] Verticals
        {
            get; set;
        } = [];
    }
}