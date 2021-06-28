using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectManager.ApplicationCore.Configuration;
using ProjectManager.Data;
using ProjectManager.ApplicationCore.Services;
using ProjectManager.Middlewares;
using System.Text.Json;
using ProjectManager.Common;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ProjectManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            services.AddDbContext<ApiDbContext>(options => {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IProjectService, ProjectService>();

            services.AddScoped<IUserService, UserService>();

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // .AddJwtBearer(options =>
            // {
            //     var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);
            //     options.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuerSigningKey = true,
            //         IssuerSigningKey = new SymmetricSecurityKey(key),
            //         ValidateIssuer = false,
            //         ValidateAudience = false,
            //         ValidateLifetime = true,
            //         RequireExpirationTime = false
            //     };
            // });
            services.AddControllers();

            services.AddMvc()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectManager", Version = "v1" });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
