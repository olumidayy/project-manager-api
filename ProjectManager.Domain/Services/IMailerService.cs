using System.Threading.Tasks;

namespace ProjectManager.Domain.Services
{
    public interface IMailerService
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}