using AdTechAPI.Enums;

namespace AdTechAPI.Models  // Define the namespace here
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public UserRole Role { get; set; } = UserRole.Regular;
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}