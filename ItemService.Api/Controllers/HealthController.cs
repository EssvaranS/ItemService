using ItemService.Api.Responses;
using ItemService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemService.Api.Controllers
{
    [ApiController]
    [Route("/health")]
    public class HealthController : ControllerBase
    {
        private readonly IMongoDbContext _dbContext;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly DateTime _start = DateTime.UtcNow;

        public HealthController(IMongoDbContext dbContext, IHostApplicationLifetime lifetime)
        {
            _dbContext = dbContext;
            _lifetime = lifetime;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var dbStatus = await _dbContext.PingAsync() ? "Connected" : "Disconnected";
            var uptime = DateTime.UtcNow - _start;
            var payload = new { status = "UP", dbStatus, uptime = $"{(int)uptime.TotalSeconds}s" };
            return Ok(ApiResponse<object>.Ok(payload));
        }
    }
}
