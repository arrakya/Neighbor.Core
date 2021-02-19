using Neighbor.Mobile.Services;
using Neighbor.Mobile.Services.Net;
using Neighbor.Mobile.Validation;
using Neighbor.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels.User
{
    public class RegisterUserInfoViewModel : BaseViewModel
    {
        private ValidatableObject<string> phoneNumber;
        private ValidatableObject<string> email;
        private ValidatableObject<string> houseNumber;

        public string UserName { get; set; }
        public string Password { get; set; }

        public ValidatableObject<string> PhoneNumber
        {
            get => phoneNumber;
            set
            {
                SetProperty(ref phoneNumber, value);
            }
        }
        public ValidatableObject<string> Email
        {
            get => email;
            set
            {
                SetProperty(ref email, value);
            }
        }
        public ValidatableObject<string> HouseNumber
        {
            get => houseNumber;
            set
            {
                SetProperty(ref houseNumber, value);
            }
        }

        public Command SubmitUserInfoCommand { get; private set; }
        public Command CancelUserInfoCommand { get; private set; }
        public Command ActivateAccountCommand { get; private set; }
        public Command ValidatePhoneNumberCommand { get; private set; }
        public Command ValidateEmailCommand { get; private set; }
        public Command ValidateHouseNumberCommand { get; private set; }

        public delegate void RegisterErrorHandler(object sender, string errorMessage);
        public delegate void RegisterSuccessHandler(RegisterUserInfoViewModel sender, string pinReference);

        public event RegisterSuccessHandler OnRegisterSuccessAccount;
        public event EventHandler OnCancelUserInfoAccount;
        public event RegisterErrorHandler OnRegisterError;

        public RegisterUserInfoViewModel()
        {
            SubmitUserInfoCommand = new Command(async (args) => { await SubmitAccount(args); }, (_) => Validate());
            CancelUserInfoCommand = new Command((_) => OnCancelUserInfoAccount?.Invoke(this, null));
            ValidatePhoneNumberCommand = new Command(() => ValidateProperty(phoneNumber));
            ValidateEmailCommand = new Command(() => ValidateProperty(email));
            ValidateHouseNumberCommand = new Command(() => ValidateProperty(houseNumber));
            ActivateAccountCommand = new Command(ActivateAccount);

            phoneNumber = new ValidatableObject<string>();
            email = new ValidatableObject<string>();
            houseNumber = new ValidatableObject<string>();

            const string isNullOrEmptyErrorMessage = "Required";

            email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            email.Validations.Add(new EmailRule<string> { ValidationMessage = "Invalid Email" });
            email.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            email.Validations.Add(new MaxLenghtEntryRule<string>(50) { ValidationMessage = "Too large" });

            phoneNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            phoneNumber.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            phoneNumber.Validations.Add(new MaxLenghtEntryRule<string>(15) { ValidationMessage = "Too large" });

            houseNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            houseNumber.Validations.Add(new RegexEntryRule<string>("^89/?([0-9]|[0-9][0-9]|1[0-5][0-4]|0[0-9][0-9])$") { ValidationMessage = "Invalid format" });
        }

        public bool ValidateProperty<T>(ValidatableObject<T> property)
        {
            var isValid = property.Validate();
            SubmitUserInfoCommand.ChangeCanExecute();
            return isValid;
        }

        public bool Validate()
        {
            var isPhoneNumberValid = phoneNumber.Validate();
            var isEmailValid = email.Validate();
            var isHouseNumberValid = houseNumber.Validate();

            var isValid = isPhoneNumberValid && isEmailValid && isHouseNumberValid;

            return isValid;
        }

        public async Task SubmitAccount(object _)
        {
            if (!Validate())
            {
                return;
            }

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
                new KeyValuePair<string, string>("userName", UserName),
                new KeyValuePair<string, string>("password", Password),
                new KeyValuePair<string, string>("email", Email.Value),
                new KeyValuePair<string, string>("phone", PhoneNumber.Value),
                new KeyValuePair<string, string>("houseNumber", HouseNumber.Value),
            });

            var response = await httpClient.PostAsync("user/create", request);

            IsBusy = false;

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                OnRegisterError?.Invoke(this, errorMessage);
                return;
            }

            var pinReference = await RequestPINAsync();

            OnRegisterSuccessAccount?.Invoke(this, pinReference);
        }

        private async Task<string> RequestPINAsync()
        {
            IsBusy = true;

            var httpClientService = DependencyService.Resolve<HttpClientService>(DependencyFetchTarget.NewInstance);
            var createBasicHttpClientResult = await httpClientService.CreateBasicHttpClientAsync(HttpClientService.ClientType.Identity);

            if (!createBasicHttpClientResult.IsReady)
            {
                IsBusy = false;
                return string.Empty;
            }

            var requestPINService = DependencyService.Resolve<PINService>(DependencyFetchTarget.NewInstance);
            var reference = await requestPINService.RequestAsync(PhoneNumber.Value, createBasicHttpClientResult.HttpClient, null);

            IsBusy = false;
            return reference;
        }

        private async void ActivateAccount(object _)
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
                new KeyValuePair<string, string>("userName", UserName)
            });

            var response = await httpClient.PostAsync("user/activate", request);

            IsBusy = false;

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                OnRegisterError?.Invoke(this, errorMessage);
                return;
            }
        }
    }
}
