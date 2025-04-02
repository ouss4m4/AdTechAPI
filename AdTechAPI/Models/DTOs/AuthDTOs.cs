using AdTechAPI.Enums;

using System.ComponentModel.DataAnnotations;

namespace AdTechAPI.Models.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }

        [Required]
        public int ClientId { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }

    public class AuthResponse
    {
        public required string Token { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public UserRole Role { get; set; }
        public int ClientId { get; set; }
    }
}