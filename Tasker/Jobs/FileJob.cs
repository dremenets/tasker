using System;
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

        public override bool Run()
        {
            // задержка в 10сек.
            var task = Task.Delay(10000)
                .ContinueWith(t =>
                {
                    var path = ConfigurationManager.AppSettings["path"];
                    var fullPath = Path.Combine(path, _fileName);
                    try
                    {
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }

                        using (File.Create(fullPath))
                        {
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Trace(e.StackTrace);
                        return false;
                    }

                    return true;
                });

            return task.Result;
        }
    }
}