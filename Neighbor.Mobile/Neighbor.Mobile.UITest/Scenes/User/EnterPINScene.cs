using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Neighbor.Mobile.UITest.Scenes.User
{
    public class EnterPINScene
    {
        private IApp app;

        public AppQuery EnterPIN_PINEntry(AppQuery c) => c.Marked("EnterPIN_PINEntry");
        public AppQuery EnterPIN_SubmitButton(AppQuery c) => c.Marked("EnterPIN_SubmitButton");


        public EnterPINScene(IApp app)
        {
            this.app = app;
        }

        public void Play()
        {
            app.EnterText(EnterPIN_PINEntry, "123456");

            app.Tap(EnterPIN_SubmitButton);
        }
    }
}
