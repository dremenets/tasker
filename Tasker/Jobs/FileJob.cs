using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Tasker.Jobs
{
    public class FileJob: Job
    {
        private readonly string _fileName;

        public FileJob(string fileName)
        {
            _fileName = fileName;
        }

        public override void Run()
        {
            var path = ConfigurationManager.AppSettings["path"];

            throw new NotImplementedException();
        }
    }
}
