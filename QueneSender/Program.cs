using DBLibrary.EF;
using DBLibrary.Entity;
using RabbitMQ.Client;
using System;
using System.Threading;

namespace QueueSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "tasker_queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    Console.WriteLine("Queue was initialized");

                    while (true)
                    {
                        var sender = new Sender(new EFGenericRepository<Task>());
                        sender.Send(channel);
                        Thread.Sleep(10000);
                    }
                }
            }
        }
    }
}