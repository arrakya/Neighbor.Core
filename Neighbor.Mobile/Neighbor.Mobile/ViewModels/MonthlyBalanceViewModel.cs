using MediatR;
using Neighbor.Core.Application.Request.Finance;
using Neighbor.Core.Domain.Models.Finance;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels
{
    public class MonthlyBalanceViewModel : BaseViewModel
    {
        public ICommand LoadItemsCommand { get; }

        private ObservableCollection<MonthlyBalance> items;

        public ObservableCollection<MonthlyBalance> Items
        {
            get => items;
            set
            {
                SetProperty(ref items, value, nameof(Items));
            }
        }

        public MonthlyBalanceViewModel()
        {
            LoadItemsCommand = new Command(async (object commandParam) => await LoadItems(Convert.ToInt16(commandParam)));
        }

        private async Task LoadItems(int year)
        {
            IsBusy = true;
            var mediator = DependencyService.Resolve<IMediator>();
            var request = new MonthlyBalanceRequest { Year = year };
            var response = await mediator.Send(request);

            Title = year.ToString();
            Items = new ObservableCollection<MonthlyBalance>(response.Content.OrderByDescending(p => p.MonthNo));

            IsBusy = false;
        }
    }
}
