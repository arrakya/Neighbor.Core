using Microsoft.AppCenter.Analytics;
using Neighbor.Mobile.Views;
using System;
using Xamarin.Essentials;
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
            Preferences.Remove("RefreshToken");
            Preferences.Remove("AccessToken");

            await Current.GoToAsync("//LoginPage");
        }

        private async void OnEnvironmentClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync("//Environment");
        }
    }
}
