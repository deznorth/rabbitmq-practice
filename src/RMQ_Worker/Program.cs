using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using Common.Jobs;
using RMQ_Worker.handlers;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace RMQ_Worker
{
    internal class Program
    {
        static Dictionary<JobType, dynamic> jobHandlerMappings = new Dictionary<JobType, dynamic>
        {
            { JobType.Upvotes, new UpvotesJobHandler() },
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var factory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@rabbitmq:5672/") };
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
            consumer.Received += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var jsonString = Encoding.UTF8.GetString(body);
                var job = JsonConvert.DeserializeObject<Job>(jsonString);
                var jobHanlder = jobHandlerMappings.GetValueOrDefault(JobType.Upvotes);

                //switch (job?.JobType)
                //{
                //    case JobType.Upvotes:
                //        await jobHandlerMappings[JobType.Upvotes].DoWork(job);
                //        break;
                //    default:
                //        throw new NotImplementedException($"No handler implemented for job type: {job?.JobType}");
                //}

                await jobHanlder.DoWork(job);

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