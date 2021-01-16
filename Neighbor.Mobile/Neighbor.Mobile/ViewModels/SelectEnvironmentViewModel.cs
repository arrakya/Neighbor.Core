using Neighbor.Mobile.ViewModels.Base;
using System;
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
        public event EventHandler CancelSelectEnvironment;

        public Command SaveSelectEnvironmentCommand { get; private set; }
        public Command CancelSelectEnvironmentCommand { get; private set; }

        public SelectEnvironmentViewModel()
        {
            SaveSelectEnvironmentCommand = new Command(() =>
            {
                SaveSelectEnvironment?.Invoke(selectedEnvironment);
            });

            CancelSelectEnvironmentCommand = new Command(() =>
            {
                CancelSelectEnvironment?.Invoke(this, null);
            });
        }
    }
}
