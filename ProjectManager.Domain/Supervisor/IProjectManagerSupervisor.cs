using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;

namespace ProjectManager.Domain.Supervisor
{
    public interface IProjectManagerSupervisor
    {
        public IAuthenticationRepository AuthenticationRepository { get; set; }
        public IProjectRepository ProjectRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IMailerService MailerService { get; set; }
    }
}