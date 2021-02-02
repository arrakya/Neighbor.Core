using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Neighbor.Mobile.UITest
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
            AppQuery registerNavLink(AppQuery c) => c.Marked("Register_NavLink");
            AppQuery registerNserNameEntry(AppQuery c) => c.Marked("Register_UserNameEntry");
            AppQuery registerPasswordEntry(AppQuery c) => c.Marked("Register_PasswordEntry");            
            AppQuery register_RePasswordEntry(AppQuery c) => c.Marked("Register_RePasswordEntry");
            AppQuery emailEntry(AppQuery c) => c.Marked("Register_EmailEntry");
            AppQuery phoneEntry(AppQuery c) => c.Marked("Register_PhoneEntry");
            AppQuery houseNumberEntry(AppQuery c) => c.Marked("Register_HouseNumberEntry");
            AppQuery submitEntry(AppQuery c) => c.Marked("Register_SubmitEntry");

            AppQuery enterPINEntry(AppQuery c) => c.Marked("EnterPIN_PINEntry");
            AppQuery enterPINButton(AppQuery c) => c.Marked("EnterPIN_SubmitButton");

            AppQuery loginUserNameEntry(AppQuery c) => c.Marked("Login_UserNameEntry");
            AppQuery loginPasswordEntry(AppQuery c) => c.Marked("Login_PasswordEntry");
            AppQuery loginButton(AppQuery c) => c.Marked("Login_SubmitButton");

            Dictionary<string, string> registerDic = new Dictionary<string, string>
            {
                { "Register_UserNameEntry", "Test001" },                
                { "Register_PasswordEntry", "Password123" },
                { "Register_RePasswordEntry", "Password123" },
                { "Register_EmailEntry", "test001@gmail.com" },
                { "Register_PhoneEntry", "0912345678" },
                { "Register_HouseNumberEntry", "89/86" },
                { "EnterPIN_PINEntry", "123456" },
                { "Login_UserNameEntry", "Test001" },
                { "Login_PasswordEntry", "Password123" }
            };

            app.Tap(registerNavLink);

            app.WaitForElement(registerNserNameEntry);            

            app.EnterText(registerNserNameEntry, registerDic["Register_UserNameEntry"]);
            app.EnterText(registerPasswordEntry, registerDic["Register_PasswordEntry"]);
            app.EnterText(register_RePasswordEntry, registerDic["Register_RePasswordEntry"]);
            app.EnterText(emailEntry, registerDic["Register_EmailEntry"]);
            app.Back();
            app.EnterText(phoneEntry, registerDic["Register_PhoneEntry"]);
            app.Back();
            app.EnterText(houseNumberEntry, registerDic["Register_HouseNumberEntry"]);
            app.Back();

            app.Tap(submitEntry);

            app.WaitForElement(enterPINEntry);

            app.EnterText(enterPINEntry, registerDic["EnterPIN_PINEntry"]);

            app.Tap(enterPINButton);

            app.WaitForElement(loginUserNameEntry);

            app.EnterText(loginUserNameEntry, registerDic["Login_UserNameEntry"]);
            app.EnterText(loginPasswordEntry, registerDic["Login_PasswordEntry"]);

            app.Tap(loginButton);
        }
    }
}
