using Neighbor.Mobile.ViewModels.User;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Neighbor.Mobile.Views.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermAndConditionPage : ContentPage
    {
        private double previousScrollPosition = 0;
        private readonly TermAndConditionViewModel viewModel;

        public TermAndConditionPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new TermAndConditionViewModel();

            viewModel.OnCancelAcceptTC += ViewModel_OnCancelAcceptTC;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.TCLoadingCommand.Execute(null);
        }

        private async void ViewModel_OnCancelAcceptTC(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (previousScrollPosition < e.ScrollY)
            {
                //scrolled down
                previousScrollPosition = e.ScrollY;

                var scrollView = (ScrollView)sender;
                var contentHeight = scrollView.ContentSize.Height;
                var bottomPosition = contentHeight - scrollView.Height;

                if (!viewModel.IsScrollTCToButtom)
                {
                    viewModel.IsScrollTCToButtom = Math.Round(bottomPosition, 0) == Math.Round(e.ScrollY, 0);
                }
            }
            else
            {
                //scrolled up
                if (Convert.ToInt16(e.ScrollY) == 0)
                    previousScrollPosition = 0;
            }
        }
    }
}