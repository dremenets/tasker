using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Newtonsoft.Json;
using Tasker.DB;
using Tasker.Jobs;

namespace Tasker
{
    public class JobManager
    {
        private readonly List<Timer> _timers;

        public JobManager()
        {
            _timers = new List<Timer>();
        }

        public void InitTasks()
        {
            using (TaskerContext db = new TaskerContext())
            {
                var tasks = db.Tasks.Where(x => x.ExpectedStart < DateTime.Now
                                                && (x.Status != "Scheduled" || x.Status != "Completed"));
                foreach (var task in tasks)
                {
                    var job = CreateJob(task);
                    task.Status = "Scheduled";
                    var interval = task.ExpectedStart.Subtract(DateTime.Now);
                    var timer = new Timer(interval.TotalMilliseconds);
                    timer.Elapsed += (sender, e) => RunJob(job);
                    _timers.Add(timer);
                }

                db.SaveChanges();
            }
        }

        private void RunJob(Job job)
        {
            job.Run();
            using (TaskerContext db = new TaskerContext())
            {
                var task = db.Tasks.FirstOrDefault(x => x.Id == job.TaskId);
                if (task != null)
                {
                    task.Status = "Completed";
                }

                db.SaveChanges();
            }
        }

        private Job CreateJob(Task task)
        {
            Job job;
            switch (task.Type)
            {
                case "File":
                {
                    job = new FileJob(task.Params) {TaskId = task.Id};
                    break;
                }
                default:
                {
                    var emailSettings = JsonConvert.DeserializeObject<EmailSettings>(task.Params);
                    job = new EmailJob(emailSettings.EmailAddress, emailSettings.Subject, emailSettings.Message)
                    {
                        TaskId = task.Id
                    };
                    break;
                }
            }

            return job;
        }
    }
}
