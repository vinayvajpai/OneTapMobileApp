using System;
using Android.Webkit;
using OneTapMobile.Droid.Service;
using OneTapMobile.Interface;
using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.App;
using Application = Android.App.Application;
using Xamarin.Forms;
[assembly: Xamarin.Forms.Dependency(typeof(NotchService))]
[assembly: Xamarin.Forms.Dependency(typeof(ClearCookies))]
[assembly: Xamarin.Forms.Dependency(typeof(HandleNotification))]
namespace OneTapMobile.Droid.Service
{
    public class NotchService : INotchService
    {
        public bool HasNotch()
        {
            return IsHasNotch;
        }
        private const int __NOTCH_SIZE_THRESHHOLD = 40; //dp
        /// <summary>
        /// Device has a notched display (or not)
        /// </summary>
        public bool IsHasNotch
        {
            get
            {
                // The 'solution' is to measure the size of the status bar; on devices without a notch, it returns 24dp.. on devices with a notch, it should be > __NOTCH_SIZE_THRESHHOLD (tested on emulator / S10)
                int id = MainActivity.mainActivity.Resources.GetIdentifier("status_bar_height", "dimen", "android");
                if (id > 0)
                {
                    int height = MainActivity.mainActivity.Resources.GetDimensionPixelSize(id);
                    if (pxToDp(height) > __NOTCH_SIZE_THRESHHOLD)
                        return true;
                }
                return false;
            }
        }

        /// Helper method to convert PX to DP
        private int pxToDp(int px)
        {
            return (int)(px / Android.App.Application.Context.Resources.DisplayMetrics.Density);
        }
    }
    public class ClearCookies : IClearCookies
    {
        public void ClearAllCookies()
        {
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
        }
    }
    public class HandleNotification : IHandleNotification
    {
        public void CancelPush(int id)
        {
            throw new NotImplementedException();
        }
        public void EnablePush()
        {
            CreateNotificationChannel();
        }
        public bool registeredForNotifications()
        {
            var nm = NotificationManagerCompat.From(Android.App.Application.Context);
            bool enabled = nm.AreNotificationsEnabled();
            return enabled;
        }
        void CreateNotificationChannel()
        {
            // Notification channels are new as of "Oreo".
            // There is no need to create a notification channel on older versions of Android.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(MainActivity.CHANNEL_ID, MainActivity.TAG, NotificationImportance.Default)
                {
                    Description = "Firebase Cloud Messages appear in this channel"
                };
                ///Forms.Context.GetSystemService(Context.NotificationService) as NotificationManager;
                var notificationManager = (NotificationManager)MainActivity.mainActivity.GetSystemService(Context.NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
    }
}
