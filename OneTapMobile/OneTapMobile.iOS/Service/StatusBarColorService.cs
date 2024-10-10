using Foundation;
using OneTapMobile.Interface;
using OneTapMobile.iOS.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBarColorService))]
namespace OneTapMobile.iOS.Service
{
    public class StatusBarColorService : IStatusBarColorService
    {

        public void SetDefaultTheme()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                            //var statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
                        UIView statusBar;
                        if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                        {
                            int tag = 123; // Customize this tag as you want
                            UIWindow window = UIApplication.SharedApplication.Windows.FirstOrDefault();
                            statusBar = window.ViewWithTag(tag);
                                //if (statusBar == null)
                                //{
                            statusBar = new UIView(UIApplication.SharedApplication.StatusBarFrame);
                            statusBar.Tag = tag;
                            statusBar.BackgroundColor = FromHEX("#0A92D6"); // Customize the view

                            window.AddSubview(statusBar);
                                //}
                        }
                        else
                        {
                            statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
                        }

                        if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
                        {
                            statusBar.BackgroundColor = FromHEX("#0A92D6");
                            statusBar.TintColor = FromHEX("#0A92D6");
                        }

                            //UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, false);
                        GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("status bar color service failed.");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("status bar color service failed.");
            }
        }

        public void SetTheme(string ColorCode)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                    //var statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
                UIView statusBarnew;
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    int tag = 1234; // Customize this tag as you want
                    UIWindow window = UIApplication.SharedApplication.Windows.FirstOrDefault();
                    statusBarnew = window.ViewWithTag(tag);
                        //if (statusBarnew == null)
                        //{
                    statusBarnew = new UIView(UIApplication.SharedApplication.StatusBarFrame);
                    statusBarnew.Tag = tag;
                    statusBarnew.BackgroundColor = FromHEX(ColorCode);

                    window.AddSubview(statusBarnew);
                        //}
                }
                else
                {
                    statusBarnew = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
                }

                if (statusBarnew.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
                {
                    statusBarnew.BackgroundColor = FromHEX(ColorCode);
                    statusBarnew.TintColor = FromHEX(ColorCode);
                }

                    //UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, false);
                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
            });

            //GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
        }

        public void SetDarkTheme()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.BlackOpaque, false);
                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
            });
        }

        public void SetLightTheme()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
            });
        }

        UIViewController GetCurrentViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;
            return vc;
        }

        public static UIColor FromHEX(string hex)
        {
            int r = 0, g = 0, b = 0, a = 0;

            if (hex.Contains("#"))
                hex = hex.Replace("#", "");

            switch (hex.Length)
            {
                case 2:
                    r = int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
                    g = int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
                    b = int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
                    a = 255;
                    break;
                case 3:
                    r = int.Parse(hex.Substring(0, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    g = int.Parse(hex.Substring(1, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    b = int.Parse(hex.Substring(2, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    a = 255;
                    break;
                case 4:
                    r = int.Parse(hex.Substring(0, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    g = int.Parse(hex.Substring(1, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    b = int.Parse(hex.Substring(2, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    a = int.Parse(hex.Substring(3, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                    break;
                case 6:
                    r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    a = 255;
                    break;
                case 8:
                    r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    a = int.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    break;
            }

            return UIColor.FromRGBA(r, g, b, a);
        }


        #region IStatusBar implementation

        public void HideStatusBar()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.StatusBarHidden = true;
            });
        }

        public void ShowStatusBar()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.StatusBarHidden = false;
            });
        }

        #endregion

    }
}