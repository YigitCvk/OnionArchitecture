using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OAController> _logger;

        public OAController(IMediator mediator, ILogger<OAController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        #region HealthCheck
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            var isHealthy = true;

            if (isHealthy)
            {
                return Ok(new { Status = "Healthy" });
            }
            else
            {
                return StatusCode(500, new { Status = "Unhealthy" });
            }
        }
        #endregion
    }
}
