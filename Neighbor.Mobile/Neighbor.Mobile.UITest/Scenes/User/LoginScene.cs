using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Neighbor.Mobile.UITest.Scenes.User
{
    public class LoginScene
    {
        private IApp app;

        public AppQuery Login_SubmitButton(AppQuery c) => c.Marked("Login_SubmitButton");


        public LoginScene(IApp app)
        {
            this.app = app;
        }

        public void Play()
        {
            app.WaitForElement("Login_Title");

            app.Query(e => e.Marked("Login_UserNameEntry").Invoke("setText", "Test001"));
            app.Query(e => e.Marked("Login_PasswordEntry").Invoke("setText", "Password123"));

            app.Tap(Login_SubmitButton);
        }
    }
}
