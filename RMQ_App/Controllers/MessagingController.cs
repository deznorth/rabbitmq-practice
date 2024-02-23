using Microsoft.AspNetCore.Mvc;
using RMQ_App.Messaging;
using RMQ_App.Models;

namespace RMQ_App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagingController : ControllerBase
    {
        private readonly ILogger<MessagingController> _logger;
        private readonly IMessagingClient _rabbitClient;

        public MessagingController(ILogger<MessagingController> logger, IMessagingClient rabbitClient)
        {
            _logger = logger;
            _rabbitClient = rabbitClient;
        }

        [HttpPost("export")]
        public IActionResult RequestExport([FromBody] ExportRequest request)
        {
            var exportTask = new ExportTask(request.FileName, request.FileId);
            _rabbitClient.SendMessage(exportTask);
            return Ok(exportTask);
        }

        [HttpPost("upvotes")]
        public IActionResult RequestUpvotes([FromBody] UpvotesRequest request)
        {
            var upvotesTask = new UpvotesTask(request.PostId, request.NumOfVotes);
            _rabbitClient.SendMessage(upvotesTask);
            return Ok(upvotesTask);
        }
    }
}