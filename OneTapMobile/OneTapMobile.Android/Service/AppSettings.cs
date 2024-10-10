using Android.App;
using Android.Content;
using OneTapMobile.Droid.Service;
using OneTapMobile.Interface;

[assembly: Xamarin.Forms.Dependency(typeof(AppSettings))]

namespace OneTapMobile.Droid.Service
{
    public class AppSettings : ISettingsHelper
    {
        public void AppLicationsettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            var uri = Android.Net.Uri.FromParts("package", "com.onetapsocial.onetap", null);
            intent.SetData(uri);
            Application.Context.StartActivity(intent);
        }

    }
}