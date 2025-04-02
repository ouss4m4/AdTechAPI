using System.ComponentModel.DataAnnotations;
using AdTechAPI.Enums;

namespace AdTechAPI.Models.DTOs
{
    public class CreateCampaignRequest
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public int AdvertiserId { get; set; }


        [Required]
        [MinLength(1)]
        public int[] Verticals { get; set; } = [];

        [Required]
        public int LanderId { get; set; }


        [Required]
        [Range(0, double.MaxValue)]
        public decimal Budget { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal DailyBudget { get; set; }

        public string? Notes { get; set; }


        [Required]
        [MinLength(1)]
        [MaxLength(3)]
        public required int[] Platforms { get; set; } = [];


        public CampaignStatus Status { get; set; }

        [Required]
        [MinLength(1)]
        public required int[] Countries { get; set; } = [];
    }

    public class UpdateCampaignRequest
    {
        [StringLength(100)]
        public string? Name { get; set; }


        public int? LanderId { get; set; }

        public string? Notes { get; set; }
        public CampaignStatus? Status { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Budget { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? DailyBudget { get; set; }

        [MinLength(1)]
        [MaxLength(3)]
        public int[]? Platforms { get; set; }

        [MinLength(1)]
        public int[]? Verticals { get; set; }

        [MinLength(1)]
        public int[]? Countries { get; set; }
    }

    public class CampaignResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int AdvertiserId { get; set; }
        public string? Notes { get; set; }
        public int Status { get; set; }
        public decimal Budget { get; set; }
        public decimal DailyBudget { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int[] Platforms { get; set; } = [];
        public int[] Verticals { get; set; } = [];
        public string[] VerticalNames { get; set; } = [];
        public int[] Countries { get; set; } = [];
    }
}