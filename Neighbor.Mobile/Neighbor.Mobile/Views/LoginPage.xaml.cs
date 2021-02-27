﻿using Neighbor.Mobile.NativeHelpers;
using Neighbor.Mobile.Services;
using Neighbor.Mobile.ViewModels;
using Neighbor.Mobile.Views.User;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Neighbor.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel viewModel;

        public string UserName => viewModel.UserName.Value;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new LoginViewModel();

            viewModel.OnClickRegister += ViewModel_OnClickRegister;
            viewModel.OnLoginError += ViewModel_OnLoginError;
            viewModel.OnLoginSuccess += ViewModel_OnLoginSuccess;
            viewModel.OnTapLoginLabel += ViewModel_OnTapLoginLabel;
            viewModel.OnForgetPassword += ViewModel_OnForgetPasswordCommand;
            viewModel.OnRequestPIN += ViewModel_OnRequestPIN;
            viewModel.OnRequestPINError += ViewModel_OnRequestPINError;
        }

        private async void ViewModel_OnRequestPINError(object sender, string errorMessage)
        {
            await DisplayAlert("Forget Password", errorMessage, "Close");
        }

        private async void ViewModel_OnRequestPIN(object sender, string reference, string phoneNumber)
        {
            var enterPINPage = new EnterPinPage()
            {
                Message = "Please enter OTP from your Phone in SMS.",
                Refer = reference,
                PhoneNumber = phoneNumber
            };

            enterPINPage.OnSuccessCallback += async (enterPINSender, args) =>
            {
                await Shell.Current.Navigation.PopModalAsync();

                var resetPasswordPage = new ResetPasswordPage();
                resetPasswordPage.PhoneNumber = phoneNumber;
                resetPasswordPage.OnCancelResetPassword += async (resetPasswordSender, resetPasswordArgs) =>
                {
                    await Shell.Current.Navigation.PopModalAsync();
                };

                resetPasswordPage.OnSubmitResetPassword += async (resetPasswordSender, resetPasswordArgs) =>
                {
                    await Shell.Current.Navigation.PopModalAsync();

                    var toastHelper = DependencyService.Resolve<IToastHelper>(DependencyFetchTarget.NewInstance);
                    toastHelper.Show("Reset password done. Please use new password to Login");
                };

                await Shell.Current.Navigation.PushModalAsync(resetPasswordPage);
            };


            await Shell.Current.Navigation.PushModalAsync(enterPINPage);
        }

        private async void ViewModel_OnTapLoginLabel(object sender, System.EventArgs e)
        {
            if (App.BranchVersion.Contains("master"))
            {
                return;
            }
            await Shell.Current.Navigation.PushModalAsync(new SelectEnvironmentPage());
        }

        private void ViewModel_OnLoginError(LoginViewModel sender, string errorMessage)
        {
            DisplayAlert("Fail", errorMessage, "Close");
        }

        private async void ViewModel_OnLoginSuccess(object sender, System.EventArgs e)
        {
            var userContextUservice = DependencyService.Resolve<UserContextService>(DependencyFetchTarget.GlobalInstance);
            MessagingCenter.Send(userContextUservice, "UpdateUserContext");

            await Shell.Current.GoToAsync("//MonthlyBalanceListViewPage");
        }

        private async void ViewModel_OnClickRegister(object sender, System.EventArgs e)
        {
            //await Shell.Current.Navigation.PushModalAsync(new RegisterPage());
            await Shell.Current.Navigation.PushModalAsync(new TermAndConditionPage());
        }

        private async void ViewModel_OnForgetPasswordCommand(object sender, System.EventArgs e)
        {
            var phoneNumber = await DisplayPromptAsync("Forget password", "Please enter your registrated phone number.", accept: "OK", cancel: null, placeholder: "Phone number", maxLength: 20);

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                viewModel.IsBusy = true;
                viewModel.RequestPINCommand.Execute(phoneNumber);
                viewModel.IsBusy = false;
                return;
            }
            await DisplayAlert("Forget password", "Phone number cannot be empty.", "Close");
        }
    }
}