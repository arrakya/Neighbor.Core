﻿using MediatR;
using Microsoft.AppCenter;
using Neighbor.Core.Application.Requests.Security;
using Neighbor.Core.Domain.Models.Security;
using Neighbor.Mobile.Models;
using Neighbor.Mobile.Views;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string userName;
        private string password;
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

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        public event EventHandler OnClickRegister;

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(() =>
            {
                OnClickRegister?.Invoke(this, null);
            });
        }

        private async void OnLoginClicked(object obj)
        {
            IsBusy = true;
            var tokens = default(TokensModel);

            try
            {
                var request = new RefreshTokenRequest
                {
                    Username = UserName,
                    Password = Password
                };
                var mediator = DependencyService.Resolve<IMediator>();
                var response = await mediator.Send(request);
                tokens = response.Tokens;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Refresh Token Request Error" ,ex);             
            }

            const string refreshTokenPropertyName = "refresh_token";
            if (Application.Current.Properties.ContainsKey(refreshTokenPropertyName))
            {
                Application.Current.Properties.Remove(refreshTokenPropertyName);
            }

            const string accessTokenPropertyName = "access_token";
            if (Application.Current.Properties.ContainsKey(accessTokenPropertyName))
            {
                Application.Current.Properties.Remove(accessTokenPropertyName);
            }

            Application.Current.Properties.Add(refreshTokenPropertyName, tokens.refresh_token);
            Application.Current.Properties.Add(accessTokenPropertyName, tokens.access_token);


            IsBusy = false;
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(MonthlyBalanceListViewPage)}");
        }
    }
}
