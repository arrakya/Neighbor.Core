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
        public readonly string ServerAddress = "10.0.2.2";

        public readonly string IdentityBaseAddress;
        public readonly string FinanceBaseAddress;

        public App()
        {
#if !DEBUG
            ServerAddress = "arrakya.thddns.net";
#endif
            IdentityBaseAddress = $"http://{ServerAddress}";
            FinanceBaseAddress = $"http://{ServerAddress}";

            InitializeComponent();

            var services = new ServiceCollection();
            DependencyService.Register<MockDataStore>();

            // Configure services
            var applicationAssembly = typeof(ApplicationStartup).Assembly;
            ApplicationStartup.ClientConfigureBuilder(services,
                (httpClient) =>
                {
                    httpClient.BaseAddress = new Uri(FinanceBaseAddress);
#if DEBUG
                    httpClient.BaseAddress = new Uri("http://10.0.2.2:5000");
#endif
                },
                (httpClient) =>
                {
                    httpClient.BaseAddress = new Uri(IdentityBaseAddress);
#if DEBUG
                    httpClient.BaseAddress = new Uri("http://10.0.2.2:6000");
#endif
                },
                (clientServices) =>
                {
                    var clientTokenProvider = new ClientTokenProvider(clientServices)
                    {
                        GetCurrentRefreshToken = () =>
                        {
                            Application.Current.Properties.TryGetValue("token", out var refreshToken);

                            return refreshToken?.ToString() ?? string.Empty;
                        },
                        GetCertificate = () =>
                        {
                            var assetProvider = DependencyService.Resolve<IAssetsProvider>();
                            var certBytes = assetProvider.Get<byte[]>("");

                            return certBytes;
                        }
                    };

                    return clientTokenProvider;
                });

            var serviceProvider = services.BuildServiceProvider();
            DependencyResolver.ResolveUsing(type => services.Any(p => p.ServiceType == type) ? serviceProvider.GetService(type) : null);

            MainPage = new AppShell();
        }                

        private async void Current_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            if(e.Current.Location.OriginalString == "//LoginPage")
            {
                return;
            }

            var isAlive = await CheckApplicationLifetime();
            if (!isAlive)
            {
                e.Cancel();
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        protected async override void OnStart()
        {
            AppCenter.Start("27f68fc7-587a-48b6-aa5f-48fcdc59e28c", typeof(Analytics), typeof(Crashes));

            var isAlive = await CheckApplicationLifetime();
            if (!isAlive)
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }

            Shell.Current.Navigating += Current_Navigating;
        }

        protected override void OnSleep()
        {
        }

        protected async override void OnResume()
        {
            var isAlive = await CheckApplicationLifetime();
            if (!isAlive)
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        public static async Task<bool> CheckApplicationLifetime()
        {
            if (!Application.Current.Properties.TryGetValue("token", out var tokenObj) 
                || string.IsNullOrEmpty(tokenObj?.ToString()))
            {
                return false;                
            }

            var token = tokenObj.ToString();
            var checkAuthorizeRequest = new ValidateRefreshTokenRequest { RefreshToken = token.ToString() };
            var mediator = DependencyService.Resolve<IMediator>();
            var response = await mediator.Send(checkAuthorizeRequest);

            if (!response.IsValid)
            {
                return false;                
            }

            return true;
        }
    }
}
