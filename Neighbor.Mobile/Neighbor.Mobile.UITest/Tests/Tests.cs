using Neighbor.Mobile.UITest.Scenes.User;
using NUnit.Framework;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Neighbor.Mobile.UITest.Tests
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void WelcomeTextIsDisplayed()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Groove Ville"));
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void Register()
        {
            var tcScene = new TermAndConditionScene(app);
            var registerAccountScene = new RegisterAccountScene(app);
            var registerUserInfoScene = new RegisterUserInfoScene(app);
            var enterPINScene = new EnterPINScene(app);
            var loginScene = new LoginScene(app);
            AppQuery registerNavLink(AppQuery c) => c.Marked("Register_NavLink");

            app.Tap(registerNavLink);

            tcScene.Play();
            registerAccountScene.Play();
            registerUserInfoScene.Play();
            enterPINScene.Play();
            loginScene.Play();
        }
    }
}
