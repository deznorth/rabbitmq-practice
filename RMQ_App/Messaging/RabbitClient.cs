using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace RMQ_App.Messaging
{
    public class RabbitClient : IMessagingClient
    {
        private readonly ILogger<RabbitClient> _logger;
        private readonly IModel _rabbitChannel;

        public RabbitClient(ILogger<RabbitClient> logger, IConfiguration config)
        {
            _logger = logger;

            var factory = new ConnectionFactory { Uri = new Uri(config.GetConnectionString("RMQUri")) };
            var connection = factory.CreateConnection();
            _rabbitChannel = connection.CreateModel();

            _rabbitChannel.QueueDeclare(
                queue: "task_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _rabbitChannel.CreateBasicProperties();
            properties.Persistent = true;

            _rabbitChannel.BasicPublish(
                exchange: string.Empty,
                routingKey: "task_queue",
                basicProperties: properties,
                body: body
            );

            _logger.LogInformation("Sent message: {message}", message);
        }

        public void SendMessage<T>(T payload)
        {
            var jsonString = JsonSerializer.Serialize(payload);
            var body = Encoding.UTF8.GetBytes(jsonString);

            var properties = _rabbitChannel.CreateBasicProperties();
            properties.Persistent = true;

            _rabbitChannel.BasicPublish(
                exchange: string.Empty,
                routingKey: "task_queue",
                basicProperties: properties,
                body: body
            );

            _logger.LogInformation("Sent message: {message}", jsonString);
        }
    }
}
