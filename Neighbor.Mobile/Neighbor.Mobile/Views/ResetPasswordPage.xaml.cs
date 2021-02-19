using Neighbor.Mobile.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Neighbor.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasswordPage : ContentPage
    {
        private readonly ResetPasswordViewModel viewModel;

        public event EventHandler OnSubmitResetPassword;
        public event EventHandler OnCancelResetPassword;

        public string PhoneNumber
        {
            set
            {
                viewModel.PhoneNumber = value;
            }
        }
        public ResetPasswordPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ResetPasswordViewModel();

            viewModel.OnCancelResetPassword += ViewModel_OnCancelResetPassword;
            viewModel.OnSubmitResetPassword += ViewModel_OnSubmitResetPassword;
            viewModel.OnSubmitResetPasswordError += ViewModel_OnSubmitResetPasswordError;
        }

        private async void ViewModel_OnSubmitResetPasswordError(object sender, string errorMessage)
        {
            await DisplayAlert("Reset Password", errorMessage, "Close");
        }

        private void ViewModel_OnSubmitResetPassword(object sender, EventArgs e)
        {
            OnSubmitResetPassword?.Invoke(sender, e);
        }

        private void ViewModel_OnCancelResetPassword(object sender, EventArgs e)
        {
            OnCancelResetPassword?.Invoke(sender, e);
        }
    }
}