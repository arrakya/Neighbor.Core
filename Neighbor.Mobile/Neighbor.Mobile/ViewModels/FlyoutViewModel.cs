using Neighbor.Mobile.ViewModels.Base;

namespace Neighbor.Mobile.ViewModels
{
    public class FlyoutViewModel : BaseViewModel
    {
        private string displayName;

        public string DisplayName
        {
            get => displayName;
            set
            {
                SetProperty(ref displayName, value);
            }
        }
    }
}
