using Newtonsoft.Json;
using Tasker.DB;
using Tasker.Jobs;
using Task = Tasker.DB.Task;

namespace Tasker
{
    public class JobFactory
    {
        public static Job CreateJob(Task task)
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

            job.TaskId = task.Id;
            return job;
        }
    }
}