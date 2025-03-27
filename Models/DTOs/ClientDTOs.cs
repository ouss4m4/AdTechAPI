using System.ComponentModel.DataAnnotations;
using AdTechAPI.Enums;

namespace AdTechAPI.Models.DTOs
{
    public class CreateClientRequest
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [EnumDataType(typeof(ClientType))]
        public required ClientType Type { get; set; }
    }

    public class ClientResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}