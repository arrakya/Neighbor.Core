using Neighbor.Mobile.NativeHelpers;
using Neighbor.Mobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Neighbor.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private readonly RegisterViewModel viewModel;

        public RegisterPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RegisterViewModel();

            viewModel.OnClickCancel += ViewModel_OnClickCancel;
            viewModel.OnRegisterSuccess += ViewModel_OnClickSubmit;
            viewModel.OnRegisterError += ViewModel_OnSubmitError;
        }

        private async void ViewModel_OnSubmitError(RegisterViewModel sender, string errorMessage)
        {
            await DisplayAlert("Error", errorMessage, "Close");
        }
            
        private async void ViewModel_OnClickSubmit(RegisterViewModel sender)
        {
            var toastHelper = DependencyService.Resolve<IToastHelper>(DependencyFetchTarget.NewInstance);
            toastHelper.Show("Now you can login");

            await Shell.Current.Navigation.PopModalAsync();
        }

        private async void ViewModel_OnClickCancel(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}