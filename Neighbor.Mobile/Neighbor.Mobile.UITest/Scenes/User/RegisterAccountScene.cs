using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Neighbor.Mobile.UITest.Scenes.User
{
    public class RegisterAccountScene
    {
        private IApp app;

        public AppQuery RegisterAccountSubmitButton(AppQuery c) => c.Marked("RegisterAccountSubmitButton");


        public RegisterAccountScene(IApp app)
        {
            this.app = app;
        }

        public void Play()
        {
            app.WaitForElement("RegisterContactUserName");
            app.Query(e => e.Marked("RegisterContactUserName").Invoke("setText", "Test001"));
            app.Query(e => e.Marked("RegisterContactPassword").Invoke("setText", "Password123"));
            app.Query(e => e.Marked("RegisterContactConfirmPassword").Invoke("setText", "Password123"));

            app.Tap(RegisterAccountSubmitButton);
        }
    }
}
