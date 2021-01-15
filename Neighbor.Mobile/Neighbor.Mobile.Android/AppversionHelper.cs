using Android.Content.PM;
using Neighbor.Mobile.Droid;
using Neighbor.Mobile.NativeHelpers;

[assembly: Xamarin.Forms.Dependency(typeof(AppversionHelper))]
namespace Neighbor.Mobile.Droid
{
    public class AppversionHelper : IAppVersionHelper
    {
        private readonly PackageInfo _appInfo;

        public string AppVersion
        {
            get
            {
                return _appInfo.VersionName;
            }
        }

        public AppversionHelper()
        {
            var context = Android.App.Application.Context;
            _appInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
        }
    }
}