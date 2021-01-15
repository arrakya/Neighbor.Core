using Neighbor.Mobile.ViewModels.Base;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels
{
    public class SelectEnvironmentViewModel : BaseViewModel
    {
        private string selectedEnvironment;

        public string SelectedEnvironment
        {
            get => selectedEnvironment;
            set
            {
                SetProperty(ref selectedEnvironment, value);
            }
        }

        public delegate void SaveSelectEnvironmentHandler(string selectEnvironment);

        public event SaveSelectEnvironmentHandler SaveSelectEnvironment;

        public Command SaveSelectEnvironmentCommand { get; private set; }

        public SelectEnvironmentViewModel()
        {
            SaveSelectEnvironmentCommand = new Command(() =>
            {
                SaveSelectEnvironment?.Invoke(selectedEnvironment);
            });
        }
    }
}
