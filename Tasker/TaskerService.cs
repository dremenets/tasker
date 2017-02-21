using DBLibrary;
using DBLibrary.Entity;
using DBLibrary.Entity.Enums;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Timers;

namespace Tasker
{
    public class TaskerService
    {
        private Timer _timer;
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
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "tasker_queue",
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

                channel.BasicConsume(queue: "tasker_queue",
                                     noAck: false,
                                     consumer: consumer);

                while (true) { }
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