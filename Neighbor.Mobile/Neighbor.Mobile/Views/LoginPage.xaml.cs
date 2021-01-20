﻿using Neighbor.Mobile.ViewModels;
using System.Threading.Tasks;
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
            viewModel.OnForgetPassword += ViewModel_OnForgetPasswordCommand;
        }

        private async void ViewModel_OnTapLoginLabel(object sender, System.EventArgs e)
        {
            if (App.IsProductionVersion)
            {
                return;
            }
            await Shell.Current.Navigation.PushModalAsync(new SelectEnvironmentPage());
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

        private async void ViewModel_OnForgetPasswordCommand(object sender, System.EventArgs e)
        {
            var phoneNumber = await DisplayPromptAsync("Forget password", "Please enter your registrated phone number.", accept: "OK", cancel: null, placeholder: "Phone number", maxLength: 20);

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                viewModel.IsBusy = true;
                await Task.Delay(5000);

                await Shell.Current.Navigation.PushModalAsync(new EnterPinPage() 
                { 
                    Message = "Please enter OTP from your Phone in SMS.",
                    Refer = "Reference No. : ABC123"
                });

                viewModel.IsBusy = false;
                return;
            }
            await DisplayAlert("Forget password", "Phone number cannot be empty.", "Close");
        }
    }
}