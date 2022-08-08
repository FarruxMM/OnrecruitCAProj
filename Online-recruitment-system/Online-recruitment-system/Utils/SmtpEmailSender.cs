using Online_recruitment_system.Models.Interface;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Online_recruitment_system.Utils
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly int _port;
        private readonly bool _enableSSL;
        private readonly string _username;
        private readonly string _password;
        private readonly string _host;

        public SmtpEmailSender(string host, int port, bool enableSSl, string username, string password)
        {
            _host = host;
            _port = port;
            _enableSSL = enableSSl;
            _username = username;
            _password = password;

        }


        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient()
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = _enableSSL,
                Host = _host,
                Port = _port
            };
            return client.SendMailAsync(
                new MailMessage(_username, email, subject, message)
                {
                    IsBodyHtml = true
                });

        }
    
    }
}
