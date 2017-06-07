using System.Threading.Tasks;

namespace Reddit.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
