using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Tasker.Jobs
{
    public class EmailJob: Job
    {
        private string _message;
        private string _subject;
        private string _emailAddress;

        public EmailJob(string emailAddress, string subject, string message)
        {
            _message = message;
            _subject = subject;
            _emailAddress = emailAddress;
        }

        public string Message {
            get { return _message; }
            set { _message = value; }
        }

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        public override Task Run()
        {
            var smtp = ConfigurationManager.AppSettings["smtp"];
            var login = ConfigurationManager.AppSettings["login"];
            var password = ConfigurationManager.AppSettings["password"];

            throw new NotImplementedException();
        }
    }
}
