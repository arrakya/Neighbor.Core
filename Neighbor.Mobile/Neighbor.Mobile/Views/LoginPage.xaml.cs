using Neighbor.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Neighbor.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel viewModel;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new LoginViewModel();

            viewModel.OnClickRegister += ViewModel_OnClickRegister;
            viewModel.OnLoginError += ViewModel_OnLoginError;
            viewModel.OnLoginSuccess += ViewModel_OnLoginSuccess;
            viewModel.OnTapLoginLabel += ViewModel_OnTapLoginLabel;
        }

        private async void ViewModel_OnTapLoginLabel(object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync("//Environment");
        }

        private void ViewModel_OnLoginError(LoginViewModel sender, string errorMessage)
        {
            DisplayAlert("Fail", "Unauthorized", "Close");
        }

        private async void ViewModel_OnLoginSuccess(object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync("//MonthlyBalanceListViewPage");
        }

        private async void ViewModel_OnClickRegister(object sender, System.EventArgs e)
        {
            await Shell.Current.Navigation.PushModalAsync(new RegisterPage());
        }
    }
}