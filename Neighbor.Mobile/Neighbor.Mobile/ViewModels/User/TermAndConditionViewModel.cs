using Neighbor.Mobile.ViewModels.Base;
using System;
using Xamarin.Forms;

namespace Neighbor.Mobile.ViewModels.User
{
    public class TermAndConditionViewModel : BaseViewModel
    {
        private string termAndConditionMessage;
        private bool isEnableAcceptButton;

        public string TermAndConditionMessage
        {
            get => termAndConditionMessage;
            private set
            {
                SetProperty(ref termAndConditionMessage, value);
            }
        }
        public bool IsScrollTCToButtom
        {
            get => isEnableAcceptButton;
            set
            {
                SetProperty(ref isEnableAcceptButton, value, onChanged: () =>
                 {
                     if (value)
                     {
                         AcceptTCCommand.ChangeCanExecute();
                     }
                 });
            }
        }
        public Command TCLoadingCommand { get; private set; }
        public Command AcceptTCCommand { get; private set; }
        public Command CancelAcceptTCCommand { get; private set; }

        public event EventHandler OnAcceptTC;
        public event EventHandler OnCancelAcceptTC;

        public TermAndConditionViewModel()
        {
            TCLoadingCommand = new Command(TCLoadingCommandHandler);
            AcceptTCCommand = new Command(AcceptTCCommandHandler, (_) => IsScrollTCToButtom);
            CancelAcceptTCCommand = new Command(CancelAcceptTCCommandHandler);
        }

        private void TCLoadingCommandHandler(object args)
        {
            TermAndConditionMessage = "<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam ut consequat eros, at lobortis urna. Nulla sagittis ex ac aliquam posuere. Mauris egestas hendrerit erat, et sodales lectus vehicula ut. Nunc sed massa ultricies, varius ipsum sit amet, ultricies quam. Donec sit amet leo in nunc feugiat pharetra eget ut nulla. Nam laoreet nibh sed quam bibendum aliquam. Praesent efficitur ante ut facilisis dignissim. In eget felis commodo, efficitur mi eu, pulvinar quam. Nullam facilisis nulla id neque vehicula, porttitor hendrerit dolor luctus.</p><p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Quisque consectetur efficitur arcu sed vehicula. Aliquam scelerisque urna vitae quam dapibus sollicitudin. Maecenas eget diam non ligula lacinia varius non ac ex. Interdum et malesuada fames ac ante ipsum primis in faucibus. Curabitur ullamcorper posuere congue. Sed mauris risus, efficitur eget vehicula iaculis, aliquet commodo nisl. Maecenas ac erat feugiat, suscipit nibh vel, pellentesque quam. Nunc mollis condimentum bibendum. Cras pulvinar elit lacus, ut pretium nulla commodo eget. Maecenas aliquam libero et odio consectetur aliquam. Aliquam convallis eros dolor, vel scelerisque felis posuere ut. Nullam tincidunt, est eget tempus convallis, magna metus varius dui, eget fermentum est lorem at nisl. Nunc ultricies condimentum odio non eleifend.</p><p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Quisque consectetur efficitur arcu sed vehicula. Aliquam scelerisque urna vitae quam dapibus sollicitudin. Maecenas eget diam non ligula lacinia varius non ac ex. Interdum et malesuada fames ac ante ipsum primis in faucibus. Curabitur ullamcorper posuere congue. Sed mauris risus, efficitur eget vehicula iaculis, aliquet commodo nisl. Maecenas ac erat feugiat, suscipit nibh vel, pellentesque quam. Nunc mollis condimentum bibendum. Cras pulvinar elit lacus, ut pretium nulla commodo eget. Maecenas aliquam libero et odio consectetur aliquam. Aliquam convallis eros dolor, vel scelerisque felis posuere ut. Nullam tincidunt, est eget tempus convallis, magna metus varius dui, eget fermentum est lorem at nisl. Nunc ultricies condimentum odio non eleifend.</p><p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Quisque consectetur efficitur arcu sed vehicula. Aliquam scelerisque urna vitae quam dapibus sollicitudin. Maecenas eget diam non ligula lacinia varius non ac ex. Interdum et malesuada fames ac ante ipsum primis in faucibus. Curabitur ullamcorper posuere congue. Sed mauris risus, efficitur eget vehicula iaculis, aliquet commodo nisl. Maecenas ac erat feugiat, suscipit nibh vel, pellentesque quam. Nunc mollis condimentum bibendum. Cras pulvinar elit lacus, ut pretium nulla commodo eget. Maecenas aliquam libero et odio consectetur aliquam. Aliquam convallis eros dolor, vel scelerisque felis posuere ut. Nullam tincidunt, est eget tempus convallis, magna metus varius dui, eget fermentum est lorem at nisl. Nunc ultricies condimentum odio non eleifend.</p><p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Vivamus ut feugiat tortor, ultrices fermentum leo. Sed ligula leo, vestibulum at lorem a, pharetra commodo purus. Aliquam eget ornare nulla. Suspendisse vestibulum, massa ac dictum egestas, urna turpis ullamcorper nulla, et fringilla odio dolor et erat. Cras quis viverra urna. Sed ullamcorper eleifend convallis. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Fusce volutpat venenatis vulputate. Sed laoreet ullamcorper suscipit. Ut erat erat, condimentum sed fermentum eget, posuere hendrerit magna. Donec porta, justo nec congue iaculis, felis dui porttitor mi, eget pharetra lectus magna sit amet ligula. Nulla aliquet tortor quis tristique rutrum. Etiam ornare interdum pretium. Vestibulum at suscipit lorem, ut consectetur tortor. In malesuada velit quam, at porttitor enim vehicula ac.</p>";
            IsScrollTCToButtom = false;
        }

        private void AcceptTCCommandHandler(object args)
        {
            OnAcceptTC?.Invoke(this, null);
        }

        private void CancelAcceptTCCommandHandler(object args)
        {
            OnCancelAcceptTC?.Invoke(this, null);
        }
    }
}
