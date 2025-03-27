using System.ComponentModel.DataAnnotations;

namespace AdTechAPI.Models.DTOs
{
    public class CreateVerticalRequest
    {
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        public string? Description { get; set; }
    }

    public class UpdateVerticalRequest
    {
        [StringLength(50)]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }

    public class VerticalResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}