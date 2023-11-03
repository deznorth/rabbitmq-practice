using Microsoft.AspNetCore.Mvc;

namespace RMQ_App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagingController : ControllerBase
    {
        private readonly ILogger<MessagingController> _logger;
        private readonly RabbitClient _rabbitClient;

        public MessagingController(ILogger<MessagingController> logger, RabbitClient rabbitClient)
        {
            _logger = logger;
            _rabbitClient = rabbitClient;
        }

        [HttpGet()]
        public IActionResult Get([FromQuery] string message)
        {
            _rabbitClient.SendMessage(message);
            return Ok("Hello, World!");
        }
    }
}