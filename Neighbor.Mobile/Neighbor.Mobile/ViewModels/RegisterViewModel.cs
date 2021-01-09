using MediatR;
using Neighbor.Core.Application.Requests.Identity;
using System;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string userName;
        private string password;
        private string rePassword;
        private string email;
        private string phone;
        private string houseNumber;

        public string UserName
        {
            get => userName;
            set
            {
                SetProperty(ref userName, value);
            }
        }
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
            }
        }
        public string RePassword
        {
            get => rePassword;
            set
            {
                SetProperty(ref rePassword, value);
            }
        }
        public string Email
        {
            get => email;
            set
            {
                SetProperty(ref email, value);
            }
        }
        public string Phone
        {
            get => phone;
            set
            {
                SetProperty(ref phone, value);
            }
        }
        public string HouseNumber
        {
            get => houseNumber;
            set
            {
                SetProperty(ref houseNumber, value);
            }
        }

        public Command CancelCommand { get; set; }
        public Command SubmitCommand { get; set; }

        public event EventHandler OnClickCancel;
        public event EventHandler OnClickSubmit;

        public RegisterViewModel()
        {
            CancelCommand = new Command(() =>
            {
                OnClickCancel?.Invoke(this, null);
            });

            SubmitCommand = new Command(Submit);
        }

        public async void Submit(object args)
        {
            var request = new CreateUserRequest
            {
                UserName = userName,
                Password = password,
                Email = email,
                HouseNumber = houseNumber,
                Phone = phone
            };
            var mediator = DependencyService.Resolve<IMediator>(DependencyFetchTarget.NewInstance);
            var response = await mediator.Send(request);

            if (response.IsSuccess)
            {
                OnClickSubmit?.Invoke(this, null);
            }
        }
    }
}
