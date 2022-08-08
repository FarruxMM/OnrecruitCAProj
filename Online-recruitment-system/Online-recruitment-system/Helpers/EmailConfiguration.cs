namespace Online_recruitment_system.Models.Entities
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public string UserName { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }    
    }

}
