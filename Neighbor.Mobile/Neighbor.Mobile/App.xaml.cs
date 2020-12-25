using MediatR;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Core.Application;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Infrastructure.Client;
using Neighbor.Mobile.Services;
using Neighbor.Mobile.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Neighbor.Mobile
{
    public partial class App : Xamarin.Forms.Application
    {
        //public readonly string ServerAddress = "10.0.2.2";
        public readonly string ServerAddress = "arrakya.thddns.net:4431";

        public readonly string IdentityBaseAddress;
        public readonly string FinanceBaseAddress;

        public App()
        {
#if !DEBUG
            ServerAddress = "arrakya.thddns.net";
#endif
            IdentityBaseAddress = $"https://{ServerAddress}";
            FinanceBaseAddress = $"https://{ServerAddress}";

            InitializeComponent();

            var services = new ServiceCollection();
            DependencyService.Register<MockDataStore>();

            // Configure services
            var applicationAssembly = typeof(ApplicationStartup).Assembly;
            ApplicationStartup.ClientConfigureBuilder(services,
                (httpClient) =>
                {
                    httpClient.BaseAddress = new Uri(FinanceBaseAddress);
                },
                (httpClient) =>
                {
                    httpClient.BaseAddress = new Uri(IdentityBaseAddress);
                },
                (clientServices) =>
                {
                    var clientTokenProvider = new ClientTokenProvider(clientServices)
                    {
                        GetCurrentRefreshToken = () =>
                        {
                            Application.Current.Properties.TryGetValue("refresh_token", out var refreshToken);

                            return refreshToken?.ToString() ?? string.Empty;
                        },
                        GetCurrentAccessToken = () =>
                        {
                            Application.Current.Properties.TryGetValue("access_token", out var accessToken);

                            return accessToken?.ToString() ?? string.Empty;
                        },
                        SetCurrentAccessToken = (token) =>
                        {
                            if (Application.Current.Properties.ContainsKey("access_token"))
                            {
                                Application.Current.Properties.Remove("access_token");
                            }

                            Application.Current.Properties.Add("access_token", token);
                        },
                        GetCertificate = () =>
                        {
                            var assetProvider = DependencyService.Resolve<IAssetsProvider>();
                            var certBytes = assetProvider.Get<byte[]>("arrakya.thddns.net.crt");

                            return certBytes;
                        }
                    };

                    return clientTokenProvider;
                });

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
