using MediatR;
using Neighbor.Core.Application.Request.Finance;
using Neighbor.Core.Domain.Models.Finance;
using Neighbor.Mobile.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Neighbor.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonthlyBalanceListViewPage : ContentPage
    {
        private MonthlyBalanceViewModel _viewModel;

        public MonthlyBalanceListViewPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new MonthlyBalanceViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

           // _viewModel.LoadItemsCommand.Execute(DateTime.Now.Year);
        }
    }
}
