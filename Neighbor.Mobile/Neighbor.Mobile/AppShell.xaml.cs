using MediatR;
using Microsoft.AppCenter.Analytics;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Mobile.Views;
using System;
using Xamarin.Forms;

namespace Neighbor.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);
            
            Analytics.TrackEvent(args.Current.Location.OriginalString);
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {            
            await Current.GoToAsync("//LoginPage");
        }
    }
}
