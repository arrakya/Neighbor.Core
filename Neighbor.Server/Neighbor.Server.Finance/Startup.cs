using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using Neighbor.Core.Application;
using Neighbor.Server.Finance.MonthlyBalance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using System;
using Neighbor.Core.Domain.Interfaces.Finance;

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
            ApplicationStartup.ServerConfigureBuilder(services);
            services.AddMediatR(new[] { typeof(ApplicationStartup).Assembly, typeof(Startup).Assembly });

            var defaultConnection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MonthlyBalanceDbContext>(options =>
            {
                options.UseSqlServer(defaultConnection);
            });
            services.AddAuthentication(config =>
            {
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var x509CertificateFilePath = Configuration.GetSection("Security:CertificatePath").Value;
                    var x509Certfificate = new X509Certificate2(x509CertificateFilePath);
                    var x509SecurityKey = new X509SecurityKey(x509Certfificate);

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        IssuerSigningKey = x509SecurityKey,
                        ValidateLifetime = true,
                        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                        {
                            var isValidLifeTime = expires > DateTime.UtcNow;

                            return isValidLifeTime;
                        },
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });            
            services.AddTransient<IFinance, FinanceProvider>();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
