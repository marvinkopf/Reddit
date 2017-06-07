using System.Threading.Tasks;

namespace Reddit.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
