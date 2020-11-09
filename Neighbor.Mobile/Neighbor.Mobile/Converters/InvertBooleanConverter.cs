using System;
using System.Globalization;
using Xamarin.Forms;

namespace Neighbor.Mobile.Converters
{
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var invertBoolean = !System.Convert.ToBoolean(value);
            return invertBoolean;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var invertBoolean = !System.Convert.ToBoolean(value);
            return invertBoolean;
        }
    }
}
