using System;
using Tasker.DB;

namespace Tasker
{
    public class TaskerService
    {
        public void Start()
        {
            using (TaskerContext db = new TaskerContext())
            {
                
            }
        }

        public void Stop()
        {
            Console.WriteLine("Tasker Service is closed!");
        }
    }
}
