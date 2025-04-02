using AdTechAPI.Models;
using AdTechAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdTechAPI.Enums;


namespace AdTechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CampaignsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignResponse>>> GetCampaigns()
        {
            var campaigns = await _context.Campaigns
                .Include(c => c.Advertiser)
                .Include(c => c.Verticals)
                .ToListAsync();
            return Ok(campaigns);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CampaignResponse>> GetCampaign(int id)
        {
            var campaign = await _context.Campaigns
                .Include(c => c.Advertiser)
                .Include(c => c.Verticals)
                .Include(c => c.Lander)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null)
            {
                return NotFound();
            }
            return Ok(campaign);
            // return new CampaignResponse
            // {
            //     Id = campaign.Id,
            //     Name = campaign.Name,
            //     AdvertiserId = campaign.AdvertiserId,
            //     Notes = campaign.Notes,
            //     Status = (int)campaign.Status,
            //     Budget = campaign.Budget,
            //     DailyBudget = campaign.DailyBudget,

            //     CreatedAt = campaign.CreatedAt,
            //     UpdatedAt = campaign.UpdatedAt,
            //     Platforms = campaign.Platforms.ToArray(),
            //     Verticals = campaign.Verticals.Select(v => v.Id).ToArray(),
            //     VerticalNames = campaign.Verticals.Select(v => v.Name).ToArray(),
            //     Countries = campaign.Countries.ToArray()
            // };
        }


        [HttpPost]
        public async Task<ActionResult<CampaignResponse>> CreateCampaign(CreateCampaignRequest request)
        {

            // Verify advertiser exists and is of type Advertiser
            var advertiser = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == request.AdvertiserId && c.Type == ClientType.Advertiser);

            if (advertiser == null)
            {
                return BadRequest("Invalid advertiser ID or client is not an advertiser");
            }

            // Get verticals
            var verticals = await _context.Verticals
                .Where(v => request.Verticals.Contains(v.Id))
                .ToListAsync();

            if (verticals.Count != request.Verticals.Length)
            {
                return BadRequest("One or more vertical IDs are invalid");
            }

            var campaign = new Campaign
            {
                Name = request.Name,
                AdvertiserId = request.AdvertiserId,
                LanderId = request.LanderId,
                Notes = request.Notes,
                Budget = request.Budget,
                DailyBudget = request.DailyBudget > 0 ? request.DailyBudget : 0,
                Platforms = request.Platforms.ToList(),
                Countries = request.Countries.ToList(),
                Verticals = verticals,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            campaign.Validate();

            // return Ok(campaign);
            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCampaign), new { id = campaign.Id }, new CampaignResponse
            {
                Id = campaign.Id,
                Name = campaign.Name,
                AdvertiserId = campaign.AdvertiserId,
                Notes = campaign.Notes,
                Status = (int)campaign.Status,
                Budget = campaign.Budget,
                DailyBudget = campaign.DailyBudget,
                CreatedAt = campaign.CreatedAt,
                UpdatedAt = campaign.UpdatedAt,
                Platforms = request.Platforms,
                Verticals = verticals.Select(v => v.Id).ToArray(),
                VerticalNames = verticals.Select(v => v.Name).ToArray(),
                Countries = request.Countries
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampaign(int id, UpdateCampaignRequest request)
        {
            var campaign = await _context.Campaigns
                .Include(c => c.Verticals)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null)
            {
                return NotFound();
            }

            // Track changes to prevent unnecessary DB updates
            bool isUpdated = false;

            if (!string.IsNullOrWhiteSpace(request.Name) && campaign.Name != request.Name)
            {
                campaign.Name = request.Name;
                isUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Notes) && campaign.Notes != request.Notes)
            {
                campaign.Notes = request.Notes;
                isUpdated = true;
            }

            if (request.Status.HasValue && campaign.Status != request.Status.Value)
            {
                campaign.Status = request.Status.Value;
                isUpdated = true;
            }

            if (request.Budget.HasValue && campaign.Budget != request.Budget.Value)
            {
                campaign.Budget = request.Budget.Value;
                isUpdated = true;
            }

            if (request.DailyBudget.HasValue && campaign.DailyBudget != request.DailyBudget.Value)
            {
                campaign.DailyBudget = request.DailyBudget.Value;
                isUpdated = true;
            }

            if (request.LanderId.HasValue && campaign.LanderId != request.LanderId.Value)
            {
                bool landerExists = await _context.Landers.AnyAsync(l => l.Id == request.LanderId.Value);
                if (!landerExists)
                {
                    return BadRequest("Invalid Lander ID.");
                }

                campaign.LanderId = request.LanderId.Value;
                isUpdated = true;
            }

            if (request.Platforms != null)
            {
                var uniquePlatforms = request.Platforms.Distinct().ToList();
                if (uniquePlatforms.Any(p => !Enum.IsDefined(typeof(Platform), p)))
                {
                    return BadRequest("Invalid platform value.");
                }

                if (!campaign.Platforms.SequenceEqual(uniquePlatforms))
                {
                    campaign.Platforms = uniquePlatforms;
                    isUpdated = true;
                }
            }

            if (request.Countries != null)
            {
                var uniqueCountries = request.Countries.Distinct().ToList();
                if (!campaign.Countries.SequenceEqual(uniqueCountries))
                {
                    campaign.Countries = uniqueCountries;
                    isUpdated = true;
                }
            }

            if (request.Verticals != null)
            {
                var verticals = await _context.Verticals
                    .Where(v => request.Verticals.Contains(v.Id))
                    .ToListAsync();

                if (verticals.Count != request.Verticals.Length)
                {
                    return BadRequest("One or more vertical IDs are invalid.");
                }

                if (!campaign.Verticals.Select(v => v.Id).OrderBy(id => id)
                        .SequenceEqual(verticals.Select(v => v.Id).OrderBy(id => id)))
                {
                    campaign.Verticals = verticals;
                    isUpdated = true;
                }
            }

            if (!isUpdated)
            {
                return NoContent();
            }

            campaign.UpdatedAt = DateTime.UtcNow;

            // Model-level validation
            try
            {
                campaign.Validate();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // // DELETE: api/Campaigns/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteCampaign(int id)
        // {
        //     var campaign = await _context.Campaigns.FindAsync(id);
        //     if (campaign == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Campaigns.Remove(campaign);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }
    }
}