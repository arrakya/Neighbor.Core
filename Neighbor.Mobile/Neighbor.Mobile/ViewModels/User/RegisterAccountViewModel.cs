using Neighbor.Mobile.Validation;
using Neighbor.Mobile.ViewModels.Base;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels.User
{
    public class RegisterAccountViewModel : BaseViewModel
    {
        private ValidatableObject<string> userName;
        private ValidatableObject<string> password;
        private ValidatableObject<string> confirmPassword;

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
        public ValidatableObject<string> ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                SetProperty(ref confirmPassword, value);
            }
        }

        public Command SubmitAccountCommand { get; private set; }
        public Command CancelRegisterAccountCommand { get; private set; }
        public Command ValidateUserNameCommand { get; private set; }
        public Command ValidatePasswordCommand { get; private set; }
        public Command ValidateConfirmPasswordCommand { get; private set; }

        public event EventHandler OnSubmitAccount;
        public event EventHandler OnCancelRegisterAccount;

        public RegisterAccountViewModel()
        {
            SubmitAccountCommand = new Command(async (args) => { await SubmitAccount(args); }, (_) => Validate());
            CancelRegisterAccountCommand = new Command((_) => OnCancelRegisterAccount?.Invoke(this, null));
            ValidateUserNameCommand = new Command(() => ValidateProperty(userName));
            ValidatePasswordCommand = new Command(() => ValidateProperty(password));
            ValidateConfirmPasswordCommand = new Command(() => ValidateProperty(confirmPassword));

            userName = new ValidatableObject<string>();
            password = new ValidatableObject<string>();
            confirmPassword = new ValidatableObject<string>();

            const string isNullOrEmptyErrorMessage = "Required";

            userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            userName.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            userName.Validations.Add(new MaxLenghtEntryRule<string>(15) { ValidationMessage = "Too large" });
            userName.Validations.Add(new RegexEntryRule<string>("\\W", false) { ValidationMessage = "Restrict chars" });

            password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            password.Validations.Add(new MinLenghtEntryRule<string>(6) { ValidationMessage = "Too short" });
            password.Validations.Add(new MaxLenghtEntryRule<string>(15) { ValidationMessage = "Too large" });

            confirmPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = isNullOrEmptyErrorMessage });
            confirmPassword.Validations.Add(new CompareEntryRule<string>(this, nameof(Password)) { ValidationMessage = "Not match with Password" });
        }

        public bool ValidateProperty<T>(ValidatableObject<T> property)
        {
            var isValid = property.Validate();
            SubmitAccountCommand.ChangeCanExecute();
            return isValid;
        }

        public bool Validate()
        {
            var isUserNameValid = userName.Validate();
            var isPasswordValid = password.Validate();
            var isRePasswordValid = confirmPassword.Validate();

            var isValid = isUserNameValid && isPasswordValid && isRePasswordValid;

            return isValid;
        }

        public async Task SubmitAccount(object _)
        {
            if (!Validate())
            {
                return;
            }

            IsBusy = true;

            await Task.Delay(500);

            IsBusy = false;

            OnSubmitAccount?.Invoke(this, null);
        }
    }
}
