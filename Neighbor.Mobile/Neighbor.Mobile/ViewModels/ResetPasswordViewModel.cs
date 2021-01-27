using Neighbor.Core.Domain.Models.Security;
using Neighbor.Mobile.Services.Net;
using Neighbor.Mobile.Validation;
using Neighbor.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        private ValidatableObject<string> newPassword;
        private ValidatableObject<string> confirmPassword;

        public ValidatableObject<String> NewPassword
        {
            get => newPassword;
            set
            {
                SetProperty(ref newPassword, value);
            }
        }

        public ValidatableObject<String> ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                SetProperty(ref confirmPassword, value);
            }
        }

        public delegate void SubmitResetPasswordErrorHandler(object sender, string errorMessage);

        public event EventHandler OnSubmitResetPassword;
        public event SubmitResetPasswordErrorHandler OnSubmitResetPasswordError;
        public event EventHandler OnCancelResetPassword;

        public Command SubmitCommand { get; set; }
        public Command CancelCommand { get; set; }
        public Command ValidateNewPasswordCommand { get; set; }
        public Command ValidateConfirmPasswordCommand { get; set; }
        public string PhoneNumber { get; set; }

        public ResetPasswordViewModel()
        {
            SubmitCommand = new Command(Submit, (args) => Validate());
            CancelCommand = new Command(() => OnCancelResetPassword?.Invoke(this, null));

            newPassword = new ValidatableObject<string>();
            confirmPassword = new ValidatableObject<string>();

            ValidateNewPasswordCommand = new Command(() => ValidateProperty(newPassword));
            ValidateConfirmPasswordCommand = new Command(() => ValidateProperty(confirmPassword));

            const string isNullOrEmptyErrorMessage = "Required";

            newPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            newPassword.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            newPassword.Validations.Add(new MaxLenghtEntryRule<string>(15) { ValidationMessage = "Too large" });

            confirmPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            confirmPassword.Validations.Add(new CompareEntryRule<string>(this, nameof(NewPassword)) { ValidationMessage = "Not match with New Password" });
        }

        public bool ValidateProperty<T>(ValidatableObject<T> property)
        {
            var isValid = property.Validate();
            SubmitCommand.ChangeCanExecute();
            return isValid;
        }

        public bool Validate()
        {
            var isPasswordValid = newPassword.Validate();
            var isRePasswordValid = confirmPassword.Validate();

            var isValid = isPasswordValid && isRePasswordValid;

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
            var form = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string,string>("password", NewPassword.Value),
                new KeyValuePair<string,string>("phoneNumber", PhoneNumber),
            });

            var response = await httpClient.PostAsync($"user/password/reset", form);
            var jsonSerializerOption = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var submitPINResult = JsonSerializer.Deserialize<ResetPasswordResultModel>(responseContentString, jsonSerializerOption);

            if (!submitPINResult.Result)
            {
                var errorMessage = submitPINResult.Message;
                OnSubmitResetPasswordError?.Invoke(this, errorMessage);

                IsBusy = false;

                return;
            }

            OnSubmitResetPassword?.Invoke(this, null);
            IsBusy = false;
        }
    }
}
