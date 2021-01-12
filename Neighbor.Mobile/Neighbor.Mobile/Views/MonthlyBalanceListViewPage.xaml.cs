using Neighbor.Mobile.ViewModels;
using System;

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
            //_viewModel.AccessTokenExpired += ViewModel_OpenLoginPage;
            _viewModel.OpenYearPickerHandler += ViewModel_OpenYearPickerHandler;
            _viewModel.Year = DateTime.Now.Year;
        }

        private async void ViewModel_OpenYearPickerHandler(object sender, EventArgs e)
        {
            var yearStr = await DisplayActionSheet("Select Year", string.Empty, string.Empty, new[] { "2020", "2021" });
            if (string.IsNullOrEmpty(yearStr))
            {
                yearStr = _viewModel.Year.ToString();
            }
            _viewModel.Year = Convert.ToInt16(yearStr);
        }

        private async void ViewModel_OpenLoginPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
