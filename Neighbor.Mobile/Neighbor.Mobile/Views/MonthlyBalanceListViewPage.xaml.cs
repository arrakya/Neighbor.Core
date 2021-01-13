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
            _viewModel.OpenYearPickerHandler += ViewModel_OpenYearPickerHandler;
            _viewModel.StoreSelectYear += StoreMonthlyBalanceSelectYear;
            _viewModel.Year = DateTime.Now.Year;
            if (Application.Current.Properties.TryGetValue("MonthBalance_SelectYear", out var selectYear))
            {
                _viewModel.Year = Convert.ToInt32(selectYear);
            }
        }

        private void StoreMonthlyBalanceSelectYear(int selectYear)
        {
            Application.Current.Properties.Remove("MonthBalance_SelectYear");
            Application.Current.Properties["MonthBalance_SelectYear"] = selectYear.ToString();
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
    }
}
