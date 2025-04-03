using AdTechAPI.Enums;
using AdTechAPI.Models;
using AdTechAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlacementsController : ControllerBase
    {
        private readonly AppDbContext _db;


        public PlacementsController(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Placement>>> GetPlacement()
        {
            var result = await _db.Placements
                                    .Include(p => p.Publisher)
                                    .Include(p => p.TrafficSource).ToArrayAsync();

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> UpdatePlacement(int id, UpdatePlacementRequest request)
        {
            var publisher = await _db.Clients
                .FirstOrDefaultAsync(c => c.Id == request.PublisherId && c.Type == ClientType.Publisher);

            if (publisher == null)
            {
                return BadRequest("Invalid publisher ID or client is not an publiser");

            }

            // Get verticals
            var verticals = await _db.Verticals
                .Where(v => request.Verticals.Contains(v.Id))
                .ToListAsync();

            if (verticals.Count != request.Verticals.Length)
            {
                return BadRequest("One or more vertical IDs are invalid");
            }

            var trafficSource = await _db.TrafficSources.FirstOrDefaultAsync(
                ts => ts.Id == request.TrafficSourceId && ts.PublisherId == request.PublisherId);

            if (trafficSource == null)
            {
                return BadRequest("Traffic source invalid, or does not belong to publisher");
            }

            var placement = await _db.Placements.FirstOrDefaultAsync();

            if (placement == null)
            {
                return NotFound("Placement not found");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                placement.Name = request.Name;
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Placement>> CreatePlacement(CreatePlacementRequest request)
        {
            var publisher = await _db.Clients
                .FirstOrDefaultAsync(c => c.Id == request.PublisherId && c.Type == ClientType.Publisher);

            if (publisher == null)
            {
                return BadRequest("Invalid publisher ID or client is not an publiser");

            }

            // Get verticals
            var verticals = await _db.Verticals
                .Where(v => request.Verticals.Contains(v.Id))
                .ToListAsync();

            if (verticals.Count != request.Verticals.Length)
            {
                return BadRequest("One or more vertical IDs are invalid");
            }

            var trafficSource = await _db.TrafficSources.FirstOrDefaultAsync(
                ts => ts.Id == request.TrafficSourceId && ts.PublisherId == request.PublisherId);

            if (trafficSource == null)
            {
                return BadRequest("Traffic source invalid, or does not belong to publisher");
            }

            var newPlacement = new Placement
            {
                Name = request.Name,
                PublisherId = request.PublisherId,
                TrafficSourceId = request.TrafficSourceId,
                Verticals = request.Verticals.ToList(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Placements.Add(newPlacement);

            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlacement), new
            {
                id = newPlacement.Id
            }, newPlacement);

        }

    }
}