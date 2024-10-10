using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Acr.UserDialogs;
using Plugin.FacebookClient;
using Android.Content;
using static Android.Content.PM.PackageManager;
using Android.Util;
using Plugin.GoogleClient;
//using Android.Gms.Auth.Api.SignIn;
//using Android.Gms.Auth.Api;
using Rg.Plugins.Popup.Services;
using FFImageLoading.Forms.Platform;
using MediaManager;
using System.IO;
using System.Threading.Tasks;
using Java.Security;
using Xamarin.Forms;
using OneTapMobile.Views;
using DeviceOrientation.Forms.Plugin.Droid;
using AndroidX.AppCompat.App;
using OneTapMobile.Interface;
using OneTapMobile.Services;
using OneTapMobile.Authentication;
using OneTapMobile.Global;
using OneTapMobile.ViewModels;
using Android.Widget;
using Plugin.CurrentActivity;
using OneTapMobile.LocalDataBase;

namespace OneTapMobile.Droid
{
    [Activity(Label = "OneTap", Icon = "@drawable/OneTapLogo", Theme = "@style/MyTheme.Splash", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, LaunchMode = LaunchMode.SingleTop,Exported = true)]
    [IntentFilter(new[] { Xamarin.Essentials.Platform.Intent.ActionAppAction },
        Categories = new[] { Intent.CategoryDefault })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, Interface.IGoogleAuthenticationDelegate
    {
        public static MainActivity mainActivity;
        internal static readonly string CHANNEL_ID = "default";
        internal static readonly int NOTIFICATION_ID = 100;
        public const string TAG = "Splash FirebaseMsgService";
        internal static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mainActivity = this;
            Instance = this;
            try
            {
                //allowing the device to change the screen orientation based on the rotation
                MessagingCenter.Subscribe<FullScreenVideoView>(this, "allowLandScapePortrait", sender =>
                {
                    RequestedOrientation = ScreenOrientation.Landscape;
                });

                //during page close setting back to portrait
                MessagingCenter.Subscribe<FullScreenVideoView>(this, "preventLandScape", sender =>
                {
                    RequestedOrientation = ScreenOrientation.Portrait;
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            //Window.AddFlags(WindowManagerFlags.Fullscreen);
            //Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);
            try
            {

                PackageInfo info = PackageManager.GetPackageInfo(
                            "com.onetapsocial.onetap",
                            PackageInfoFlags.Signatures);
                foreach (var signature in info.Signatures)
                {
                    MessageDigest md = MessageDigest.GetInstance("SHA");
                    md.Update(signature.ToByteArray());
                    Log.Debug("Key hashes:", Base64.EncodeToString(md.Digest(), Base64Flags.Default));
                    var s = Base64.EncodeToString(md.Digest(), Base64Flags.Default);
                }
            }
            catch (NameNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (NoSuchAlgorithmException e)
            {
                Console.WriteLine(e.Message);
            }
            InitDatabaseTable.Root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            DeviceOrientationImplementation.Init();
            CrossMediaManager.Current.Init(this);
            Rg.Plugins.Popup.Popup.Init(this);
            CachedImageRenderer.Init(true);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            FacebookClientManager.Initialize(this);
            GoogleClientManager.Initialize(this);
            UserDialogs.Init(this);
            LoadApplication(new App());
            PrintHashKey(this);
        }

        // Field, property, and method for Picture Picker
        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            //if (intent != null)
            //{
            //    Android.Net.Uri uri_android = intent.Data;
            //    var uri_netfx = new Uri(uri_android.ToString());
            //    // Send the URI to the Authenticator for continuation
            //    NoGoogleAdAccView.Auth?.OnPageLoading(uri_netfx);
            //}

            Controls.ImageCropper.Platform.Droid.OnActivityResult(requestCode, resultCode, intent);
            FacebookClientManager.OnActivityResult(requestCode, resultCode, intent);
            GoogleClientManager.OnAuthCompleted(requestCode, resultCode, intent);
            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    if (Helper.PageName == "NoGoogleAdAccView")
                    {
                        var uri_netfx = new Uri(uri.ToString());
                        // Send the URI to the Authenticator for continuation
                        NoGoogleAdAccView.Auth?.OnPageLoading(uri_netfx);
                    }
                    else
                    {
                        Stream stream = ContentResolver.OpenInputStream(uri);
                        // Set the Stream as the completion of the Task
                        PickImageTaskCompletionSource.SetResult(stream);
                    }
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }

        public async override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
                await PopupNavigation.Instance.PopAsync();

        }

        public override void OnConfigurationChanged(global::Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            DeviceOrientationImplementation.NotifyOrientationChange(newConfig);
        }

        public async void OnAuthenticationCompleted(GoogleOAuthToken token)
        {
            //Retrieve the user's email address
            var googleService = new GoogleService();
            var email = await googleService.GetEmailAsync(token.TokenType, token.AccessToken);

            // Display it on the UI
            Console.WriteLine("Hello Google Ad User");
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            new Android.App.AlertDialog.Builder(this)
               .SetTitle(message)
               .SetMessage(exception?.ToString())
               .Show();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Xamarin.Essentials.Platform.OnNewIntent(intent);
        }

        public void OnAuthenticationCanceled()
        {
            new Android.App.AlertDialog.Builder(this)
                .SetTitle("Authentication canceled")
                .SetMessage("You didn't completed the authentication process")
                .Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }


        void PrintHashKey(Context pContext)
        {
            try
            {
                PackageInfo info = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.Signatures);
                foreach (var signature in info.Signatures)
                {
                    MessageDigest md = MessageDigest.GetInstance("SHA");
                    md.Update(signature.ToByteArray());

                    System.Diagnostics.Debug.WriteLine(BitConverter.ToString(md.Digest()).Replace("-", ":"));
                }
            }
            catch (NoSuchAlgorithmException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }

    [Activity(Label = "GoogleAuthInterceptor",Exported = true)]
    [IntentFilter(actions: new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
              DataSchemes = new[]
              {
                  // First part of the redirect url (Package name)
                  "com.onetapsocial.onetap"
              },
              DataPaths = new[]
              {
                  // Second part of the redirect url (Path)
                  "/oauth2redirect"
              })]
    public class GoogleAuthInterceptor : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Android.Net.Uri uri_android = Intent.Data;

            // Convert Android Url to C#/netxf/BCL System.Uri
            var uri_netfx = new Uri(uri_android.ToString());

            // Send the URI to the Authenticator for continuation
            if (!Helper.IsFacebookAdLogin)
            {
                if (Helper.PageName == "NoGoogleAdAccView")
                    NoGoogleAdAccView.Auth?.OnPageLoading(uri_netfx);
                else
                    ProfileViewModel.Auth?.OnPageLoading(uri_netfx);
            }
            Finish();
        }
    }

    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
        Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
        DataScheme = "onetapmobile")]
    public class WebAuthenticationCallbackActivity : Xamarin.Essentials.WebAuthenticatorCallbackActivity
    {

    }

}