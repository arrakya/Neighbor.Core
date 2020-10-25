using System.ComponentModel;
using Xamarin.Forms;
using Neighbor.Mobile.ViewModels;

namespace Neighbor.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}