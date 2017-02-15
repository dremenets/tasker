using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Tasker.Jobs
{
    public class EmailJob : Job
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

        public string Message
        {
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

        public override bool Run()
        {
            var smtp = ConfigurationManager.AppSettings["smtp"];
            int smtpPort = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["smtp_port"], out smtpPort);
            var password = ConfigurationManager.AppSettings["password"];
            var login = ConfigurationManager.AppSettings["login"];

            MailAddress from = new MailAddress(login);
            MailAddress to = new MailAddress(EmailAddress);
            MailMessage m = new MailMessage(from, to)
            {
                Subject = Subject,
                Body = Message
            };
            SmtpClient smtpClient = new SmtpClient(smtp, smtpPort)
            {
                Credentials = new NetworkCredential(login, password),
                EnableSsl = true
            };

            try
            {
                smtpClient.Send(m);
            }
            catch (Exception e)
            {
                Log.Trace(e.StackTrace);
                return false;
            }
            return true;
        }
    }
}