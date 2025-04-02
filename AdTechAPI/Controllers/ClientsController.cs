using AdTechAPI.Models;
using AdTechAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientResponse>>> GetClients()
        {
            var clients = await _context.Clients
            .Select(c => new ClientResponse
            {
                Id = c.Id,
                Name = c.Name,
                Type = Enum.GetName(c.Type)
            })
                .ToListAsync();
            return Ok(clients);
        }


        [HttpGet("{id}")]

        public async Task<ActionResult<ClientResponse>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return new ClientResponse
            {
                Id = client.Id,
                Name = client.Name,
                Type = Enum.GetName(client.Type),
                CreatedAt = client.CreatedAt,
                UpdatedAt = client.UpdatedAt
            };
        }


        [HttpPost]
        public async Task<ActionResult<ClientResponse>> CreateClient(CreateClientRequest request)
        {
            var client = new Client
            {
                Name = request.Name,
                Type = request.Type,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, new ClientResponse
            {
                Id = client.Id,
                Name = client.Name,
                Type = Enum.GetName(client.Type),
                CreatedAt = client.CreatedAt,
                UpdatedAt = client.UpdatedAt
            });
        }

    }
}