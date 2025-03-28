using AdTechAPI.Enums;
using AdTechAPI.Models;
using AdTechAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LandersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LandersController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lander>>> GetUsers()
        {
            var landers = await _context.Landers
                .ToListAsync();
            return Ok(landers);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lander>> GetLander(int id)
        {
            var lander = await _context.Landers
                .Include(l => l.Advertiser)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lander == null)
            {
                return NotFound();
            }

            return Ok(lander);
        }

        [HttpPost]
        public async Task<ActionResult<Lander>> CreateLander(CreateLanderRequest lander)
        {
            var advertiser = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == lander.AdvertiserId && c.Type == ClientType.Advertiser);

            if (advertiser == null)
            {
                return BadRequest("Invalid advertiser ID or client is not an advertiser");
            }


            var newLander = new Lander
            {
                Name = lander.Name,
                Url = lander.Url,
                AdvertiserId = lander.AdvertiserId
            };

            _context.Landers.Add(newLander);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLander), new { id = newLander.Id }, lander);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLander(int id, UpdateLanderRequest dto)
        {


            Lander lander = _context.Landers.Where(lander => lander.Id == id).First();

            if (lander == null)
            {
                return NotFound();
            }

            if (dto.Name != null) lander.Name = dto.Name;
            if (dto.Url != null) lander.Url = dto.Url;
            if (dto.Notes != null) lander.Notes = dto.Notes;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception err)
            {

                return BadRequest(err);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLander(int id)
        {
            var lander = await _context.Landers.FindAsync(id);
            if (lander == null)
            {
                return NotFound();
            }

            _context.Landers.Remove(lander);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}