using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Neighbor.Mobile.UITest.Scenes.User
{
    public class RegisterUserInfoScene
    {
        private IApp app;

        public AppQuery RegisterUserInfoPhoneNumber(AppQuery c) => c.Marked("RegisterUserInfoPhoneNumber");
        public AppQuery RegisterUserInfoEmail(AppQuery c) => c.Marked("RegisterUserInfoEmail");
        public AppQuery RegisterUserInfoHouseNumber(AppQuery c) => c.Marked("RegisterUserInfoHouseNumber");
        public AppQuery RegisterUserInfoSubmitButton(AppQuery c) => c.Marked("RegisterUserInfoSubmitButton");


        public RegisterUserInfoScene(IApp app)
        {
            this.app = app;
        }

        public void Play()
        {
            app.EnterText(RegisterUserInfoPhoneNumber, "0987654321");
            app.EnterText(RegisterUserInfoEmail, "Test001@test01.com");
            app.EnterText(RegisterUserInfoHouseNumber, "89/86");

            app.Tap(RegisterUserInfoSubmitButton);
        }
    }
}
