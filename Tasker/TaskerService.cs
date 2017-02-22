using System.Configuration;
using DBLibrary;
using DBLibrary.Entity;
using DBLibrary.Entity.Enums;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Tasker
{
    public class TaskerService
    {
        private readonly IGenericRepository<Task> _repository;
        private readonly IJobControl _jobManager;

        public TaskerService(IJobControl jobManager, IGenericRepository<Task> repository)
        {
            _repository = repository;
            _jobManager = jobManager;
        }

        public void Start()
        {
            Log.Info("Tasker Service is started!");
            var queueHost = ConfigurationManager.AppSettings["queue_host"];
            var factory = new ConnectionFactory() {HostName = queueHost};
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueName = ConfigurationManager.AppSettings["queue_name"];
                channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    _jobManager.Init(JsonConvert.DeserializeObject<Task>(message));
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: queueName,
                    noAck: false,
                    consumer: consumer);

                while (true)
                {
                }
            }
        }

        public void Stop()
        {
            var taskIds = _jobManager.GetScheduledTaskIds();
            foreach (var id in taskIds)
            {
                var task = _repository.FindById(id);
                task.Status = Status.None;
                _repository.Update(task);
            }
            Log.Info("Tasker Service is stopped!");
        }
    }
}