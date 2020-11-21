using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Mobile.Views;
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
            var request = new AuthorizeRequest
            {
                Username = "arrakya",
                Password = "12345678"
            };
            var mediator = DependencyService.Resolve<IMediator>();
            var response = await mediator.Send(request);
            var token = response.Token;

            if (Application.Current.Properties.ContainsKey("token"))
            {
                Application.Current.Properties.Remove("token");
            }

            Application.Current.Properties.Add("token", token);
            IsBusy = false;
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(MonthlyBalanceListViewPage)}");
        }
    }
}
