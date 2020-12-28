﻿using MediatR;
using Neighbor.Core.Application.Requests.Finance;
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
            _viewModel.AccessTokenExpired += ViewModel_OpenLoginPage;

            this.Appearing += MonthlyBalanceListViewPage_Appearing;
        }

        private void MonthlyBalanceListViewPage_Appearing(object sender, EventArgs e)
        {
            _viewModel.LoadItemsCommand.Execute(DateTime.Now.Year);
        }

        private async void ViewModel_OpenLoginPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
