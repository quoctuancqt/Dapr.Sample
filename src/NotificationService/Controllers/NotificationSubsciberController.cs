using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationSubsciberController : ControllerBase
    {
        private readonly ILogger<NotificationSubsciberController> _logger;

        public NotificationSubsciberController(ILogger<NotificationSubsciberController> logger)
        {
            _logger = logger;
        }

        [Topic("order-sub", "order")]
        [HttpGet]
        public async Task<IActionResult> Notify([FromBody] dynamic request)
        {
            _logger.LogInformation($"Received Product - {request.Id}");

            await Task.CompletedTask;

            return Ok();
        }
    }
}
