using Neighbor.Mobile.Services;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Neighbor.Mobile.Converters
{
    public class MenuItemIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value?.ToString())
            {
                case "Change Password":
                    return FontAwesomeIcons.Key;
                case "Logout":
                    return FontAwesomeIcons.DoorOpen;
                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
