using Neighbor.Mobile.ViewModels;

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

            Application.Current.Properties.TryGetValue("ReleaseVersion", out var releaseVersion);
            viewModel.SelectedEnvironment = releaseVersion?.ToString();
        }

        private async void ViewModels_SaveSelectEnvironment(string selectEnvironment)
        {
            Application.Current.Properties.Remove("ReleaseVersion");
            Application.Current.Properties["ReleaseVersion"] = selectEnvironment;

            Application.Current.Properties.Remove("refresh_token");
            Application.Current.Properties.Remove("access_token");

            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}