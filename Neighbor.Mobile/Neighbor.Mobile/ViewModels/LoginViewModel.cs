using MediatR;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Mobile.Models;
using Neighbor.Mobile.Views;
using System.IO;
using System.Text;
using System.Text.Json;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            IsBusy = true;
            var request = new RefreshTokenRequest
            {
                Username = "arrakya",
                Password = "12345678"
            };
            var mediator = DependencyService.Resolve<IMediator>();
            var response = await mediator.Send(request);
            var tokens = response.Tokens;

            const string refreshTokenPropertyName = "refresh_token";
            if (Application.Current.Properties.ContainsKey(refreshTokenPropertyName))
            {
                Application.Current.Properties.Remove(refreshTokenPropertyName);
            }

            const string accessTokenPropertyName = "access_token";
            if (Application.Current.Properties.ContainsKey(accessTokenPropertyName))
            {
                Application.Current.Properties.Remove(accessTokenPropertyName);
            }

            Application.Current.Properties.Add(refreshTokenPropertyName, tokens.refresh_token);
            Application.Current.Properties.Add(accessTokenPropertyName, tokens.access_token);


            IsBusy = false;
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(MonthlyBalanceListViewPage)}");
        }
    }
}
