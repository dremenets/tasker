using System;
using System.Timers;

namespace Tasker
{
    public class TaskerService
    {
        private Timer _timer;

        public void Start()
        {
            var jobManager = new JobManager();
            _timer = new Timer(5000) {AutoReset = true};
            _timer.Elapsed += (o, e) => jobManager.InitTasks();
        }

        public void Stop()
        {
            Console.WriteLine("Tasker Service is closed!");
        }
    }
}