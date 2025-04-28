using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using AdTechAPI.Services;


namespace AdTechAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PostbackController : ControllerBase
    {

        private readonly AppDbContext _db;
        private readonly ILogger<PostbackController> _logger;
        private readonly IBackgroundJobClient _backgroundJobs;

        public PostbackController(AppDbContext db, ILogger<PostbackController> logger, IBackgroundJobClient backgroundJobs)
        {
            _db = db;
            _logger = logger;
            _backgroundJobs = backgroundJobs;

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string cid, [FromQuery] decimal revenue)
        {
            // uuid is actually a param passed as /postback?cid={here}&revenue={decimal}
            // uuid;
            // revenue;
            Guid uuid = Guid.Parse(cid);
            if (string.IsNullOrEmpty(cid))
            {
                return BadRequest("Missing cid");
            }
            var click = await _db.Clicks.FirstOrDefaultAsync(c => c.Uuid == uuid);

            if (click == null)
            {
                return NotFound("Click not found");
            }

            _logger.LogInformation("Postback received: cid={cid}, revenue={revenue}", cid, revenue);


            _backgroundJobs.Enqueue<ClickService>(svc => svc.UpdateClickRevenue(uuid, revenue));

            return Accepted();
        }
    }
}
