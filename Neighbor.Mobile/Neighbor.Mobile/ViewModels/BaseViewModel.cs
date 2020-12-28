using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using Neighbor.Mobile.Models;
using Neighbor.Mobile.Services;
using Neighbor.Core.Application.Requests.Security;
using MediatR;
using System.Threading.Tasks;

namespace Neighbor.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = "Neighbor";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        protected async Task<bool> PrepareAccessToken()
        {
            var mediator = DependencyService.Resolve<IMediator>();

            var hasAccessToken = Application.Current.Properties.TryGetValue("access_token", out var accessToken);
            if (!hasAccessToken)
            {
                // No access token
                return false;
            }

            var validateTokenRequest = new ValidateTokenRequest { Token = accessToken.ToString() };
            var validateTokenResponse = await mediator.Send(validateTokenRequest);

            if (validateTokenResponse.IsValid)
            {
                // Has access token and valid
                return true;
            }

            var hasRefreshToken = Application.Current.Properties.TryGetValue("access_token", out var refreshToken);
            if (!hasRefreshToken)
            {
                // No refresh token
                return false;
            }

            validateTokenRequest.Token = refreshToken.ToString();
            validateTokenResponse = await mediator.Send(validateTokenRequest);

            if (!validateTokenResponse.IsValid)
            {
                // Has refresh token but not valid
                return false;
            }

            var accessTokenRequest = new AccessTokenRequest { RefreshToken = refreshToken.ToString() };
            var accessTokenResponse = await mediator.Send(accessTokenRequest);

            if (Application.Current.Properties.ContainsKey("access_token"))
            {
                Application.Current.Properties.Remove("access_token");
            }

            Application.Current.Properties.Add("access_token", accessTokenResponse.Tokens.access_token);

            return true;
        }
    }
}
