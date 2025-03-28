using System.ComponentModel.DataAnnotations;
using AdTechAPI.Enums;

namespace AdTechAPI.Models.DTOs
{
    public class CreateLanderRequest
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string Url { get; set; }

        public string? Notes { get; set; }

        [Required]
        public required int AdvertiserId { get; set; }

    }

    public class UpdateLanderRequest
    {
        public string? Name { get; set; }

        public string? Url { get; set; }

        public string? Notes { get; set; }

    }


    public class LanderResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Notes { get; set; }

        public string? Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}