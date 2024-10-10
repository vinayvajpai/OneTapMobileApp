using System;
using UserNotifications;

namespace OneTapMobile.iOS
{
    internal class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {

        public UserNotificationCenterDelegate()
        {
        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            //var body = notification.Request.Content.UserInfo;
            //NSDictionary aps = body.ObjectForKey(new NSString("aps")) as NSDictionary;
            //string alert = string.Empty;
            //string title = string.Empty;
            //if (aps.ContainsKey(new NSString("alert")))
            //{
            //    var alertData = aps.ObjectForKey(new NSString("alert")) as NSDictionary;
            //    if (alertData.ContainsKey(new NSString("body")))
            //        alert = (alertData.ObjectForKey(new NSString("body")) as NSString).ToString();
            //    if (alertData.ContainsKey(new NSString("title")))
            //        title = (alertData.ObjectForKey(new NSString("title")) as NSString).ToString();

            //    //LogInformation(nameof(DidReceiveRemoteNotification), userInfo);
            //    //completionHandler(UIBackgroundFetchResult.NewData);
            //    SaveNewNotification(alert, title);
            //    completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
            //}
        }

        public static void SaveNewNotification(string message, string title)
        {
            try
            {
                //NotificationModel showNotification = new NotificationModel();
                //showNotification.Notification = message;
                //showNotification.Title = title;
                //showNotification.Createdate = DateTime.Now;
                //App.Database.SaveNewNotificationAsync(showNotification);
            }
            catch (Exception ex)
            {

            }
        }

    }
}
