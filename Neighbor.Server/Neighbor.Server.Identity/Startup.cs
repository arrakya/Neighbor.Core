using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Core.Application;
using Neighbor.Core.Infrastructure.Server;
using Neighbor.Server.Identity.Data;

namespace Neighbor.Server.Identity
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
            ApplicationStartup.ServerConfigureBuilder(services);
            services.AddMediatR(new[] { typeof(ApplicationStartup).Assembly, typeof(Startup).Assembly });            

            var defaultConnection = Configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(defaultConnection);
            });
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<IdentityDbContext>();

            services.AddTransient<IIdentityDbContext, IdentityDbContext>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                using var scope = app.ApplicationServices.CreateScope();
                var dbContext = (IdentityDbContext)scope.ServiceProvider.GetRequiredService(typeof(IdentityDbContext));
                dbContext.Database.Migrate();
                dbContext.Database.EnsureCreated();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
