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
    public class ChangePasswordViewModel : BaseViewModel
    {
        private ValidatableObject<string> currentPassword;
        private ValidatableObject<string> newPassword;
        private ValidatableObject<string> confirmPassword;

        public ValidatableObject<string> CurrentPassword
        {
            get => currentPassword;
            set
            {
                SetProperty(ref currentPassword, value);
            }
        }
        public ValidatableObject<string> NewPassword
        {
            get => newPassword;
            set
            {
                SetProperty(ref newPassword, value);
            }
        }
        public ValidatableObject<string> ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                SetProperty(ref confirmPassword, value);
            }
        }

        public Command ValidateCurrentPassword { get; set; }
        public Command ValidateNewPassword { get; set; }
        public Command ValidateConfirmPassword { get; set; }
        public Command SubmitChangePasswordCommand { get; set; }
        public Command CancelChangePasswordCommand { get; set; }

        public delegate void ChangePassowrdErrorHandler(string errorMessage);

        public event ChangePassowrdErrorHandler OnChangePasswordError;

        public event EventHandler OnChangePasswordSuccess;
        public event EventHandler OnCancelChangePassword;

        public ChangePasswordViewModel()
        {
            currentPassword = new ValidatableObject<string>();
            newPassword = new ValidatableObject<string>();
            confirmPassword = new ValidatableObject<string>();

            ValidateCurrentPassword = new Command(() => ValidateProperty(currentPassword));
            ValidateNewPassword = new Command(() => ValidateProperty(newPassword));
            ValidateConfirmPassword = new Command(() => ValidateProperty(confirmPassword));

            const string isNullOrEmptyErrorMessage = "Required";

            currentPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            currentPassword.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            currentPassword.Validations.Add(new MaxLenghtEntryRule<string>(15) { ValidationMessage = "Too large" });

            newPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            newPassword.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            newPassword.Validations.Add(new MaxLenghtEntryRule<string>(15) { ValidationMessage = "Too large" });

            confirmPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            confirmPassword.Validations.Add(new CompareEntryRule<string>(this, nameof(NewPassword)) { ValidationMessage = "Not match with Password" });

            CancelChangePasswordCommand = new Command(() => OnCancelChangePassword?.Invoke(this, null));
            SubmitChangePasswordCommand = new Command(ExecuteSubmitChangePassword, Validate);
        }

        public bool Validate(object _)
        {
            var isCurrentPasswordValid = currentPassword.Validate();
            var isNewPasswordValid = newPassword.Validate();
            var isConfirmPasswordValid = confirmPassword.Validate();

            var isValid = isCurrentPasswordValid && isNewPasswordValid && isConfirmPasswordValid;

            return isValid;
        }

        private bool ValidateProperty<T>(ValidatableObject<T> property)
        {
            var isValid = property.Validate();
            SubmitChangePasswordCommand.ChangeCanExecute();
            return isValid;
        }

        private async void ExecuteSubmitChangePassword(object obj)
        {
            IsBusy = true;

            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = await GetOAuthHttpClientAsync(ClientTypeName.Identity, cancellationTokenSource.Token);

            if(httpClient == null)
            {
                IsBusy = false;
                return;
            }

            var form = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string,string>("currentPassword", CurrentPassword.Value),
                new KeyValuePair<string,string>("password", NewPassword.Value)
            });

            var response = await httpClient.PostAsync($"user/password/change", form);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                OnChangePasswordError?.Invoke(errorMessage);

                IsBusy = false;

                return;
            }

            OnChangePasswordSuccess.Invoke(this, null);

            IsBusy = false;
        }
    }
}
