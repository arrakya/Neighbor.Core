using Neighbor.Mobile.Validation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using System.Linq;
using Neighbor.Mobile.ViewModels.Base;
using Neighbor.Mobile.Services;
using Neighbor.Mobile.Services.Net;

namespace Neighbor.Mobile.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private ValidatableObject<string> userName;
        private ValidatableObject<string> password;
        private ValidatableObject<string> rePassword;
        private ValidatableObject<string> email;
        private ValidatableObject<string> phone;
        private ValidatableObject<string> houseNumber;

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
        public ValidatableObject<string> RePassword
        {
            get => rePassword;
            set
            {
                SetProperty(ref rePassword, value);
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
        public ValidatableObject<string> Phone
        {
            get => phone;
            set
            {
                SetProperty(ref phone, value);
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

        public Command CancelCommand { get; set; }
        public Command SubmitCommand { get; set; }
        public Command ValidateUserNameCommand { get; set; }
        public Command ValidatePasswordCommand { get; set; }
        public Command ValidateRePasswordCommand { get; set; }
        public Command ValidateEmailCommand { get; set; }
        public Command ValidatePhoneCommand { get; set; }
        public Command ValidateHouseNumberCommand { get; set; }
        public Command ActivateAccountCommand { get; set; }

        public event EventHandler OnClickCancel;

        public delegate void RegisterSuccessHandler(RegisterViewModel sender, string registerReferenceCode);
        public event RegisterSuccessHandler OnRegisterSuccess;

        public delegate void RegisterErrorHandler(RegisterViewModel sender, string errorMessage);
        public event RegisterErrorHandler OnRegisterError;

        public RegisterViewModel()
        {
            CancelCommand = new Command(() =>
            {
                OnClickCancel?.Invoke(this, null);
            });

            SubmitCommand = new Command(Submit, (args) => Validate());
            ValidateUserNameCommand = new Command(() => ValidateProperty(userName));
            ValidatePasswordCommand = new Command(() => ValidateProperty(password));
            ValidateRePasswordCommand = new Command(() => ValidateProperty(rePassword));
            ValidateEmailCommand = new Command(() => ValidateProperty(email));
            ValidatePhoneCommand = new Command(() => ValidateProperty(phone));
            ValidateHouseNumberCommand = new Command(() => ValidateProperty(HouseNumber));
            ActivateAccountCommand = new Command(() => ActivateAccount());

            userName = new ValidatableObject<string>();
            password = new ValidatableObject<string>();
            rePassword = new ValidatableObject<string>();
            email = new ValidatableObject<string>();
            phone = new ValidatableObject<string>();
            houseNumber = new ValidatableObject<string>();

            const string isNullOrEmptyErrorMessage = "Required";

            userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            userName.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            userName.Validations.Add(new MaxLenghtEntryRule<string>(15) { ValidationMessage = "Too large" });
            userName.Validations.Add(new RegexEntryRule<string>("\\W", false) { ValidationMessage = "Restrict chars" });

            password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            password.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            password.Validations.Add(new MaxLenghtEntryRule<string>(15) { ValidationMessage = "Too large" });

            rePassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            rePassword.Validations.Add(new CompareEntryRule<string>(this, nameof(Password)) { ValidationMessage = "Not match with Password" });

            email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            email.Validations.Add(new EmailRule<string> { ValidationMessage = "Invalid Email" });
            email.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            email.Validations.Add(new MaxLenghtEntryRule<string>(50) { ValidationMessage = "Too large" });

            phone.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            phone.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            phone.Validations.Add(new MaxLenghtEntryRule<string>(15) { ValidationMessage = "Too large" });

            houseNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            houseNumber.Validations.Add(new RegexEntryRule<string>("^89/?([0-9]|[0-9][0-9]|1[0-5][0-4]|0[0-9][0-9])$") { ValidationMessage = "Invalid format" });
        }

        public bool ValidateProperty<T>(ValidatableObject<T> property)
        {
            var isValid = property.Validate();
            SubmitCommand.ChangeCanExecute();
            return isValid;
        }

        public bool Validate()
        {
            var isUserNameValid = userName.Validate();
            var isPasswordValid = password.Validate();
            var isRePasswordValid = rePassword.Validate();
            var isEmailValid = email.Validate();
            var isPhoneValid = phone.Validate();
            var isHouseNumberValid = houseNumber.Validate();

            var isValid = isUserNameValid && isPasswordValid && isRePasswordValid && isEmailValid && isEmailValid && isPhoneValid && isHouseNumberValid;

            return isValid;
        }

        public async void Submit(object args)
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
                new KeyValuePair<string, string>(nameof(userName), UserName.Value),
                new KeyValuePair<string, string>(nameof(password), Password.Value),
                new KeyValuePair<string, string>(nameof(email), Email.Value),
                new KeyValuePair<string, string>(nameof(phone), Phone.Value),
                new KeyValuePair<string, string>(nameof(houseNumber), HouseNumber.Value),
            });

            var response = await httpClient.PostAsync("user/create", request);

            IsBusy = false;

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                OnRegisterError?.Invoke(this, errorMessage);
                return;
            }

            createBasicHttpClientResult = await httpClientService.CreateBasicHttpClientAsync(HttpClientService.ClientType.Identity);

            if (!createBasicHttpClientResult.IsReady)
            {
                IsBusy = false;
                return;
            }

            var requestPINService = DependencyService.Resolve<PINService>(DependencyFetchTarget.NewInstance);
            var reference = await requestPINService.RequestAsync(Phone.Value, createBasicHttpClientResult.HttpClient, null);

            OnRegisterSuccess?.Invoke(this, reference);
        }

        private async void ActivateAccount()
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
                new KeyValuePair<string, string>(nameof(userName), UserName.Value)
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
