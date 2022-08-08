using System.Threading.Tasks;

namespace Online_recruitment_system.Models.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email,string subject,string message);
    }
}
