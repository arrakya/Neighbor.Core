﻿using Microsoft.AppCenter;
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
            get => Preferences.Get("ReleaseVersion", "Production");
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

        public App()
        {
            var serverAddress = "10.0.2.2";
            var IdentityBaseAddress = $"https://{serverAddress}:6001";
            var FinanceBaseAddress = $"https://{serverAddress}:5001";           

            switch (ReleaseVersion)
            {
                case "SIT":
                    serverAddress = "arrakya.thddns.net:4431";
                    IdentityBaseAddress = $"https://{serverAddress}/neighbor/identity/";
                    FinanceBaseAddress = $"https://{serverAddress}/neighbor/finance/";
                    break;
                case "Production":
                    serverAddress = "arrakya.thddns.net:443";
                    IdentityBaseAddress = $"https://{serverAddress}/neighbor/identity/";
                    FinanceBaseAddress = $"https://{serverAddress}/neighbor/finance/";
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
