using Common.Jobs;
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
            var exportJob = new ExportJob(request.FileName, request.FileId);
            _rabbitClient.SendMessage(exportJob);
            return Ok(exportJob);
        }

        [HttpPost("upvotes")]
        public IActionResult RequestUpvotes([FromBody] UpvotesRequest request)
        {
            var upvotesJob = new UpvotesJob(request.PostId, request.NumOfVotes);
            _rabbitClient.SendMessage(upvotesJob);
            return Ok(upvotesJob);
        }
    }
}