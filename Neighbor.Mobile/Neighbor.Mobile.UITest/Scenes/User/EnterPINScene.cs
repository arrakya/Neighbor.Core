using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Neighbor.Mobile.UITest.Scenes.User
{
    public class EnterPINScene
    {
        private IApp app;

        public AppQuery EnterPIN_SubmitButton(AppQuery c) => c.Marked("EnterPIN_SubmitButton");


        public EnterPINScene(IApp app)
        {
            this.app = app;
        }

        public void Play()
        {
            app.WaitForElement("EnterPIN_PINEntry");
            app.Query(e => e.Marked("EnterPIN_PINEntry").Invoke("setText", "123456"));

            app.Tap(EnterPIN_SubmitButton);
        }
    }
}
