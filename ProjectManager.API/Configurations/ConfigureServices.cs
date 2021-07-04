using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectManager.Data.Repositories;
using ProjectManager.Data.Services;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using ProjectManager.Domain.Supervisor;

namespace ProjectManager.API.Configurations
{
    public static class ServicesConfiguration
    {
        public static void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IProjectRepository, ProjectRepository>();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMailerService, MailerService>();
        }

        public static void ConfigureSupervisor(IServiceCollection services)
        {
            services.AddScoped<ProjectManagerSupervisor>();
        }
        
        public static void AddCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }
    }
}