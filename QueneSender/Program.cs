using DBLibrary.EF;
using DBLibrary.Entity;
using RabbitMQ.Client;
using System;
using System.Configuration;
using System.Threading;

namespace QueueSender
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var queueName = ConfigurationManager.AppSettings["queue_name"];
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    Console.WriteLine("Queue was initialized");

                    while (true)
                    {
                        var sender = new Sender(new EFGenericRepository<Task>());
                        sender.Send(channel, queueName);
                        Thread.Sleep(10000);
                    }
                }
            }
        }
    }
}