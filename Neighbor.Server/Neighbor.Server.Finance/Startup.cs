using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using Neighbor.Core.Application;
using Neighbor.Core.Infrastructure.Server;
using Neighbor.Server.Finance.MonthlyBalance.Data;
using Microsoft.EntityFrameworkCore;

namespace Neighbor.Server.Finance.MonthlyBalance
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ApplicationStartup.ConfigureBuilder(services);
            services.AddMediatR(new[] { typeof(ApplicationStartup).Assembly, typeof(Startup).Assembly });

            var defaultConnection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MonthlyBalanceDbContext>(options =>
            {
                options.UseSqlServer(defaultConnection);
            });

            services.AddNeighborInfrastructureDbContext<MonthlyBalanceDbContext>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                using var scope = app.ApplicationServices.CreateScope();
                var dbContext = (MonthlyBalanceDbContext)scope.ServiceProvider.GetRequiredService(typeof(MonthlyBalanceDbContext));
                dbContext.Database.Migrate();
                dbContext.Database.EnsureCreated();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
