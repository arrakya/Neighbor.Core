using MediatR;
using Neighbor.Core.Application.Handlers.Security;
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

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            Application.Current.Properties.Remove("token");
            await Current.GoToAsync("//LoginPage");
        }

        protected async override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            var isNotInLoginRoute = !CurrentItem.Route.ToLower().Contains($"_{nameof(LoginPage).ToLower()}");
            var isNoToken = !Application.Current.Properties.ContainsKey("token");

            if (!isNotInLoginRoute)
            {
                return;
            }

            if (isNoToken)
            {
                await GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var token = Application.Current.Properties["token"].ToString();
            var checkAuthorizeRequest = new CheckAuthorizeRequest { Token = token };
            var mediator = DependencyService.Resolve<IMediator>();
            var response = await mediator.Send(checkAuthorizeRequest);

            if (!response.IsValid)
            {
                await GoToAsync($"//{nameof(LoginPage)}");
                
                return;
            }
        }
    }
}
