using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using Neighbor.Core.Application;
using Neighbor.Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

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
