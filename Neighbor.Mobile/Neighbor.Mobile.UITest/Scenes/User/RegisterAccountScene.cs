using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Neighbor.Mobile.UITest.Scenes.User
{
    public class RegisterAccountScene
    {
        private IApp app;

        public AppQuery RegisterContactUserName(AppQuery c) => c.Marked("RegisterContactUserName");
        public AppQuery RegisterContactPassword(AppQuery c) => c.Marked("RegisterContactPassword");
        public AppQuery RegisterContactConfirmPassword(AppQuery c) => c.Marked("RegisterContactConfirmPassword");
        public AppQuery RegisterAccountSubmitButton(AppQuery c) => c.Marked("RegisterAccountSubmitButton");


        public RegisterAccountScene(IApp app)
        {
            this.app = app;
        }

        public void Play()
        {
            app.EnterText(RegisterContactUserName, "Test001");
            app.EnterText(RegisterContactPassword, "Password123");
            app.EnterText(RegisterContactConfirmPassword, "Password123");

            app.Tap(RegisterAccountSubmitButton);
        }
    }
}
