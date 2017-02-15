using System.Timers;
using Tasker.DB;

namespace Tasker
{
    public class TaskerService
    {
        private Timer _timer;
        private readonly GenericRepository<Task> _repository;
        private readonly JobManager _jobManager;

        public TaskerService()
        {
            _repository = new GenericRepository<Task>(new TaskerContext());
            _jobManager = new JobManager();
        }

        public void Start()
        {
            Log.Info("Tasker Service is started!");

            _jobManager.InitTasks();

            _timer = new Timer
            {
                AutoReset = true,
                Enabled = true,
                Interval = 5000
            };

            _timer.Elapsed += (o, e) => _jobManager.InitTasks();
        }

        public void Stop()
        {
            var taskIds = _jobManager.GetActiveTaskIds();
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