using Neighbor.Mobile.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Neighbor.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterPinPage : ContentPage
    {
        private readonly EnterPinViewModel viewModel;

        public string Message
        {
            set
            {
                viewModel.Message = value;
            }
        }

        public string Refer
        {
            set
            {
                viewModel.Refer = value;
            }
        }

        public EnterPinPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new EnterPinViewModel();

            viewModel.OnCancelSubmitPIN += ViewModel_OnCancelSubmitPIN;
            viewModel.OnSubmitPIN += ViewModel_OnSubmitPIN;
        }

        private void ViewModel_OnSubmitPIN(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void ViewModel_OnCancelSubmitPIN(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopModalAsync(true);
        }
    }
}