﻿using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using Neighbor.Core.Application;
using Neighbor.Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Neighbor.Mobile
{
    public partial class App : Xamarin.Forms.Application
    {
        static IContainer container;
        static readonly ContainerBuilder builder = new ContainerBuilder();


        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
           
            DependencyResolver.ResolveUsing(type => container.IsRegistered(type) ? container.Resolve(type) : null);

            var applicationAssembly = typeof(ApplicationStartup).Assembly;
            builder.RegisterMediatR(new[] { applicationAssembly });
            ApplicationStartup.ConfigureBuilder(builder);

            container = builder.Build();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
