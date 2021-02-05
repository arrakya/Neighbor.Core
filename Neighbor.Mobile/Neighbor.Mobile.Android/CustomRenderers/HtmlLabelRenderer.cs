using Android.Content;
using Android.Text;
using Android.Widget;
using Neighbor.Mobile.Droid.CustomRenderers;
using Neighbor.Mobile.Extends;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace Neighbor.Mobile.Droid.CustomRenderers
{
    public class HtmlLabelRenderer : LabelRenderer
    {
        public HtmlLabelRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            Control?.SetText(Html.FromHtml(Element.Text, FromHtmlOptions.ModeCompact), TextView.BufferType.Spannable);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                Control?.SetText(Html.FromHtml(Element.Text, FromHtmlOptions.ModeCompact), TextView.BufferType.Spannable);
            }
        }

    }
}