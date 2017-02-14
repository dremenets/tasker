using System;
using Tasker.DB;

namespace Tasker
{
    public class TaskerService
    {
        public void Start()
        {
            var jobManager = new JobManager();
            jobManager.InitTasks();
        }

        public void Stop()
        {
            Console.WriteLine("Tasker Service is closed!");
        }
    }
}
