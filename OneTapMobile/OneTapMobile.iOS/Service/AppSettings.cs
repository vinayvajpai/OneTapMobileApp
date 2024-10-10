using Foundation;
using OneTapMobile.Interface;
using OneTapMobile.iOS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(AppSettings))]

namespace OneTapMobile.iOS.Service
{
    public class AppSettings : ISettingsHelper
    {
        public void AppLicationsettings()
        {
            var url = new NSUrl($"app-settings:com.onetapsocial.onetap");
            UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}