using MediatR;
using Neighbor.Application.Request.Finance;
using Xamarin.Forms;

namespace Neighbor.Mobile.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            var mediator = DependencyService.Resolve<IMediator>();
            var request = new MonthlyBalanceRequest { Year = 2020 };
            var response = await mediator.Send(request);

            System.Diagnostics.Debugger.Break();

            base.OnAppearing();
        }
    }
}