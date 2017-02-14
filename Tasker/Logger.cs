using NLog;

namespace Tasker
{
    public class Log
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Trace(string message)
        {
            _logger.Trace(message);
        }

        public static void Info(string message)
        {
            _logger.Info(message);
        }
    }
}
