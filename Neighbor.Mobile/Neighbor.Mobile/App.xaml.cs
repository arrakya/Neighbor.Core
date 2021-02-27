using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Mobile.NativeHelpers;
using Neighbor.Mobile.Services;
using Neighbor.Mobile.Services.Net;
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
        public static string BranchVersion
        {
            get
            {
                var appVersionHelper = DependencyService.Resolve<IAppVersionHelper>(DependencyFetchTarget.NewInstance);

                return appVersionHelper.AppVersion.ToLower();
            }
        }

        public static string ReleaseVersion
        {
            get
            {
                var releaseVersion = Preferences.Get("ReleaseVersion", "Production");                
                return releaseVersion;
            }
            set
            {
                Preferences.Set("ReleaseVersion", value);                
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
            get
            {
                switch (ReleaseVersion)
                {
                    //case "SIT": return "arrakya.thddns.net:4431";
                    case "SIT": return "192.168.1.145:4431";
                    case "Production": return "arrakya.thddns.net:443";
                    default: return "10.0.2.2";
                };
            }
        }

        public static string IdentityBaseAddress
        {
            get
            {
                var identityBaseAddress = $"https://{ServerAddress}/neighbor/identity/"; 

                if(ReleaseVersion == "Development")
                {
                    identityBaseAddress = $"https://{ServerAddress}:6001";
                }

                return identityBaseAddress;
            }
        }

        public static string FinanceBaseAddress
        {
            get
            {
                var identityBaseAddress = $"https://{ServerAddress}/neighbor/finance/";

                if (ReleaseVersion == "Development")
                {
                    identityBaseAddress = $"https://{ServerAddress}:5001/";
                }

                return identityBaseAddress;
            }
        }

        public App()
        {
            if (BranchVersion.Contains("master"))
            {
                Preferences.Set("ReleaseVersion", "Production");
            }
            else
            {
                Preferences.Set("ReleaseVersion", "SIT");
            }

            InitializeComponent();

            var services = new ServiceCollection();
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<HttpClientService>();
            DependencyService.Register<UserContextService>();
            DependencyService.Register<PINService>();

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
            MessagingCenter.Subscribe<HttpClientService>(this, "RefreshTokenExpired", async (viewModel) =>
            {
                Preferences.Remove("RefreshToken");
                Preferences.Remove("AccessToken");

                await Shell.Current.GoToAsync("//LoginPage");
            });
        }

        protected async override void OnStart()
        {
            AppCenter.Start("27f68fc7-587a-48b6-aa5f-48fcdc59e28c", typeof(Analytics), typeof(Crashes));

            await ((AppShell)Shell.Current).UpdateFlyoutViewModel();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
