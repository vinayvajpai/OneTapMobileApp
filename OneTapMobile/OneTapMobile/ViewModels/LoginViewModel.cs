using Acr.UserDialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Services;
using OneTapMobile.Views;
using Plugin.FacebookClient;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
namespace OneTapMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        #region  properties

        private string _LoaderText;
        public string LoaderText
        {
            get
            {
                return _LoaderText;
            }
            set
            {
                _LoaderText = value;
                OnPropertyChanged("LoaderText");
            }
        }


        private bool _ButtonVisible;
        public bool ButtonVisible
        {
            get
            {
                return _ButtonVisible;
            }
            set
            {
                _ButtonVisible = value;
                OnPropertyChanged("ButtonVisible");
            }
        }

        #endregion

        #region Commands

        private readonly ILoginService _loginService;
        public Command SignInWithGoogleCmd { get; set; }
        public Command SignInWithFBCmd { get; set; }
        public Command SignInWithEmailCmd { get; set; }
        public ICommand SignInWithAppleCommand { get; set; }
        public INavigation nav { get; set; }
        public bool IsAppleSignInAvailable { get { return appleSignInService?.IsAvailable ?? false; } }
        public event EventHandler OnSignIn = delegate { };
        IAppleSignInService appleSignInService;

        #endregion

        #region Constructor
        public LoginViewModel()
        {
            this._loginService = DependencyService.Get<ILoginService>();

            MessagingCenter.Subscribe<object, bool>(this, "ChangeIsTapToFalse", (sender, arg) =>
            {
                if (!arg)
                {
                  IsTap = arg;
                  IsBusy = arg;
                }
            });

            SignInWithGoogleCmd = new Command(async () => await LoginGoogleAsync());
            SignInWithFBCmd = new Command(() => SignInWithFBMethod());
            SignInWithEmailCmd = new Command(() => SignInWithEmailMethod());
            appleSignInService = DependencyService.Get<IAppleSignInService>();
            SignInWithAppleCommand = new Command(OnAppleSignInRequest);
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    ButtonVisible = true;
                    break;
                case Device.Android:
                    ButtonVisible = false;
                    break;
                case Device.macOS:
                    ButtonVisible = true;
                    break;
                default:
                    ButtonVisible = true;
                    break;
            }
        }

        #endregion

        #region Email_Login
        private async void SignInWithEmailMethod()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                IsEnable = false;
                await nav.PushAsync(new EmailLoginView());
                IsTap = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsEnable = true;
                IsTap = false;
            }
        }

        #endregion

        #region SignIn With Apple
        async void OnAppleSignInRequest()
        {
            if (!Conn)
            {
                return;
            }

            IsBusy = true;
            IsEnable = false;
            LoaderText = "Connecting with Apple";
            try
            {
                await _loginService.AppleLogin(async (account) =>
                {
                    if (account != null)
                    {

                        await LoggedinViaApple(account);
                        //Helper.UserEmail = account.Email;
                        //App.Current.MainPage = new NavigationPage(new ConnectAccountView());
                        //IsEnable = true;
                        //System.Diagnostics.Debug.WriteLine($"Signed in!\n  Name: {account?.Name ?? string.Empty}\n  Email: {account?.Email ?? string.Empty}\n  UserId: {account?.UserId ?? string.Empty}");
                        //OnSignIn?.Invoke(this, default(EventArgs));

                    }
                    else
                    {
                        popupnav = new UserDialogPopup("Failed", "Unable to login.", "Ok");
                        IsBusy = false;
                        IsEnable = true;
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        return;
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
                IsEnable = true;
            }

        }

        private async Task LoggedinViaApple(AppleAccount account)
        {
            try
            {

                if (account != null)
                {
                    var proRoot = new AppleProviderRoot();
                    var AppleLoginData = new AppleProviderData()
                    {
                        Email = account.Email,
                        Name = account.Name,
                        RealUserStatus = account.RealUserStatus,
                        Token = account.Token,
                        UserId = account.UserId
                    };
                    proRoot.provider_data = AppleLoginData;

                    IsBusy = true;
                    await Task.Delay(10);
                    string url = "apple-login";
                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(proRoot, url, false);
                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var globalAccessResult = JsonConvert.DeserializeObject<GlobalLoginDataModel>(rest_result.response_body);
                            if (globalAccessResult.status == true)
                            {
                                Helper.SetLoginUserData(rest_result.response_body);
                                Constant.Token = globalAccessResult.result.token;
                                Constant.OneTapUserId = globalAccessResult.result.id;
                                Constant.IsLoggedOut = false;
                                Helper.UserEmail = globalAccessResult.result.email;
                                Helper.profileModel.ProfileImage = globalAccessResult.result.image;
                                if (!string.IsNullOrEmpty(globalAccessResult.result.name))
                                    Helper.profileModel.UserName = globalAccessResult.result.name;
                                else
                                    Helper.profileModel.UserName = "NA";
                                Constant.Role = globalAccessResult.result.role;
                                    await RegisterUserToken();
                                App.Current.MainPage = new NavigationPage(new ConnectAccountView());
                            }
                            else
                            {
                                IsBusy = false;
                                UserDialogPopup popupnav = new UserDialogPopup("Failed", globalAccessResult.message, "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                            }
                        }
                        IsBusy = false;
                    }
                    else
                    {
                        IsBusy = false;
                        UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to login", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }

                }
                else
                {
                    IsBusy = false;
                    UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to login", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to login", "Ok");
                await PopupNavigation.Instance.PushAsync(popupnav);
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Facebook_Login
        private async void SignInWithFBMethod()
        {
            try
            {
                if (!Conn)
                {
                    return;
                }
                IsEnable = false;
                IsBusy = true;
                LoaderText = "Connecting with Facebook";
                await _loginService.FaceBookLogin(async (response) =>
                {
                    if (response != null)
                    {
                        switch (response.Status)
                        {
                            case FacebookActionStatus.Completed:
                                await LoadData();
                                break;
                            case FacebookActionStatus.Canceled:
                                CrossFacebookClient.Current.Logout();
                                IsBusy = false;
                                break;
                            case FacebookActionStatus.Unauthorized:
                                popupnav = new UserDialogPopup("Unauthorized", response.Message, "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                                break;
                            case FacebookActionStatus.Error:
                                popupnav = new UserDialogPopup("Error", response.Message, "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                                break;
                        }
                    }
                    else
                    {
                        popupnav = new UserDialogPopup("Failed", "Unable to login.", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                        IsEnable = true;
                    }
                });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                popupnav = new UserDialogPopup("Failed", "Unable to respond.", "Ok");
                await PopupNavigation.Instance.PushAsync(popupnav);
                IsBusy = false;
            }
            finally
            {
                IsEnable = true;
            }
        }
        /// <summary>
        /// Method to get facebook user login details
        /// </summary>
        /// <returns></returns>
        public async Task LoadData()
        {
            try
            {
                var jsonData = await CrossFacebookClient.Current.RequestUserDataAsync
                (
                    new string[] { "id", "name", "email", "picture", "cover", "friends"}, new string[] { }
                );
                var data = JObject.Parse(jsonData.Data);
                var proRoot = new ProviderRoot();
                var FbProfile = new ProviderData()
                {
                    FullName = data["name"].ToString(),
                    Picture = Convert.ToString(data["picture"]["data"]["url"]),
                    Email = data["email"].ToString(),
                    Token = CrossFacebookClient.Current.ActiveToken,
                    Id = data["id"].ToString()
                };
                Helper.facebookProfile = FbProfile;
                proRoot.provider_data = FbProfile;
                string url = "facebook-login";

                Rest_ResponseModel rest_result = await WebService.WebService.PostData(proRoot, url, false);
                if (rest_result != null)
                {
                    if (rest_result.status_code == 200)
                    {
                        var globalAccessResult = JsonConvert.DeserializeObject<GlobalLoginDataModel>(rest_result.response_body);
                        if (globalAccessResult.status)
                        {
                            Helper.SetLoginUserData(rest_result.response_body);
                            Constant.Token = globalAccessResult.result.token;
                            Constant.OneTapUserId = globalAccessResult.result.id;
                            Constant.IsLoggedOut = false;
                            Helper.UserEmail = globalAccessResult.result.email;
                            Helper.profileModel.ProfileImage = globalAccessResult.result.image;
                            if (!string.IsNullOrEmpty(globalAccessResult.result.name))
                                Helper.profileModel.UserName = globalAccessResult.result.name;
                            else
                                Helper.profileModel.UserName = "NA";
                            Helper.profileModel.fb_ad_account_id = globalAccessResult.result.facebook_id;
                            Constant.Role = globalAccessResult.result.role;
                            Helper.SavePropertyData("facebookProfile", JsonConvert.SerializeObject(Helper.facebookProfile));
                            await RegisterUserToken();
                            App.Current.MainPage = new NavigationPage(new ConnectAccountView());
                            IsBusy = false;
                        }
                        else
                        {
                            IsBusy = false;
                            UserDialogPopup popupnav = new UserDialogPopup("Failed", globalAccessResult.message, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                IsEnable = true;
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
                IsEnable = true;
            }
        }
        #endregion

        #region Google_Login

        async Task LoginGoogleAsync()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                if (!Conn)
                {
                    return;
                }
                IsEnable = false;
                IsBusy = true;
                LoaderText = "Connecting with Google";
                await _loginService.GoogleLogin(async (NetworkAuthData obj) =>
                {
                    if (obj != null)
                    {
                        var googleProvidedRoot = new GoogleProviderRoot();
                        var googleProviderData = new GoogleProviderData()
                        {
                            Email = obj.Email,
                            FamilyName = obj.Name,
                            Id = obj.Id,
                            GivenName = obj.Name,
                            Name = obj.Name,
                            Picture = obj.Picture
                        };
                        googleProvidedRoot.provider_data = googleProviderData;
                        Helper.GoogleProfile = googleProviderData;
                        string url = "google-login";
                        Rest_ResponseModel rest_result = await WebService.WebService.PostData(googleProvidedRoot, url, false);
                        if (rest_result != null)
                        {
                            if (rest_result.status_code == 200)
                            {
                                var globalAccessResult = JsonConvert.DeserializeObject<GlobalLoginDataModel>(rest_result.response_body);
                                if (globalAccessResult.status)
                                {
                                    Helper.SetLoginUserData(rest_result.response_body);
                                    Constant.Token = globalAccessResult.result.token;
                                    Constant.OneTapUserId = globalAccessResult.result.id;
                                    Constant.IsLoggedOut = false;
                                    Helper.UserEmail = globalAccessResult.result.email;
                                    Helper.profileModel.ProfileImage = globalAccessResult.result.image;
                                    if (!string.IsNullOrEmpty(globalAccessResult.result.name))
                                        Helper.profileModel.UserName = globalAccessResult.result.name;
                                    else
                                        Helper.profileModel.UserName = "NA";
                                    await RegisterUserToken();
                                    App.Current.MainPage = new NavigationPage(new ConnectAccountView());
                                }
                                else
                                {
                                    IsBusy = false;
                                    UserDialogPopup popupnav = new UserDialogPopup("Failed", globalAccessResult.message, "Ok");
                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                }
                                IsBusy = false;
                            }
                            else
                            {
                                popupnav = new UserDialogPopup("Failed", rest_result.ErrorMessage, "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                            }
                        }
                        else
                        {
                            popupnav = new UserDialogPopup("Failed", "Unable to login.", "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            IsBusy = false;
                        }
                    }
                    else
                    {
                        popupnav = new UserDialogPopup("Failed", "Unable to login.", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                    }
                });
                IsEnable = true;
                IsTap = false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                IsBusy = false;
                IsEnable = true;
                IsTap = false;
            }
        }
        #endregion

        #region register for a push notification token
        async Task RegisterUserToken()
        {
            try
            {
                if (Constant.DeviceToken != null && Constant.OneTapUserId != 0)
                {
                    NotificationRequestModel notificationRequestModel = new NotificationRequestModel
                    {
                        user_id = Convert.ToString(Constant.OneTapUserId),
                        device_token = Constant.DeviceToken
                    };


                    string url = "user/add-device-token";
                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(notificationRequestModel, url, true);
                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var NotificationResponseModel = JsonConvert.DeserializeObject<NotificationResponseModel>(rest_result.response_body);
                            
                            if(NotificationResponseModel != null)
                            {
                                Debug.WriteLine("Token Registered for notification");
                            }
                            else
                            {
                                Debug.WriteLine("not Registered for notification");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }
        #endregion
    }
}




