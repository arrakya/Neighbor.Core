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
        }

        private async void ViewModel_OnClickRegister(object sender, System.EventArgs e)
        {
            await Shell.Current.Navigation.PushModalAsync(new RegisterPage());
        }
    }
}