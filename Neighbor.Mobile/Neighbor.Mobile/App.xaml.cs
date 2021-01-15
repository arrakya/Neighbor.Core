using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Mobile.Services;
using Neighbor.Mobile.ViewModels.Base;
using System;
using System.Linq;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Neighbor.Mobile
{
    public partial class App : Xamarin.Forms.Application
    {
        public static string ReleaseVersion
        {
            get => Preferences.Get("ReleaseVersion", "SIT");
            set
            {
                Preferences.Set("ReleaseVersion", value);
                switch (value)
                {
                    case "SIT":
                        ServerAddress = "arrakya.thddns.net:4431";
                        break;
                    case "Production":
                        ServerAddress = "arrakya.thddns.net:443";
                        break;
                }
            }
        }

        public static string RefreshToken
        {
            get => Preferences.Get("RefreshToken", string.Empty);
            set
            {
                Preferences.Set("RefreshToken", value);
            }
        }

        public static string AccessToken
        {
            get => Preferences.Get("AccessToken", string.Empty);
            set
            {
                Preferences.Set("AccessToken", value);
            }
        }

        public static string ServerAddress
        {
            get => Preferences.Get("ServerAddress", string.Empty);
            set
            {
                Preferences.Set("ServerAddress", value);
            }
        }

        public static string IdentityBaseAddress
        {
            get
            {
                var identityBaseAddress = $"https://{ServerAddress}:6001";
                switch (ReleaseVersion)
                {
                    case "SIT":
                        identityBaseAddress = $"https://{ServerAddress}/neighbor/identity/";
                        break;
                    case "Production":
                        identityBaseAddress = $"https://{ServerAddress}/neighbor/identity/";
                        break;
                }
                return identityBaseAddress;
            }
        }

        public static string FinanceBaseAddress
        {
            get
            {
                var financeBaseAddress = $"https://{ServerAddress}:5001";
                switch (ReleaseVersion)
                {
                    case "SIT":
                        financeBaseAddress = $"https://{ServerAddress}/neighbor/finance/";
                        break;
                    case "Production":
                        financeBaseAddress = $"https://{ServerAddress}/neighbor/finance/";
                        break;
                }
                return financeBaseAddress;
            }           
        }

        public App()
        {
            ServerAddress = "10.0.2.2";

            switch (ReleaseVersion)
            {
                case "SIT":
                    ServerAddress = "arrakya.thddns.net:4431";
                    break;
                case "Production":
                    ServerAddress = "arrakya.thddns.net:443";
                    break;
            }

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
                Preferences.Remove("RefreshToken");
                Preferences.Remove("AccessToken");

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
