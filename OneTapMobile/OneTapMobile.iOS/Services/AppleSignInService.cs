using System;
using System.Threading.Tasks;
using AuthenticationServices;
using Firebase.CloudMessaging;
using Foundation;
using OneTapMobile.Interface;
using OneTapMobile.iOS.Services;
using UIKit;
using UserNotifications;
using WebKit;

[assembly: Xamarin.Forms.Dependency(typeof(AppleSignInService))]
[assembly: Xamarin.Forms.Dependency(typeof(NotchService))]
[assembly: Xamarin.Forms.Dependency(typeof(ClearCookies))]
[assembly: Xamarin.Forms.Dependency(typeof(HandleNotification))]
namespace OneTapMobile.iOS.Services
{
    public class AppleSignInService : NSObject, IAppleSignInService, IASAuthorizationControllerDelegate, IASAuthorizationControllerPresentationContextProviding
    {
        public bool IsAvailable => UIDevice.CurrentDevice.CheckSystemVersion(13, 0);
        TaskCompletionSource<ASAuthorizationAppleIdCredential> tcsCredential;
        public async Task<AppleSignInCredentialState> GetCredentialStateAsync(string userId)
        {
            var appleIdProvider = new ASAuthorizationAppleIdProvider();
            var credentialState = await appleIdProvider.GetCredentialStateAsync(userId);
            switch (credentialState)
            {
                case ASAuthorizationAppleIdProviderCredentialState.Authorized:
                    // The Apple ID credential is valid.
                    return AppleSignInCredentialState.Authorized;
                case ASAuthorizationAppleIdProviderCredentialState.Revoked:
                    // The Apple ID credential is revoked.
                    return AppleSignInCredentialState.Revoked;
                case ASAuthorizationAppleIdProviderCredentialState.NotFound:
                    // No credential was found, so show the sign-in UI.
                    return AppleSignInCredentialState.NotFound;
                default:
                    return AppleSignInCredentialState.Unknown;
            }
        }
        public async Task<AppleAccount> SignInAsync()
        {
            var appleIdProvider = new ASAuthorizationAppleIdProvider();
            var request = appleIdProvider.CreateRequest();
            request.RequestedScopes = new[] { ASAuthorizationScope.Email, ASAuthorizationScope.FullName };
            var authorizationController = new ASAuthorizationController(new[] { request });
            authorizationController.Delegate = this;
            authorizationController.PresentationContextProvider = this;
            authorizationController.PerformRequests();
            tcsCredential = new TaskCompletionSource<ASAuthorizationAppleIdCredential>();
            var creds = await tcsCredential.Task;
            if (creds == null)
                return null;
            var appleAccount = new AppleAccount();
            appleAccount.Token = new NSString(creds.IdentityToken, NSStringEncoding.UTF8).ToString();
            appleAccount.Email = creds.Email==null?creds.User.Replace(".","")+"@gmail.com":creds.Email;
            appleAccount.UserId = creds.User;
            appleAccount.Name = NSPersonNameComponentsFormatter.GetLocalizedString(creds.FullName, NSPersonNameComponentsFormatterStyle.Default, NSPersonNameComponentsFormatterOptions.Phonetic);
            if (string.IsNullOrEmpty(appleAccount.Name))
                appleAccount.Name = creds.User;
            appleAccount.RealUserStatus = creds.RealUserStatus.ToString();
            return appleAccount;
        }
        #region IASAuthorizationController Delegate
        [Export("authorizationController:didCompleteWithAuthorization:")]
        public void DidComplete(ASAuthorizationController controller, ASAuthorization authorization)
        {
            var creds = authorization.GetCredential<ASAuthorizationAppleIdCredential>();
            tcsCredential?.TrySetResult(creds);
        }
        [Export("authorizationController:didCompleteWithError:")]
        public void DidComplete(ASAuthorizationController controller, NSError error)
        {
            // Handle error
            tcsCredential?.TrySetResult(null);
            Console.WriteLine(error);
        }
        #endregion
        #region IASAuthorizationControllerPresentation Context Providing
        public UIWindow GetPresentationAnchor(ASAuthorizationController controller)
        {
            return UIApplication.SharedApplication.KeyWindow;
        }
        #endregion
    }
    public class NotchService : INotchService
    {
        public bool HasNotch()
        {
            return UIDevice.CurrentDevice.IsNotchExist();
        }
    }
    static class CheckNotchClass
    {
        public static bool IsNotchExist(this UIDevice device)
        {
            var bottom = UIApplication.SharedApplication.KeyWindow?.SafeAreaInsets.Bottom ?? 0;
            return bottom > 0;
        }
    }
    public class ClearCookies : IClearCookies
    {
        public void ClearAllCookies()
        {
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);

            Clear();
        }

        public void Clear()
        {
            NSHttpCookieStorage.SharedStorage.RemoveCookiesSinceDate(NSDate.DistantPast);

            WKWebsiteDataStore.DefaultDataStore.FetchDataRecordsOfTypes(WKWebsiteDataStore.AllWebsiteDataTypes, (NSArray records) => {

                for (nuint i = 0; i < records.Count; i++)
                {
                    var record = records.GetItem<WKWebsiteDataRecord>(i);
                    WKWebsiteDataRecord[] recordArray = new WKWebsiteDataRecord[record.DataTypes.Count];
                    WKWebsiteDataStore.DefaultDataStore.RemoveDataOfTypes(record.DataTypes, NSDate.DistantPast, () => { });
                }

            });

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
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // For iOS 10 display notification (sent via APNS)
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
        }
        public bool registeredForNotifications()
        {
            UIUserNotificationType types = UIApplication.SharedApplication.CurrentUserNotificationSettings.Types;
            if (types.HasFlag(UIUserNotificationType.Alert))
            {
                return true;
            }
            return false;
        }
    }
}
