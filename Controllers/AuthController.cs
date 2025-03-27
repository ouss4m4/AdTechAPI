using AdTechAPI.Models;
using AdTechAPI.Models.DTOs;
using AdTechAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using AdTechAPI.Enums;

namespace AdTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("Email already registered");
            }

            // Check if client exists
            var client = await _context.Clients.FindAsync(request.ClientId);
            if (client == null)
            {
                return BadRequest("Invalid client ID");
            }

            // Set user role based on client type
            UserRole userRole;
            if (client.Type == ClientType.Owner)
            {
                userRole = UserRole.Admin;
            }
            else
            {
                userRole = UserRole.Regular;
            }

            // Create new user
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                ClientId = request.ClientId,
                Role = userRole,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate token
            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ClientId = user.ClientId
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Client)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            }

            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ClientId = user.ClientId
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}