using AdTechAPI.Enums;
using AdTechAPI.Models;
using AdTechAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TrafficSourcesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrafficSourcesController(AppDbContext dbContext)
        {
            _context = dbContext;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrafficSource>>> GetSource()
        {
            var result = await _context.TrafficSources.ToArrayAsync();

            return Ok(result);
        }



        [HttpPost]

        public async Task<ActionResult<TSResponse>> CreateSource(CreateTSourceRequest dto)
        {

            var publisher = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == dto.PublisherId && c.Type == ClientType.Publisher);

            if (publisher == null)
            {
                return BadRequest("Invalid publisher ID or client is not an publisher");
            }


            TrafficSource TS = new TrafficSource
            {
                Uuid = Guid.NewGuid(),
                Name = dto.Name,
                TrafficType = dto.TrafficType,
                PublisherId = dto.PublisherId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.TrafficSources.Add(TS);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSource), new { id = TS.Id }, new TSResponse
            {
                Id = TS.Id,
                Uuid = TS.Uuid,
                Name = TS.Name,
                TrafficType = TS.TrafficType,
                PublisherId = TS.PublisherId,
            });

        }



    }
}