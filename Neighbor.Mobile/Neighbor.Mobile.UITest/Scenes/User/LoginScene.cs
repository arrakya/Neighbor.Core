using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Neighbor.Mobile.UITest.Scenes.User
{
    public class LoginScene
    {
        private IApp app;

        public AppQuery Login_Title(AppQuery c) => c.Marked("Login_Title");
        public AppQuery Login_UserNameEntry(AppQuery c) => c.Marked("Login_UserNameEntry");
        public AppQuery Login_PasswordEntry(AppQuery c) => c.Marked("Login_PasswordEntry");
        public AppQuery Login_SubmitButton(AppQuery c) => c.Marked("Login_SubmitButton");


        public LoginScene(IApp app)
        {
            this.app = app;
        }

        public void Play()
        {
            app.WaitForElement(Login_Title);

            app.EnterText(Login_UserNameEntry, "Test001");
            app.EnterText(Login_PasswordEntry, "Password123");

            app.Tap(Login_SubmitButton);
        }
    }
}
