using RabbitMQ.Client;
using System;
using System.Text;

namespace NewTask
{
    public class NewTask
    {
        public static void Main(string[] args)
        {
            ConnectionFactory factory = new() { HostName = "localhost" };

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            _ = channel.QueueDeclare(queue: "task_queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string message = GetMessage(args);
            byte[] body = Encoding.UTF8.GetBytes(message);

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "",
                                 routingKey: "task_queue",
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine(" [x] Sent {0}", message);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return args.Length > 0 ? string.Join(" ", args) : "Hello World!";
        }
    }
}