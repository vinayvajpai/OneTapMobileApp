using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OneTapMobile.Droid.Service;
using OneTapMobile.Interface;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBarColorService))]
namespace OneTapMobile.Droid.Service
{
    public class StatusBarColorService : IStatusBarColorService
    {
        public void SetDefaultTheme()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentWindow = GetCurrentWindow();
                    currentWindow.DecorView.SystemUiVisibility = 0;
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.ParseColor("#0A92D6"));
                });
            }
        }
        public void SetTheme(string ColorCode)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentWindow = GetCurrentWindow();
                    currentWindow.DecorView.SystemUiVisibility = 0;
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.ParseColor(ColorCode));
                    currentWindow.SetNavigationBarColor(Android.Graphics.Color.ParseColor(ColorCode));
                });
            }
        }
        public void SetDarkTheme()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentWindow = GetCurrentWindow();
                    currentWindow.DecorView.SystemUiVisibility = 0;
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.DarkCyan);
                });
            }
        }

        public void SetLightTheme()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentWindow = GetCurrentWindow();
                    currentWindow.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.LightGreen);
                });
            }
        }

        Window GetCurrentWindow()
        {
            var window = CrossCurrentActivity.Current.Activity.Window;

            // clear FLAG_TRANSLUCENT_STATUS flag:
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);

            // add FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS flag to the window
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

            return window;
        }

        WindowManagerFlags _originalFlags;

        #region IStatusBar implementation

        public void HideStatusBar()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var activity = (Activity)Forms.Context;
                var attrs = activity.Window.Attributes;
                _originalFlags = attrs.Flags;
                attrs.Flags |= Android.Views.WindowManagerFlags.Fullscreen;
                activity.Window.Attributes = attrs;
            });
        }

        public void ShowStatusBar()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var activity = (Activity)Forms.Context;
                var attrs = activity.Window.Attributes;
                attrs.Flags = _originalFlags;
                activity.Window.Attributes = attrs;
            });
        }

        #endregion
    }
}