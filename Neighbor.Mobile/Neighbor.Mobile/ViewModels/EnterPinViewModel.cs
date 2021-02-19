using Neighbor.Core.Domain.Models.Security;
using Neighbor.Mobile.Services.Net;
using Neighbor.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels
{
    public class EnterPinViewModel : BaseViewModel
    {
        private string pin;
        private string message;
        private string refer;
        private string phoneNumber;

        public string PIN
        {
            get => pin;
            set
            {
                SetProperty(ref pin, value);
            }
        }
        public string Message
        {
            get => message;
            set
            {
                SetProperty(ref message, value);
            }
        }
        public string Refer
        {
            get => refer;
            set
            {
                SetProperty(ref refer, value);
                OnPropertyChanged(nameof(ReferWithPrefix));
            }
        }

        public string ReferWithPrefix
        {
            get => $"Reference No. : {Refer}";
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                SetProperty(ref phoneNumber, value);
            }
        }

        public Command SubmitPINCommand { get; set; }
        public Command CancelSubmitPINCommand { get; set; }

        public delegate void SubmitPINErrorHandler(object obj, string errorMessage);
        public event EventHandler OnSubmitPIN;
        public event EventHandler OnCancelSubmitPIN;
        public event SubmitPINErrorHandler OnSubmitPINError;

        public EnterPinViewModel()
        {
            SubmitPINCommand = new Command(SubmitPIN);
            CancelSubmitPINCommand = new Command(() => OnCancelSubmitPIN?.Invoke(this, null));
        }

        public async void SubmitPIN(object obj)
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
                new KeyValuePair<string,string>("pin",PIN),
                new KeyValuePair<string,string>("reference",Refer),
                new KeyValuePair<string,string>("phoneNumber",phoneNumber)
            });

            var response = await httpClient.PostAsync($"pin/verify/{phoneNumber}", form);
            var jsonSerializerOption = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var submitPINResult = JsonSerializer.Deserialize<VerifyPINResultModel>(responseContentString, jsonSerializerOption);

            if (!submitPINResult.Result)
            {
                var errorMessage = submitPINResult.Message;
                OnSubmitPINError?.Invoke(this, errorMessage);

                return;
            }

            OnSubmitPIN?.Invoke(this, null);
        }
    }
}
