using System;
using System.Collections.Generic;
using System.Linq;
using DeviceOrientation.Forms.Plugin.iOS;
using FFImageLoading.Forms.Platform;
using Firebase.CloudMessaging;
using Foundation;
using MediaManager;
using MediaManager.Forms.Platforms.iOS;
using OneTapMobile.Global;
using OneTapMobile.LocalDataBase;
using OneTapMobile.ViewModels;
using OneTapMobile.Views;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace OneTapMobile.iOS
{
    
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //


        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();
            //Firebase.Core.App.Configure();
            Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            Firebase.Core.App.Configure();
            DeviceOrientationImplementation.Init();
            CrossMediaManager.Current.Init();
            Rg.Plugins.Popup.Popup.Init();
            InitDatabaseTable.Root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            FacebookClientManager.Initialize(app, options);
            GoogleClientManager.Initialize();
            CachedImageRenderer.Init();
            CachedImageRenderer.InitImageSourceHandler();

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine("Push Setup successfully - " + granted);
                });
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            Messaging.SharedInstance.Delegate = this;
            if (UNUserNotificationCenter.Current != null)
            {
                UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
            }

            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);
            FacebookClientManager.OnActivated();
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            //GoogleClientManager.OnOpenUrl(app, url, options);
            //return FacebookClientManager.OpenUrl(app, url, options);
            //return base.OpenUrl(app, url,options);
            if (!Helper.IsFacebookAdLogin)
            {
                var uri_netfx = new Uri(url.AbsoluteString);
                if (Helper.PageName == "NoGoogleAdAccView")
                    NoGoogleAdAccView.Auth.OnPageLoading(uri_netfx);
                else
                {
                    ProfileViewModel.Auth.OnPageLoading(uri_netfx);
                }
            }
            //return true;
            var res = GoogleClientManager.OnOpenUrl(app, url, options);
             res = FacebookClientManager.OpenUrl(app, url, options);
            return res;
        }


        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return FacebookClientManager.OpenUrl(application, url, sourceApplication, annotation);
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            var mainPage = Xamarin.Forms.Application.Current.MainPage;
            if (mainPage.Navigation.NavigationStack.Count > 0 && mainPage.Navigation.NavigationStack.LastOrDefault() is FullScreenVideoView)
            {
                return UIInterfaceOrientationMask.AllButUpsideDown;
            }
            return UIInterfaceOrientationMask.Portrait;
        }

        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            System.Diagnostics.Debug.WriteLine($"FCM Token: {fcmToken}");
            Constant.DeviceToken = fcmToken;
        }


        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            // Monitor token generation: To be notified whenever the token is updated.
            Xamarin.Forms.Application.Current.Properties["Fcmtoken"] = fcmToken ?? "";
            Xamarin.Forms.Application.Current.SavePropertiesAsync();
            Constant.DeviceToken = fcmToken;
            // TODO: If necessary send token to application server.
            // Note: This callback is fired at each app startup and whenever a new token is generated.
        }

        // You'll need this method if you set "FirebaseAppDelegateProxyEnabled": NO in GoogleService-Info.plist
        //public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
        //{
        //	Messaging.SharedInstance.ApnsToken = deviceToken;
        //}



        //Notification tapping when the app is in background mode.
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;
            string alert = string.Empty;
            string title = string.Empty;

        }


    }
}
