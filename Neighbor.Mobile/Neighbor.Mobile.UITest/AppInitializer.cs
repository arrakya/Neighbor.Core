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
                return ConfigureApp.Android.InstalledApp("com.companyname.neighbor.mobile").StartApp();
#else
                ConfigureApp.Android.DevicePort(5554);
                var apkPath = TestContext.Parameters["apkPath"];
                return ConfigureApp.Android.ApkFile(apkPath).StartApp();
#endif
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}