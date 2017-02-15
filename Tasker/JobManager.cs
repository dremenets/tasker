using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Tasker.DB;
using Tasker.Jobs;

namespace Tasker
{
    public class JobManager
    {
        private readonly GenericRepository<Task> _repository;
        private readonly Dictionary<int, Timer> _timers;

        public JobManager()
        {
            _repository = new GenericRepository<Task>(new TaskerContext());
            _timers = new Dictionary<int, Timer>();
        }

        public void InitTasks()
        {
            var tasks = _repository.Get(x => x.ExpectedStart > DateTime.Now && x.Status == Status.None);
            foreach (var task in tasks)
            {
                var job = JobFactory.CreateJob(task);
                task.Status = Status.Scheduled;
                _repository.Update(task);

                Log.Info($"Task with TaskId: {job.TaskId} is scheduled!");

                var interval = task.ExpectedStart.Subtract(DateTime.Now);
                var timer = new Timer(interval.TotalMilliseconds) {AutoReset = false, Enabled = true};
                timer.Elapsed += (sender, e) => RunJob(job);
                _timers[task.Id] = timer;
            }

        }

        public List<int> GetActiveTaskIds()
        {
            return _timers.Keys.ToList();
        }

        private void RunJob(Job job)
        {
            Status status;
            if (job.Run())
            {
                status = Status.Completed;
                Log.Info($"Task with TaskId: {job.TaskId} is completed!");
            }
            else
            {
                status = Status.Failed;
                Log.Info($"Task with TaskId: {job.TaskId} is failed!");
            }

            var task = _repository.FindById(job.TaskId);
            task.Status = status;
            _repository.Update(task);

            _timers.Remove(job.TaskId);
        }
    }
}