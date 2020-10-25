using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Neighbor.Mobile.Services;
using Neighbor.Mobile.Views;
using Neighbor.Application;
using Neighbor.Domain.Interfaces.Finance;
using Autofac;
using Xamarin.Forms.Internals;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;

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
