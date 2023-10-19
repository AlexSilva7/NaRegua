using Microsoft.AspNetCore.Mvc;
using NaRegua_Api.Common.Contracts;
namespace NaRegua_Api.Controllers
{
    [Route("[controller]")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IHealthService _healthService;
        public HealthController(ILogger<HealthController> logger, IHealthService healthService)
        {
            _logger = logger;
            _healthService = healthService;
        }

        [HttpGet("/health")] // GET /health
        public async Task<IActionResult> GetHealthInfoAsync()
        {
            try
            {
                var result = _healthService.GetSystemInfo();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }
    }
}
