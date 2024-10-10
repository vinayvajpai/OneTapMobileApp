using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using OneTapMobile.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneTapMobile.Droid
{
        [Service(Exported=true)]
        [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
        public class FirebaseService : FirebaseMessagingService
        {
            int count = 0;
            public override void OnMessageReceived(RemoteMessage message)
            {
                base.OnMessageReceived(message);

                try
                {
                    string messageBody = string.Empty;

                    if (message.GetNotification() != null)
                    {
                        messageBody = message.GetNotification().Body;
                    }
                    else
                    {
                        if (!message.Data.GetEnumerator().MoveNext())
                            SendNotification(message.GetNotification().Title, message.GetNotification().Body, null);
                        else
                            SendNotification(message.Data);

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            }

            public override void OnNewToken(string token)
            {
                // TODO: save token instance locally, or log if desired

                System.Diagnostics.Debug.WriteLine("Token - " + token);
                Constant.DeviceToken = token;
                Helper.SavePropertyData("DeviceToken", token);
            //SendRegistrationToServer1(token);
        }

            private void SendNotification(IDictionary<string, string> data)
            {
                string title, body, serializedNews;
                data.TryGetValue("title", out title);
                data.TryGetValue("message", out body);
                data.TryGetValue("SerializedNews", out serializedNews);
                SendNotification(title, body, serializedNews);
            }

            private void SendNotification(string title, string body, string news)
            {
              

                var intent = new Intent(this, typeof(MainActivity));

                // here i put data in intent and start    mainactivity where I show data in new Page
                intent.AddFlags(ActivityFlags.ClearTop);

                intent.PutExtra(title, body);


           
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
              
                if (Helper.GetLoginUserData() != null)
                {
                    if (!string.IsNullOrEmpty(Helper.NoOfMessages))
                    {
                        count = Convert.ToInt32(Helper.NoOfMessages) + 1;
                    }
                    else
                    {
                        count = 1;
                    }
                }


                var notificationBuilder = new NotificationCompat.Builder(this, App.NotificationHubName)
                    .SetContentTitle(title)
                    .SetContentText(body)
                    .SetStyle(new NotificationCompat.BigTextStyle().BigText(body))
                    .SetAutoCancel(true)
                    .SetNumber(count)
                    .SetContentIntent(pendingIntent);
                notificationBuilder.SetColor(Color.ParseColor("#0C89CC"));
                notificationBuilder.SetColorized(true);
            //notificationBuilder.SetSmallIcon(Resource.Drawable.OneTapLogo);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    notificationBuilder.SetChannelId(App.NotificationHubName);
                }

                var notificationManager = NotificationManager.FromContext(this);
                notificationManager.Notify(0, notificationBuilder.Build());
            }

        }
    }