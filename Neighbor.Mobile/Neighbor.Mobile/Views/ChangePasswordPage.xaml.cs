using Neighbor.Mobile.NativeHelpers;
using Neighbor.Mobile.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Neighbor.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        private readonly ChangePasswordViewModel viewModel;

        public ChangePasswordPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ChangePasswordViewModel();

            viewModel.OnChangePasswordSuccess += ViewModel_OnChangePasswordSuccess;
            viewModel.OnCancelChangePassword += ViewModel_OnCancelChangePassword;
            viewModel.OnChangePasswordError += ViewModel_OnChangePasswordError;
        }

        private async void ViewModel_OnChangePasswordSuccess(object sender, EventArgs e)
        {
            var toastHelper = DependencyService.Resolve<IToastHelper>(DependencyFetchTarget.NewInstance);
            toastHelper.Show("Change Password Success");

            await Shell.Current.Navigation.PopModalAsync();
        }

        private async void ViewModel_OnChangePasswordError(string errorMessage)
        {
            await DisplayAlert("Change Password", errorMessage, "Close");
        }

        private async void ViewModel_OnCancelChangePassword(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}