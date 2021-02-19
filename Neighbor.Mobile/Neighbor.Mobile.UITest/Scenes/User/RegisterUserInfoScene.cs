using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Neighbor.Mobile.UITest.Scenes.User
{
    public class RegisterUserInfoScene
    {
        private IApp app;

        public AppQuery RegisterUserInfoSubmitButton(AppQuery c) => c.Marked("RegisterUserInfoSubmitButton");


        public RegisterUserInfoScene(IApp app)
        {
            this.app = app;
        }

        public void Play()
        {
            app.WaitForElement("RegisterUserInfoPhoneNumber");
            app.Query(e => e.Marked("RegisterUserInfoPhoneNumber").Invoke("setText", "0987654321"));
            app.Query(e => e.Marked("RegisterUserInfoEmail").Invoke("setText", "Test001@test01.com"));
            app.Query(e => e.Marked("RegisterUserInfoHouseNumber").Invoke("setText", "89/86"));

            app.Tap(RegisterUserInfoSubmitButton);
        }
    }
}
