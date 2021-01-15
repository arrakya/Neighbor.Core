using Microsoft.AppCenter.Analytics;
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

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            Application.Current.Properties.Remove("refresh_token");
            Application.Current.Properties.Remove("access_token");
            await Current.GoToAsync("//LoginPage");
        }

        private async void OnEnvironmentClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync("//Environment");
        }
    }
}
