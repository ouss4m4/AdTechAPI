using System.Text.Json.Serialization;
using AdTechAPI.Enums;

namespace AdTechAPI.Models  // Define the namespace here
{
    public class Client
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ClientType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public ICollection<User> Users { get; set; } = [];

    }
}