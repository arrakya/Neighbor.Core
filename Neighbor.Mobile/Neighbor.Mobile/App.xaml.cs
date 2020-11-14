﻿using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Neighbor.Core.Application;
using Neighbor.Mobile.Services;
using Neighbor.Mobile.Views;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Neighbor.Mobile
{
    public partial class App : Xamarin.Forms.Application
    {
        //public readonly string ServerAddress = "10.0.2.2";
        public readonly string ServerAddress = "192.168.1.203";
        public readonly string IdentityBaseAddress = $"http://192.168.1.203";
        public readonly string FinanceBaseAddress = $"http://192.168.1.203";

        public App()
        {
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
