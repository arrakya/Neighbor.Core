using Microsoft.AppCenter.Analytics;
using Neighbor.Mobile.Services;
using Neighbor.Mobile.ViewModels;
using Neighbor.Mobile.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Neighbor.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private readonly FlyoutViewModel viewModel;

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            BindingContext = viewModel = new FlyoutViewModel();

            MessagingCenter.Subscribe<UserContextService>(this, "UpdateUserContext", async (userContextService) =>
            {
                viewModel.DisplayName = await userContextService.GetUserName();
            });
        }

        public async Task UpdateFlyoutViewModel()
        {
            var userContextService = DependencyService.Resolve<UserContextService>(DependencyFetchTarget.GlobalInstance);
            viewModel.DisplayName = await userContextService.GetUserName();
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
