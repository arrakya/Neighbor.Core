using Xamarin.Forms;

namespace Neighbor.Mobile.Extends
{
    public class CustomShellContent : ShellContent
    {
        public static BindableProperty FontAwesomeCodeProperty = BindableProperty.Create(nameof(FontAwesomeCode), typeof(string), typeof(CustomShellContent));
        public static BindableProperty FontAwesomeSetProperty = BindableProperty.Create(nameof(FontAwesomeSet), typeof(string), typeof(CustomShellContent));

        public string FontAwesomeCode
        {
            get
            {
                return (string)GetValue(FontAwesomeCodeProperty);
            }
            set
            {
                SetValue(FontAwesomeCodeProperty, value);
            }
        }

        public string FontAwesomeSet
        {
            get
            {
                return (string)GetValue(FontAwesomeSetProperty);
            }
            set
            {
                SetValue(FontAwesomeSetProperty, value);
            }
        }

        public CustomShellContent() : base()
        {

        }
    }
}
