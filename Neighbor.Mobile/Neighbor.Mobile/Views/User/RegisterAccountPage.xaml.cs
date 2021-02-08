using Neighbor.Mobile.ViewModels.User;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Neighbor.Mobile.Views.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterAccountPage : ContentPage
    {
        private readonly RegisterAccountViewModel viewModel;

        public RegisterAccountPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RegisterAccountViewModel();

            viewModel.OnCancelRegisterAccount += ViewModel_OnCancelRegisterAccount;
            viewModel.OnSubmitAccount += ViewModel_OnSubmitAccount;
        }

        private async void ViewModel_OnSubmitAccount(object sender, System.EventArgs e)
        {
            Preferences.Set("userName", viewModel.UserName.Value);
            Preferences.Set("password", viewModel.Password.Value);

            await Shell.Current.Navigation.PushModalAsync(new RegisterUserInfoPage());
        }

        private async void ViewModel_OnCancelRegisterAccount(object sender, System.EventArgs e)
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}