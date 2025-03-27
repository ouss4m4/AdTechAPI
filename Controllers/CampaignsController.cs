using AdTechAPI.Models;
using AdTechAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
            // return Ok(campaigns.Select(c => new CampaignResponse
            // {
            //     Id = c.Id,
            //     Name = c.Name,
            //     AdvertiserId = c.AdvertiserId,
            //     Description = c.Description,
            //     IsActive = c.IsActive,
            //     Budget = c.Budget,
            //     DailyBudget = c.DailyBudget,
            //     StartDate = c.StartDate,
            //     EndDate = c.EndDate,
            //     CreatedAt = c.CreatedAt,
            //     UpdatedAt = c.UpdatedAt,
            //     Platforms = JsonSerializer.Deserialize<string[]>(c.Platforms != null ? JsonSerializer.Serialize(c.Platforms) : "[]") ?? [],
            //     VerticalIds = c.Verticals.Select(v => v.Id).ToArray(),
            //     VerticalNames = c.Verticals.Select(v => v.Name).ToArray(),
            //     Countries = JsonSerializer.Deserialize<string[]>(c.Countries ?? "[]") ?? []
            // }));
        }

        // GET: api/Campaigns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CampaignResponse>> GetCampaign(int id)
        {
            var campaign = await _context.Campaigns
                .Include(c => c.Advertiser)
                .Include(c => c.Verticals)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null)
            {
                return NotFound();
            }

            return new CampaignResponse
            {
                Id = campaign.Id,
                Name = campaign.Name,
                AdvertiserId = campaign.AdvertiserId,
                Description = campaign.Description,
                IsActive = campaign.IsActive,
                Budget = campaign.Budget,
                DailyBudget = campaign.DailyBudget,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                CreatedAt = campaign.CreatedAt,
                UpdatedAt = campaign.UpdatedAt,
                Platforms = campaign.Platforms.ToArray(),
                Verticals = campaign.Verticals.Select(v => v.Id).ToArray(),
                VerticalNames = campaign.Verticals.Select(v => v.Name).ToArray(),
                Countries = campaign.Countries.ToArray()
            };
        }

        // [HttpGet("Advertiser/{advertiserId}")]
        // public async Task<ActionResult<IEnumerable<CampaignResponse>>> GetCampaignsByAdvertiser(int advertiserId)
        // {
        //     var campaigns = await _context.Campaigns
        //         .Include(c => c.Advertiser)
        //         .Include(c => c.Verticals)
        //         .Where(c => c.AdvertiserId == advertiserId)
        //         .ToListAsync();

        //     return Ok(campaigns.Select(c => new CampaignResponse
        //     {
        //         Id = c.Id,
        //         Name = c.Name,
        //         AdvertiserId = c.AdvertiserId,
        //         Description = c.Description,
        //         IsActive = c.IsActive,
        //         Budget = c.Budget,
        //         DailyBudget = c.DailyBudget,
        //         StartDate = c.StartDate,
        //         EndDate = c.EndDate,
        //         CreatedAt = c.CreatedAt,
        //         UpdatedAt = c.UpdatedAt,
        //         Platforms = JsonSerializer.Deserialize<string[]>(c.Platforms ?? "[]"),
        //         VerticalIds = c.Verticals.Select(v => v.Id).ToArray(),
        //         VerticalNames = c.Verticals.Select(v => v.Name).ToArray(),
        //         Countries = JsonSerializer.Deserialize<string[]>(c.Countries ?? "[]")
        //     }));
        // }

        // POST: api/Campaigns
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
                Description = request.Description,
                Budget = request.Budget,
                DailyBudget = request.DailyBudget,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Platforms = request.Platforms.ToList(),
                Countries = request.Countries.ToList(),
                Verticals = verticals,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            campaign.Validate();

            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCampaign), new { id = campaign.Id }, new CampaignResponse
            {
                Id = campaign.Id,
                Name = campaign.Name,
                AdvertiserId = campaign.AdvertiserId,
                Description = campaign.Description,
                IsActive = campaign.IsActive,
                Budget = campaign.Budget,
                DailyBudget = campaign.DailyBudget,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                CreatedAt = campaign.CreatedAt,
                UpdatedAt = campaign.UpdatedAt,
                Platforms = request.Platforms,
                Verticals = verticals.Select(v => v.Id).ToArray(),
                VerticalNames = verticals.Select(v => v.Name).ToArray(),
                Countries = request.Countries
            });
        }

        // PUT: api/Campaigns/5
        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateCampaign(int id, UpdateCampaignRequest request)
        // {
        //     var campaign = await _context.Campaigns
        //         .Include(c => c.Verticals)
        //         .FirstOrDefaultAsync(c => c.Id == id);

        //     if (campaign == null)
        //     {
        //         return NotFound();
        //     }

        //     if (request.Name != null) campaign.Name = request.Name;
        //     if (request.Description != null) campaign.Description = request.Description;
        //     if (request.IsActive.HasValue) campaign.IsActive = request.IsActive.Value;
        //     if (request.Budget.HasValue) campaign.Budget = request.Budget.Value;
        //     if (request.DailyBudget.HasValue) campaign.DailyBudget = request.DailyBudget.Value;
        //     if (request.StartDate.HasValue) campaign.StartDate = request.StartDate.Value;
        //     if (request.EndDate.HasValue) campaign.EndDate = request.EndDate.Value;

        //     if (request.Platforms != null)
        //     {
        //         if (!Campaign.AreValidPlatforms(request.Platforms))
        //         {
        //             return BadRequest("Invalid platform values. Must be one or more of: Mobile, Desktop, Tablet");
        //         }
        //         campaign.Platforms = JsonSerializer.Serialize(request.Platforms);
        //     }

        //     if (request.Countries != null)
        //     {
        //         campaign.Countries = JsonSerializer.Serialize(request.Countries);
        //     }

        //     if (request.VerticalIds != null)
        //     {
        //         var verticals = await _context.Verticals
        //             .Where(v => request.VerticalIds.Contains(v.Id))
        //             .ToListAsync();

        //         if (verticals.Count != request.VerticalIds.Length)
        //         {
        //             return BadRequest("One or more vertical IDs are invalid");
        //         }

        //         campaign.Verticals = verticals;
        //     }

        //     campaign.UpdatedAt = DateTime.UtcNow;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!_context.Campaigns.Any(c => c.Id == id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

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