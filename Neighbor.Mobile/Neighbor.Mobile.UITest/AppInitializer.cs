using NUnit.Framework;
using Xamarin.UITest;

namespace Neighbor.Mobile.UITest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                ConfigureApp.Android.DevicePort(5555);
#if DEBUG
                return ConfigureApp.Android.ApkFile(@"C:\Users\arrak\Downloads\com.companyname.neighbor.mobile.apk").StartApp();
#else
                var apkPath = TestContext.Parameters["apkPath"];
                return ConfigureApp.Android.ApkFile(apkPath).StartApp();
#endif
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}