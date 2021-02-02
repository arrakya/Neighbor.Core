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

#if DEBUG
                ConfigureApp.Android.DevicePort(5554);
                return ConfigureApp.Android.ApkFile(@"C:\Users\arrak\Apk\com.companyname.neighbor.mobile.apk").StartApp();
#else
                ConfigureApp.Android.DevicePort(5555);
                var apkPath = TestContext.Parameters["apkPath"];
                return ConfigureApp.Android.ApkFile(apkPath).StartApp();
#endif
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}