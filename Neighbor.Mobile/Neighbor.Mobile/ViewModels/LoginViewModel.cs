using Neighbor.Core.Domain.Models.Security;
using Neighbor.Mobile.NativeHelpers;
using Neighbor.Mobile.Validation;
using Neighbor.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private ValidatableObject<string> userName;
        private ValidatableObject<string> password;

        public string AppVersionName
        {
            get
            {
                var appVersionHelper = DependencyService.Resolve<IAppVersionHelper>(DependencyFetchTarget.NewInstance);
                return appVersionHelper.AppVersion;
            }
        }

        public ValidatableObject<string> UserName
        {
            get => userName;
            set
            {
                SetProperty(ref userName, value);
            }
        }
        public ValidatableObject<string> Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
            }
        }

        public Command LoginCommand { get; set; }
        public Command RegisterCommand { get; set; }
        public Command ValidateUserNameCommand { get; set; }
        public Command ValidatePasswordCommand { get; set; }
        public Command TapLoginLabelCommand { get; set; }
        public Command ForgetPasswordCommand { get; set; }

        public event EventHandler OnLoginSuccess;
        public event EventHandler OnTapLoginLabel;
        public event EventHandler OnClickRegister;
        public event EventHandler OnForgetPassword;

        public delegate void LoginErrorHandler(LoginViewModel sender, string errorMessage);
        public event LoginErrorHandler OnLoginError;

        public LoginViewModel()
        {
            LoginCommand = new Command(Login, (args) => Validate());
            ValidateUserNameCommand = new Command(() => ValidateProperty(userName));
            ValidatePasswordCommand = new Command(() => ValidateProperty(password));

            const string isNullOrEmptyErrorMessage = "Required";

            userName = new ValidatableObject<string>();
            password = new ValidatableObject<string>();

            userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            userName.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            userName.Validations.Add(new MaxLenghtEntryRule<string>(15) { ValidationMessage = "Too large" });
            userName.Validations.Add(new RegexEntryRule<string>("\\W", false) { ValidationMessage = "Restrict chars" });

            password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            password.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            password.Validations.Add(new MaxLenghtEntryRule<string>(15) { ValidationMessage = "Too large" });

            RegisterCommand = new Command(() =>
            {
                OnClickRegister?.Invoke(this, null);
            });

            TapLoginLabelCommand = new Command(() => OnTapLoginLabel?.Invoke(this, null));
            ForgetPasswordCommand = new Command(() => OnForgetPassword?.Invoke(this, null));
        }

        public bool ValidateProperty<T>(ValidatableObject<T> property)
        {
            var isValid = property.Validate();
            LoginCommand.ChangeCanExecute();
            return isValid;
        }

        public bool Validate()
        {
            var isUserNameValid = userName.Validate();
            var isPasswordValid = password.Validate();

            var isValid = isUserNameValid && isPasswordValid;

            return isValid;
        }

        public async void Login(object args)
        {
            IsBusy = true;

            var httpClient = GetBasicHttpClient(ClientTypeName.Identity);
            var request = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type","password"),
                new KeyValuePair<string,string>(nameof(userName), UserName.Value),
                new KeyValuePair<string,string>(nameof(password), Password.Value)
            });

            var response = await httpClient.PostAsync("user/oauth/token", request);

            if (!response.IsSuccessStatusCode)
            {
                OnLoginError?.Invoke(this, "Login fail");
                IsBusy = false;
                return;
            }

            var tokenString = await response.Content.ReadAsStringAsync();
            var tokens = JsonSerializer.Deserialize<TokensModel>(tokenString);

            App.RefreshToken = tokens.refresh_token;
            App.AccessToken = tokens.access_token;

            IsBusy = false;

            OnLoginSuccess?.Invoke(this, null);
        }
    }
}
