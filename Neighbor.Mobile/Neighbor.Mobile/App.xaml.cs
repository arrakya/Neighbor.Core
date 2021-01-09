using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Core.Application;
using Neighbor.Core.Domain.Interfaces.Finance;
using Neighbor.Core.Domain.Interfaces.Security;
using Neighbor.Mobile.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using MediatR;
using System.Net.Http;

namespace Neighbor.Mobile
{
    public partial class App : Xamarin.Forms.Application
    {
#if DEBUG
        public readonly string ServerAddress = "10.0.2.2";
#else
        public readonly string ServerAddress = "arrakya.thddns.net:4431";
#endif

        public readonly string IdentityBaseAddress;
        public readonly string FinanceBaseAddress;

        public App()
        {
            IdentityBaseAddress = $"https://{ServerAddress}";
            FinanceBaseAddress = $"https://{ServerAddress}";

            InitializeComponent();

            var services = new ServiceCollection();
            DependencyService.Register<MockDataStore>();
            
            services.AddMediatR(typeof(ApplicationStartup).Assembly);

            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                }
            };
            services.AddHttpClient("finance", (httpClient) => httpClient.BaseAddress = new Uri(FinanceBaseAddress)).ConfigurePrimaryHttpMessageHandler(() => httpClientHandler);
            services.AddHttpClient("identity", (httpClient) => httpClient.BaseAddress = new Uri(IdentityBaseAddress)).ConfigurePrimaryHttpMessageHandler(() => httpClientHandler);

            services.AddTransient(typeof(IUserContextProvider), typeof(UserContextProvider));
            services.AddTransient<IFinance, FinanceService>();
            services.AddTransient(typeof(ITokenAccessor), typeof(ClientTokenAccessor));
            services.AddTransient<ITokenProvider, ClientTokenProvider>();

            var serviceProvider = services.BuildServiceProvider();            
            DependencyResolver.ResolveUsing(type => services.Any(p => p.ServiceType == type) ? serviceProvider.GetService(type) : null);

            MainPage = new AppShell();            
        }

        protected override void OnStart()
        {
            AppCenter.Start("27f68fc7-587a-48b6-aa5f-48fcdc59e28c", typeof(Analytics), typeof(Crashes));
        }        

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
