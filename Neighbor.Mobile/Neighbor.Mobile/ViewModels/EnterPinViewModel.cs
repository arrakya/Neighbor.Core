using Neighbor.Mobile.ViewModels.Base;
using System;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels
{
    public class EnterPinViewModel : BaseViewModel
    {
        private string message;
        private string refer;

        public string Message
        {
            get => message;
            set
            {
                SetProperty(ref message, value);
            }
        }
        public string Refer
        {
            get => refer;
            set
            {
                SetProperty(ref refer, value);
            }
        }

        public Command SubmitPINCommand { get; set; }
        public Command CancelSubmitPINCommand { get; set; }

        public event EventHandler OnSubmitPIN;
        public event EventHandler OnCancelSubmitPIN;

        public EnterPinViewModel()
        {
            SubmitPINCommand = new Command(() => OnSubmitPIN?.Invoke(this, null));
            CancelSubmitPINCommand = new Command(() => OnCancelSubmitPIN?.Invoke(this, null));
        }
    }
}
