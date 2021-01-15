using Neighbor.Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Neighbor.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectEnvironmentPage : ContentPage
    {
        private readonly SelectEnvironmentViewModel viewModel;

        public SelectEnvironmentPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectEnvironmentViewModel();
            viewModel.SaveSelectEnvironment += ViewModels_SaveSelectEnvironment;
            viewModel.CancelSelectEnvironment += ViewModel_CancelSelectEnvironment;
            viewModel.SelectedEnvironment = App.ReleaseVersion;
        }

        private async void ViewModel_CancelSelectEnvironment(object sender, System.EventArgs e)
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async void ViewModels_SaveSelectEnvironment(string selectEnvironment)
        {
            App.ReleaseVersion = selectEnvironment;

            Preferences.Remove("RefreshToken");
            Preferences.Remove("AccessToken");

            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}