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
       
        private readonly string _smtpServer;
        private readonly string _password;
        private readonly string _login;

        private readonly List<Timer> _timers;

        public JobManager()
        {
            // Прочитать настройки

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
                    timer.Elapsed +=  (sender, e) => job.Run();
                    _timers.Add(timer);
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
                    job = new FileJob(task.Params);
                    break;
                }
                default:
                {
                    var emailSettings = JsonConvert.DeserializeObject<EmailSettings>(task.Params);
                    job = new EmailJob(emailSettings.EmailAddress, emailSettings.Subject, emailSettings.Message);
                    break;
                }
            }

            return job;
        }
    }
}
