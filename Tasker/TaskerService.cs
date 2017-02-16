using System.Timers;
using Tasker.DB;
using Tasker.DB.EF;

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

            _jobManager.Init();

            _timer = new Timer
            {
                AutoReset = true,
                Enabled = true,
                Interval = 5000
            };

            _timer.Elapsed += (o, e) => _jobManager.Init();
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