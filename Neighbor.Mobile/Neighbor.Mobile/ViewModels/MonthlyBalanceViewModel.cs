using MediatR;
using Neighbor.Core.Application.Request.Finance;
using Neighbor.Core.Domain.Models.Finance;
using Neighbor.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

                foreach(var item in Items)
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
                SetProperty(ref _year, value, nameof(Year));
            }
        }

        public bool IsShowAll
        {
            get => _isShowAll;
            set
            {
                SetProperty(ref _isShowAll, value, nameof(IsShowAll));
                LoadItems(Year, value);
            }
        }        

        public ICommand LoadItemsCommand { get; }        

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
            Year = DateTime.Now.Year;
            LoadItemsCommand = new Command(async (object commandParam) => await LoadItems(Year, IsShowAll));           
        }

        private async Task LoadItems(int year, bool showAll)
        {
            IsBusy = true;
            var mediator = DependencyService.Resolve<IMediator>();
            var request = new MonthlyBalanceRequest { Year = year };
            var response = await mediator.Send(request);
            content = response.Content.Select(p => new MonthlyBalanceModel(p, ShowAllIncomeView));

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
