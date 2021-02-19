using Neighbor.Core.Domain.Models.Finance;
using Neighbor.Mobile.Models;
using Neighbor.Mobile.Services.Net;
using Neighbor.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels
{
    public class MonthlyBalanceViewModel : BaseViewModel
    {
        private bool _showAllIncomeView;
        private int _year;
        private bool _isShowAll;

        public bool ShowAllIncomeView
        {
            get => _showAllIncomeView;
            set
            {
                SetProperty(ref _showAllIncomeView, value, nameof(ShowAllIncomeView));

                foreach (var item in Items)
                {
                    item.TurnOnIncomeView = value;
                }
            }
        }

        public int Year
        {
            get => _year;
            set
            {
                SetProperty(ref _year, value, nameof(Year), () =>
                {
                    LoadItemsCommand.Execute(null);
                    StoreSelectYear?.Invoke(value);
                });
            }
        }

        public bool IsShowAll
        {
            get => _isShowAll;
            set
            {
                SetProperty(ref _isShowAll, value, nameof(IsShowAll), () =>
                {
                    LoadItemsCommand.Execute(null);
                });
            }
        }

        public ICommand LoadItemsCommand { get; }

        public ICommand OpenYearPickerCommand { get; }

        public event EventHandler OpenYearPickerHandler;

        public delegate void StoreSelectYearHandler(int selectYear);
        public event StoreSelectYearHandler StoreSelectYear;

        private ObservableCollection<MonthlyBalanceModel> items;
        private IEnumerable<MonthlyBalanceModel> content;

        public ObservableCollection<MonthlyBalanceModel> Items
        {
            get => items;
            set
            {
                SetProperty(ref items, value, nameof(Items));
            }
        }

        public MonthlyBalanceViewModel()
        {
            LoadItemsCommand = new Command(async (object commandParam) =>
            {
                IsBusy = true;

                await LoadItems();

                IsBusy = false;
            });
            OpenYearPickerCommand = new Command(() =>
            {
                OpenYearPickerHandler?.Invoke(this, null);
            });
        }

        private async Task LoadItems()
        {
            IsBusy = true;

            var cancellationTokenSource = new CancellationTokenSource();
            var requestUri = $"monthlybalance?year={Year}";
            var httpClientService = DependencyService.Resolve<HttpClientService>(DependencyFetchTarget.NewInstance);
            var createOAuthHttpClientResult = await httpClientService.CreateOAuthHttpClientAsync(HttpClientService.ClientType.Finance, cancellationTokenSource.Token);

            if (!createOAuthHttpClientResult.IsReady)
            {
                IsBusy = false;
                return;
            }

            var httpClient = createOAuthHttpClientResult.HttpClient;
            var response = await httpClient.GetAsync(requestUri, cancellationTokenSource.Token);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                // Access Token Expired.
                IsBusy = false;
                return;
            }

            var responseContent = await response.Content.ReadAsStreamAsync();
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var monthlyBalanceCollection = await JsonSerializer.DeserializeAsync<IEnumerable<MonthlyBalance>>(responseContent, jsonSerializerOptions);
            content = monthlyBalanceCollection.Select(p => new MonthlyBalanceModel(p, ShowAllIncomeView));

            if (!IsShowAll)
            {
                Items = new ObservableCollection<MonthlyBalanceModel>(content.OrderByDescending(p => p.MonthNo).Where(p => p.IncomeAmount > 0));
            }
            else
            {
                Items = new ObservableCollection<MonthlyBalanceModel>(content.OrderByDescending(p => p.MonthNo));
            }

            IsBusy = false;
        }
    }
}
