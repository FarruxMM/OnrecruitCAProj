using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace Online_recruitment_system.Models.Entities
{
    public class Message :BaseEntity
    {
        public List<MailboxAddress> To { get; set; }
        //public MailboxAddress To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> to, string subject, string content)
        //public Message(string to,string subject,string content)
        {
            //To = new MailboxAddress();
            To = new List<MailboxAddress>();

            //To.AddRange(to.Select(x => new MailboxAddress(x,"mr_javid95@mail.ru")));
            To.AddRange(to.Select(x => new MailboxAddress(x,"mr_javid95@mail.ru")));
            
          
            Subject = subject;
            Content = content;
        }
    }
}
