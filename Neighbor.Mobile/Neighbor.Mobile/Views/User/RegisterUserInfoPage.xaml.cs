using Neighbor.Mobile.NativeHelpers;
using Neighbor.Mobile.ViewModels.User;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Neighbor.Mobile.Views.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterUserInfoPage : ContentPage
    {
        private readonly RegisterUserInfoViewModel viewModel;

        public RegisterUserInfoPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RegisterUserInfoViewModel();
            viewModel.UserName = Preferences.Get("userName", string.Empty);
            viewModel.Password = Preferences.Get("password", string.Empty);

            viewModel.OnCancelUserInfoAccount += ViewModel_OnCancelUserInfoAccount;
            viewModel.OnRegisterError += ViewModel_OnRegisterError;
            viewModel.OnRegisterSuccessAccount += ViewModel_OnRegisterSuccessAccount;
        }

        private async void ViewModel_OnRegisterSuccessAccount(RegisterUserInfoViewModel sender, string pinReference)
        {
            await Shell.Current.Navigation.PopModalAsync();

            var enterPinPage = new EnterPinPage
            {
                Refer = pinReference,
                PhoneNumber = sender.PhoneNumber.Value,
                Message = "Please enter OTP PIN from your registration phone number."
            };

            enterPinPage.OnSuccessCallback += async (callbackSender, callbackArgs) =>
            {
                sender.ActivateAccountCommand.Execute(null);

                var toastHelper = DependencyService.Resolve<IToastHelper>(DependencyFetchTarget.NewInstance);
                toastHelper.Show("Now you can login");

                await Shell.Current.Navigation.PopToRootAsync(false);
            };

            await Shell.Current.Navigation.PushModalAsync(enterPinPage);
        }

        private async void ViewModel_OnRegisterError(object sender, string errorMessage)
        {
            await DisplayAlert("Error", errorMessage, "Close");
        }

        private async void ViewModel_OnCancelUserInfoAccount(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}