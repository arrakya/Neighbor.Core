using Android.Widget;
using Neighbor.Mobile.Droid;
using Neighbor.Mobile.NativeHelpers;

[assembly: Xamarin.Forms.Dependency(typeof(ToastHelper))]
namespace Neighbor.Mobile.Droid
{
    public class ToastHelper : IToastHelper
    {
        public void Show(string message)
        {
            Toast.MakeText(MainActivity.Context, message, ToastLength.Long).Show();
        }
    }
}