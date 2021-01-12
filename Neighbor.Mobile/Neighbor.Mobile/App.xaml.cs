using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Mobile.NativeHelpers;
using Neighbor.Mobile.Services;
using Neighbor.Mobile.ViewModels.Base;
using System;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

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
#if DEBUG
            IdentityBaseAddress = $"https://{ServerAddress}:6001";
            FinanceBaseAddress = $"https://{ServerAddress}:5001";
#else
            IdentityBaseAddress = $"https://{ServerAddress}/neighbor/identity";
            FinanceBaseAddress = $"https://{ServerAddress}/neighbor/finance";
#endif

            InitializeComponent();

            var services = new ServiceCollection();
            DependencyService.Register<MockDataStore>();

            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                }
            };
            services.AddHttpClient("finance", (httpClient) => httpClient.BaseAddress = new Uri(FinanceBaseAddress)).ConfigurePrimaryHttpMessageHandler(() => httpClientHandler);
            services.AddHttpClient("identity", (httpClient) => httpClient.BaseAddress = new Uri(IdentityBaseAddress)).ConfigurePrimaryHttpMessageHandler(() => httpClientHandler);
            
            var serviceProvider = services.BuildServiceProvider();            
            DependencyResolver.ResolveUsing(type => services.Any(p => p.ServiceType == type) ? serviceProvider.GetService(type) : null);

            MainPage = new AppShell();
            MessagingCenter.Subscribe<BaseViewModel>(this, "RefreshTokenExpired", async (viewModel) =>
            {
                Current.Properties.Remove("refresh_token");
                Current.Properties.Remove("access_token");

                await Shell.Current.GoToAsync("//LoginPage");
            });

            
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
