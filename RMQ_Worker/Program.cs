using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using RMQ_App.Models;

namespace RMQ_Worker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var factory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@localhost:5672/") };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "task_queue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var jsonString = Encoding.UTF8.GetString(body);
                var task = JsonConvert.DeserializeObject<UpvotesTask>(jsonString);
                Console.WriteLine($" [x] Received upvotes request. Giving {task?.NumOfVotes} votes to postId: {task?.PostId}.");


                for(int i = 0; i < task?.NumOfVotes; i++)
                {
                    Thread.Sleep(500);
                    Console.WriteLine($"[postId: {task?.PostId}] votes completed: {i}");
                }

                Console.WriteLine(" [x] Done");

                // here channel could also be accessed as ((EventingBasicConsumer)sender).Model
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue: "task_queue",
                                 autoAck: false,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}