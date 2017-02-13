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
                var tasks = db.Tasks;
                foreach (Task t in tasks)
                {
                    
                }
            }
        }

        public void Stop()
        {

        }
    }
}
