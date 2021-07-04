using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;

namespace ProjectManager.Domain.Supervisor
{
    public class ProjectManagerSupervisor
    {
        public IAuthenticationRepository AuthenticationRepository;
        public IProjectRepository ProjectRepository;
        public IUserRepository UserRepository;
        public IMailerService MailerService;

        public ProjectManagerSupervisor(
            IAuthenticationRepository authenticationRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IMailerService mailerService
        )
        {
            AuthenticationRepository = authenticationRepository;
            ProjectRepository = projectRepository;
            UserRepository = userRepository;
            MailerService = mailerService;
        }
    }
}