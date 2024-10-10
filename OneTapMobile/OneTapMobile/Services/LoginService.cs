using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
namespace OneTapMobile.Services
{
    public class LoginService : ILoginService
    {
        IAppleSignInService appleSignInService;
        IFacebookClient _facebookService = CrossFacebookClient.Current;
        string[] permisions = new string[] { "email", "public_profile" ,"ads_read" , "pages_show_list" , "pages_read_engagement" , "pages_manage_ads", "pages_read_user_content", "read_insights" , "pages_manage_posts" };
        string[] permisions2 = new string[] { "ads_management"};
        //string[] permisions = new string[] { "email", "public_profile"};
        // string[] permisions = new string[] { "email", "pages_show_list", "ads_management", "ads_read", "pages_read_engagement", "pages_read_user_content", "pages_manage_ads", "pages_manage_posts", "pages_manage_engagement", "public_profile" };
        public FacebookProfile FbProfile { get; set; }
        IGoogleClientManager _googleService = CrossGoogleClient.Current;
        IOAuth2Service _oAuth2Service;
        EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate;

        #region Login Service Constructor
        public LoginService()
        { }
        #endregion

        #region Apple Login Service
        async public Task AppleLogin(Action<AppleAccount> response)
        {
            appleSignInService = DependencyService.Get<IAppleSignInService>();
            var account = await appleSignInService.SignInAsync();
            if (account != null)
            {
                response.Invoke(account);
            }
            else
            {
                response.Invoke(null);
            }
        }
        #endregion

        #region Facebook Login service
        async public Task FaceBookLogin(Action<FacebookResponse<bool>> response)
        {
            try
            {
                CrossFacebookClient.Current.Logout();
                DependencyService.Get<IClearCookies>().ClearAllCookies();
                FacebookResponse<bool> Readres = await CrossFacebookClient.Current.LoginAsync(permisions);
                FacebookResponse<bool> Publishres = await CrossFacebookClient.Current.LoginAsync(permisions2, FacebookPermissionType.Publish);

                if (Publishres != null && Readres != null)
                {
                    if (Publishres.Status == FacebookActionStatus.Completed && Readres.Status == FacebookActionStatus.Completed)
                    {
                        if (response != null)
                        {
                            response.Invoke(Readres);
                        }
                        else
                        {
                            MessagingCenter.Send<object, bool>(this, "ChangeIsTapToFalse", false);
                            response.Invoke(null);
                        }
                    }
                    else if(Publishres.Status == FacebookActionStatus.Canceled)
                    {
                        MessagingCenter.Send<object, bool>(this, "ChangeIsTapToFalse", false);
                        response.Invoke(null);
                    }
                    else
                    {
                        MessagingCenter.Send<object, bool>(this, "ChangeIsTapToFalse", false);
                        var popupnav = new UserDialogPopup(Constant.PopupMessage, "All permissions required for the app please try again !!", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }
                }
                else
                {
                    MessagingCenter.Send<object, bool>(this, "ChangeIsTapToFalse", false);
                    var popupnav = new UserDialogPopup(Constant.PopupMessage, "All permissions required for the app please try again !!", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Google Login Service
        async public Task GoogleLogin(Action<NetworkAuthData> response)
        {
            _googleService.Logout();
            userLoginDelegate = (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
            {
                switch (e.Status)
                {
                    case GoogleActionStatus.Completed:
                        var googleUserString = JsonConvert.SerializeObject(e.Data);
                        Debug.WriteLine($"Google Logged in succesfully: {googleUserString}");
                        var socialLoginData = new NetworkAuthData
                        {
                            Id = e.Data.Id,
                            Logo = "ic_google",
                            Foreground = "#000000",
                            Background = "#F8F8F8",
                            Picture = e.Data.Picture.AbsoluteUri,
                            Email = e.Data?.Email,
                            Name = e.Data.Name,
                        };
                        response.Invoke(socialLoginData);
                        break;
                    case GoogleActionStatus.Canceled:
                        {
                        }
                        break;
                    case GoogleActionStatus.Error:
                        response.Invoke(null);
                        break;
                    case GoogleActionStatus.Unauthorized:
                        response.Invoke(null);
                        break;
                }
                _googleService.OnLogin -= userLoginDelegate;
            };
            _googleService.OnLogin += userLoginDelegate;
            await _googleService.LoginAsync();
        }
        #endregion
    }
}
