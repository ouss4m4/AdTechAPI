using AdTechAPI.Models;
using AdTechAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerticalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VerticalsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Verticals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VerticalResponse>>> GetVerticals()
        {
            var verticals = await _context.Verticals
                .Select(v => new VerticalResponse
                {
                    Id = v.Id,
                    Name = v.Name,
                    Description = v.Description,
                    CreatedAt = v.CreatedAt,
                    UpdatedAt = v.UpdatedAt
                })
                .ToListAsync();

            return Ok(verticals);
        }

        // GET: api/Verticals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VerticalResponse>> GetVertical(int id)
        {
            var vertical = await _context.Verticals.FindAsync(id);

            if (vertical == null)
            {
                return NotFound();
            }

            return new VerticalResponse
            {
                Id = vertical.Id,
                Name = vertical.Name,
                Description = vertical.Description,
                CreatedAt = vertical.CreatedAt,
                UpdatedAt = vertical.UpdatedAt
            };
        }

        // POST: api/Verticals
        [HttpPost]
        public async Task<ActionResult<VerticalResponse>> CreateVertical(CreateVerticalRequest request)
        {
            var vertical = new Vertical
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Verticals.Add(vertical);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVertical), new { id = vertical.Id }, new VerticalResponse
            {
                Id = vertical.Id,
                Name = vertical.Name,
                Description = vertical.Description,
                CreatedAt = vertical.CreatedAt,
                UpdatedAt = vertical.UpdatedAt
            });
        }

        // PUT: api/Verticals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVertical(int id, UpdateVerticalRequest request)
        {
            var vertical = await _context.Verticals.FindAsync(id);
            if (vertical == null)
            {
                return NotFound();
            }

            if (request.Name != null) vertical.Name = request.Name;
            if (request.Description != null) vertical.Description = request.Description;

            vertical.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Verticals.Any(v => v.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Verticals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVertical(int id)
        {
            var vertical = await _context.Verticals.FindAsync(id);
            if (vertical == null)
            {
                return NotFound();
            }

            _context.Verticals.Remove(vertical);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}