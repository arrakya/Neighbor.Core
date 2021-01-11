using Neighbor.Core.Application.Requests.Finance;
using Neighbor.Core.Application.Responses.Finance;
using Neighbor.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Neighbor.Mobile.ViewModels.Base;

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
            var request = new MonthlyBalanceRequest { Year = Year };
            var response = await Request<MonthlyBalanceRequest, MonthlyBalanceResponse>(request);

            if (response?.Content == null)
            {
                return;
            }

            content = response.Content.Select(p => new MonthlyBalanceModel(p, ShowAllIncomeView));

            if (!IsShowAll)
            {
                Items = new ObservableCollection<MonthlyBalanceModel>(content.OrderByDescending(p => p.MonthNo).Where(p => p.IncomeAmount > 0));
            }
            else
            {
                Items = new ObservableCollection<MonthlyBalanceModel>(content.OrderByDescending(p => p.MonthNo));
            }
        }
    }
}
