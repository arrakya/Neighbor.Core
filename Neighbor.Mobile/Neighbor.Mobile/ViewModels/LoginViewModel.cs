using Neighbor.Core.Domain.Models.Security;
using Neighbor.Mobile.NativeHelpers;
using Neighbor.Mobile.Services.Net;
using Neighbor.Mobile.Validation;
using Neighbor.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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
        public Command RequestPINCommand { get; set; }

        public delegate void RequestPINHandler(object sender, string reference, string phoneNumber);
        public delegate void RequestPINErrorHandler(object sender, string errorMessage);
        public delegate void LoginErrorHandler(LoginViewModel sender, string errorMessage);

        public event EventHandler OnLoginSuccess;
        public event EventHandler OnTapLoginLabel;
        public event EventHandler OnClickRegister;
        public event EventHandler OnForgetPassword;
        public event RequestPINHandler OnRequestPIN;
        public event LoginErrorHandler OnLoginError;
        public event RequestPINErrorHandler OnRequestPINError;

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
            RequestPINCommand = new Command(async (obj) => await RequestPIN(obj.ToString()));
        }

        private async Task RequestPIN(string phoneNumber)
        {
            IsBusy = true;

            var cancellationTokenSource = new CancellationTokenSource();
            var httpClientService = DependencyService.Resolve<HttpClientService>(DependencyFetchTarget.NewInstance);
            var createBasicHttpClientResult = await httpClientService.CreateBasicHttpClientAsync(HttpClientService.ClientType.Identity);

            if (!createBasicHttpClientResult.IsReady)
            {
                IsBusy = false;
                return;
            }

            var httpClient = createBasicHttpClientResult.HttpClient;
            var response = await httpClient.GetAsync($"pin/generate/{phoneNumber}");
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonSerializerOption = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var requestPINResult = JsonSerializer.Deserialize<GeneratePINResultModel>(responseString, jsonSerializerOption);

            if (!requestPINResult.IsSuccess)
            {
                var errorMessage = requestPINResult.Message;
                switch (requestPINResult.Code)
                {
                    case "PIN0002":
                        errorMessage = "Phone number not found";
                        break;
                    case "PIN0003":
                        errorMessage = "Too many time request. Please try again in next 24 hrs.";
                        break;
                }

                OnRequestPINError?.Invoke(this, errorMessage);
                IsBusy = false;

                return;
            }

            var pinReference = requestPINResult.Reference;

            OnRequestPIN?.Invoke(this, pinReference, phoneNumber);

            IsBusy = false;
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

            var httpClientService = DependencyService.Resolve<HttpClientService>(DependencyFetchTarget.NewInstance);
            var createBasicHttpClientResult = await httpClientService.CreateBasicHttpClientAsync(HttpClientService.ClientType.Identity);

            if (!createBasicHttpClientResult.IsReady)
            {
                IsBusy = false;
                return;
            }

            var httpClient = createBasicHttpClientResult.HttpClient;
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
