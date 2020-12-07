﻿using MediatR;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;

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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Basic", policy =>
                {
                    policy.RequireClaim("client_id");
                    policy.RequireClaim("client_secret");
                });
            });
            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var key = Environment.GetEnvironmentVariable("NEIGHBOR_IDENTITY_KEY");
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(key));

                    var x509CertificateFilePath = @"C:\Users\arrak\source\repos\Neighbor\Neighbor.Server\Neighbor.Server.Identity\arrakya.thddns.net.crt";
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
                })
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
            
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
