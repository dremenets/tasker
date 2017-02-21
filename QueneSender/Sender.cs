using DBLibrary;
using DBLibrary.Entity;
using DBLibrary.Entity.Enums;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace QueueSender
{
    public class Sender
    {
        private readonly IGenericRepository<Task> _repository;

        public Sender(IGenericRepository<Task> repository)
        {
            _repository = repository;
        }

        public void Send(IModel channel)
        {
            var tasks = _repository.Get(x => x.ExpectedStart > DateTime.Now && x.Status == Status.None);
            Console.WriteLine($"Published tasks {tasks.ToList().Count}");
            foreach (var task in tasks)
            {
                var jsonTask = JsonConvert.SerializeObject(task);
                var body = Encoding.UTF8.GetBytes(jsonTask);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                task.Status = Status.Published;
                _repository.Update(task);

                channel.BasicPublish(exchange: "",
                                     routingKey: "tasker_queue",
                                     basicProperties: properties,
                                     body: body);
            }
        }
    }
}