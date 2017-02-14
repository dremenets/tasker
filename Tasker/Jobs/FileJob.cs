using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Tasker.Jobs
{
    public class FileJob : Job
    {
        private readonly string _fileName;

        public FileJob(string fileName)
        {
            _fileName = fileName;
        }

        public override void Run()
        {
            // задержка в 10сек.
            Task.Delay(10000)
                .ContinueWith(t =>
                {
                    var path = ConfigurationManager.AppSettings["path"];
                    var fullPath = Path.Combine(path, _fileName);
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }

                    using (File.Create(fullPath))
                    {
                    }

                });
        }
    }
}