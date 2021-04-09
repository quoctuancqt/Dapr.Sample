using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Events;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationSubsciberController : ControllerBase
    {
        private const string DaprPubSubName = "pubsub";

        private readonly ILogger<NotificationSubsciberController> _logger;

        public NotificationSubsciberController(ILogger<NotificationSubsciberController> logger)
        {
            _logger = logger;
        }

        [Topic(DaprPubSubName, "OrderCompletedIntegrationEvent")]
        [HttpPost]
        public async Task Notify(OrderCompletedIntegrationEvent request)
        {
            _logger.LogInformation($"Order detail: {JsonSerializer.Serialize(request)}");

            await Task.CompletedTask;
        }
    }
}
