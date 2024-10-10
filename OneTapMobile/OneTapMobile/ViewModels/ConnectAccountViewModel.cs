using OneTapMobile.Global;
using OneTapMobile.Views;
using OneTapMobile.Views.ErrorPage;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using OneTapMobile.Services;
using Plugin.FacebookClient;
using OneTapMobile.Popups;
using Rg.Plugins.Popup.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using OneTapMobile.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using OneTapMobile.LocalDataBase;
using OneTapMobile.LocalDataBases.DataBaseModel;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
//using Google.Ads.GoogleAds.Lib;
//using Google.Ads.GoogleAds.V10.Services;

namespace OneTapMobile.ViewModels
{
    public class ConnectAccountViewModel : BaseViewModel
    {
        #region Properties
        public INavigation nav;
        public int count = 0;

        private readonly ILoginService _loginService;

        public Helper helper;
        public Command AddFBAdCmd { get; set; }
        public Command AddGoogleAdCmd { get; set; }
        public Command SkipNowCmd { get; set; }

        private string _LoaderText = "Connecting";
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

        private bool _FbAdPageSelected = false;
        public bool FbAdPageSelected
        {
            get
            {
                return _FbAdPageSelected;
            }
            set
            {
                _FbAdPageSelected = value;
                OnPropertyChanged("FbAdPageSelected");
            }
        }

        private bool _ShowFbArrow = true;
        public bool ShowFbArrow
        {
            get
            {
                return _ShowFbArrow;
            }
            set
            {
                _ShowFbArrow = value;
                OnPropertyChanged("ShowFbArrow");
            }
        }

        private bool _GoAdPageSelected = false;

        public bool GoAdPageSelected
        {
            get
            {
                return _GoAdPageSelected;
            }
            set
            {
                _GoAdPageSelected = value;
                OnPropertyChanged("GoAdPageSelected");
            }
        }

        private bool _ShowGoArrow = true;

        public bool ShowGoArrow
        {
            get
            {
                return _ShowGoArrow;
            }
            set
            {
                _ShowGoArrow = value;
                OnPropertyChanged("ShowGoArrow");
            }
        }


        private bool _SkipNowVisible = false;
        public bool SkipNowVisible
        {
            get
            {
                return _SkipNowVisible;
            }
            set
            {
                _SkipNowVisible = value;
                OnPropertyChanged("SkipNowVisible");
            }
        }

        #endregion

        #region constructor
        public ConnectAccountViewModel()
        {
            this._loginService = DependencyService.Get<ILoginService>();
            AddFBAdCmd = new Command(() => AddFBAdMethod());
            AddGoogleAdCmd = new Command(() => AddGoogleAdmethod());
            SkipNowCmd = new Command(() => SkipNowCmdExeMethod());
        }
        #endregion

        #region Connecting to FB Account

        private void AddFBAdMethod()
        {
            IsBusy = true;
            LoaderText = "Connecting with Facebook Ads";

            if (Helper.facebookProfile == null)
            {
                Helper.IsFacebookAdLogin = true;
                _loginService.FaceBookLogin(async (response) =>
                {
                    if (response != null)
                    {
                        switch (response.Status)
                        {
                            case FacebookActionStatus.Completed:
                                await LoadData();
                                Helper.IsFacebookAdLogin = false;
                                break;
                            case FacebookActionStatus.Canceled:
                                CrossFacebookClient.Current.Logout();
                                IsBusy = false;
                                Helper.IsFacebookAdLogin = false;
                                break;
                            case FacebookActionStatus.Unauthorized:
                                popupnav = new UserDialogPopup("Unauthorized", response.Message, "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                                Helper.IsFacebookAdLogin = false;
                                break;
                            case FacebookActionStatus.Error:
                                popupnav = new UserDialogPopup("Error", response.Message, "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                                Helper.IsFacebookAdLogin = false;
                                break;
                        }
                    }
                });
            }
            else
            {
                Helper.IsFacebookAdLogin = false;
                GetFBAdAccMethod();
            }
        }
        public async Task LoadData()
        {
            try
            {
                var jsonData = await CrossFacebookClient.Current.RequestUserDataAsync
                (
                    new string[] { "id", "name", "email", "picture", "cover", "friends" }, new string[] { }
                );
                var data = JObject.Parse(jsonData.Data);
                var proRoot = new ProviderRoot();
                var FbProfile = new ProviderData()
                {
                    FullName = " ",
                    Picture = Convert.ToString(data["picture"]["data"]["url"]),
                    Email = data["email"].ToString(),
                    Token = CrossFacebookClient.Current.ActiveToken,
                    Id = data["id"].ToString()
                };
                Helper.facebookProfile = FbProfile;
                proRoot.provider_data = FbProfile;
                Helper.SavePropertyData("facebookProfile", JsonConvert.SerializeObject(Helper.facebookProfile));
                GetFBAdAccMethod();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                IsEnable = true;
                Debug.WriteLine(ex.Message);
            }
        }
        private async void GetFBAdAccMethod()
        {
            try
            {
                if (Constant.OneTapUserId != -1)
                {

                    FBAccIdListRequest fbAccIdListRequest = new FBAccIdListRequest
                    {
                        user_id = Convert.ToString(Constant.OneTapUserId),
                        fb_access_token = Helper.facebookProfile.Token,
                    };
                    string url = "user/facebook-ads-accounts";
                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(fbAccIdListRequest, url, true);
                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var FBAdResponseResult = JsonConvert.DeserializeObject<FBAccIdListResponse>(rest_result.response_body);
                            if (FBAdResponseResult.result != null)
                            {
                                Helper.SavePropertyData("FBcustomerAccList", rest_result.response_body);
                                //FBAccIdListPopup popupnav = new FBAccIdListPopup(FBAdResponseResult.result, true);
                                FBAccIdListPopup popupnav = new FBAccIdListPopup(FBAdResponseResult.result);
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsEnable = true;

                            }
                            else if (rest_result.status_code == 404)
                            {
                                IsBusy = false;
                                IsTap = false;
                                UserDialogPopup popupnav = new UserDialogPopup("Failed", "No facebook ads accounts found", "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                            }
                            else
                            {
                                UserDialogPopup popupnav = new UserDialogPopup("Failed", FBAdResponseResult.message + ". please try to check your default browser for facebook account and try again.", "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                                IsEnable = true;
                            }
                        }
                        else if (rest_result.status_code == 404)
                        {
                            IsBusy = false;
                            IsTap = false;
                            UserDialogPopup popupnav = new UserDialogPopup("Failed", "No facebook ads accounts found", "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                        }
                        else
                        {
                            UserDialogPopup popupnav = new UserDialogPopup("Failed", rest_result.ErrorMessage, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            IsBusy = false;
                            IsEnable = true;
                        }
                    }
                    else
                    {
                        UserDialogPopup popupnav = new UserDialogPopup("Failed", rest_result.ErrorMessage, "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                        IsEnable = true;
                    }
                }
                else
                {
                    UserDialogPopup popupnav = new UserDialogPopup("Failed", "OneTap Login Id Not Found.", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    IsBusy = false;
                    IsEnable = true;
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
                IsEnable = true;
            }
        }
        #endregion

        #region Add "Google Ads" Method
        private void AddGoogleAdmethod()
        {
            try
            {

                if (IsTap)
                    return;
                IsTap = true;
                IsBusy = true;
                LoaderText = "Connecting with Google Ads";
                Task.Delay(1000);
                //nav.PushAsync(new NoGoogleAdAccView());
                SetUpGoogleAdsLogin();
                IsTap = false;
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
                IsBusy = false;
            }

        }
        #endregion

        #region Skip For Now method
        private void SkipNowMethod()
        {
            try
            {
                IsBusy = true;
               // App.Current.MainPage = new NavigationPage(new DashBoard());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
            }

        }

        private void SkipNowCmdExeMethod()
        {
            try
            {
                IsBusy = true;
                App.Current.MainPage = new NavigationPage(new DashBoard());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
            }

        }

        #endregion

        #region Set Checked Icon For Fb
        public void FbAdChecked()
        {
            FbAdPageSelected = !Constant.FbAdPageadded;
            ShowFbArrow = !Constant.FbRightArrowVisible;
            SkipNowVisible = Constant.SkipNowVisisble;
            if (GoAdPageSelected && Constant.FbAdPageadded)
            {
                SkipNowMethod();
            }
        }
        public void GoogleAdChecked()
        {
            GoAdPageSelected = Constant.GoogleAdAdded;
            ShowGoArrow = !Constant.GoogleAdAdded;
            SkipNowVisible = Constant.SkipNowVisisble;
            if (GoAdPageSelected && Constant.FbAdPageadded)
            {
                SkipNowMethod();
            }
        }

        #endregion


        async void SetUpGoogleAdsLogin()
        {
            try
            {
                string google_first_Refresh_token = string.Empty;
                if (Application.Current.Properties.ContainsKey("LoginUserData"))
                {
                    var jsonUserData = Application.Current.Properties["LoginUserData"].ToString();
                    var userlogindata = JsonConvert.DeserializeObject<GlobalLoginDataModel>(jsonUserData);

                    if (userlogindata != null)
                        if (!string.IsNullOrWhiteSpace(userlogindata.result.first_google_refresh_token))
                            google_first_Refresh_token = userlogindata.result.first_google_refresh_token;
                }

                IsBusy = true;
                var url = new Uri(Constant.GoogleAuthUrl);
                var callbackUrl = new Uri("onetapmobile://");
                var authResult = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions
                {
                    Url = url,
                    CallbackUrl = callbackUrl,
                    PrefersEphemeralWebBrowserSession = true
                });
                if (authResult?.Properties.Count > 0)
                {
                    string error = authResult?.Properties["error"];
                    if (error.Equals("0"))
                    {
                        string rt = authResult?.Properties["refresh_token"];
                        if (!string.IsNullOrWhiteSpace(google_first_Refresh_token))
                            Helper.GoRefreshToken = google_first_Refresh_token;
                        else
                            Helper.GoRefreshToken = rt;

                        Device.BeginInvokeOnMainThread(async () =>
                        await GoogleAdAccountList(Helper.GoRefreshToken));

                        Helper.SavePropertyData("IsLoggedInGoogleAds",DateTime.Now.ToString());
                        Helper.SavePropertyData("GoRefreshToken", Helper.GoRefreshToken);
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
            catch (TaskCanceledException)
            {
                UserDialogPopup popupnav = new UserDialogPopup("message", "something went wrong, please try again !!", "Ok");
                await PopupNavigation.Instance.PushAsync(popupnav);
            }

            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex.Message);
            }

        }
        public async Task GoogleAdAccountList(string refreshToken)
        {
            try
            {
                if (refreshToken != null)
                {
                    IsBusy = true;
                    await Task.Delay(10);
                    GoogleAdRequestModel googleAdRequestModel = new GoogleAdRequestModel
                    {
                        user_id = Convert.ToString(Constant.OneTapUserId),
                        google_refresh_token = refreshToken,
                    };
                    string url = "user/google-ads-customers";
                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(googleAdRequestModel, url, true);
                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var googleAdResponseResult = JsonConvert.DeserializeObject<GoCustomerListResponse>(rest_result.response_body);

                            if (googleAdResponseResult != null)
                            {
                                if (googleAdResponseResult.status == true)
                                {
                                    if (googleAdResponseResult.result != null)
                                    {
                                        if (googleAdResponseResult.result.Count != 0)
                                        {
                                            Helper.SavePropertyData("google_ads_customers", rest_result.response_body);

                                            // save data to Database
                                            InitDatabaseTable db = new InitDatabaseTable();
                                            GoogleAdsCustomersDBModel saveGoogleAdsCustomers = new GoogleAdsCustomersDBModel();
                                            saveGoogleAdsCustomers.GoogleAdsCustomersData = rest_result.response_body;
                                            db.BulkDelete<GoogleAdsCustomersDBModel>();
                                            db.Save(saveGoogleAdsCustomers);

                                            ListPopup popupnav = new ListPopup(googleAdResponseResult.result);
                                            await PopupNavigation.Instance.PushAsync(popupnav);

                                        }
                                        else
                                        {
                                            UserDialogPopup popupnav = new UserDialogPopup("Message", "check default browser for correct Gmail account or This may not be an advertising account.", "Ok");
                                            await PopupNavigation.Instance.PushAsync(popupnav);
                                            IsBusy = false;
                                            IsTap = false;
                                        }
                                    }
                                    else
                                    {
                                        UserDialogPopup popupnav = new UserDialogPopup("Message", "check default browser for correct Gmail account or This may not be an advertising account.", "Ok");
                                        await PopupNavigation.Instance.PushAsync(popupnav);
                                        IsBusy = false;
                                        IsTap = false;
                                    }
                                }
                                else
                                {
                                    count++;
                                    if (count == 1)
                                    {
                                        if (!string.IsNullOrWhiteSpace(Helper.GoRefreshToken))
                                            await GoogleAdAccountList(Helper.GoRefreshToken);
                                        return;
                                    }
                                    IsBusy = false;
                                    UserDialogPopup popupnav = new UserDialogPopup("Failed", googleAdResponseResult.message, "Ok");
                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                }
                                //App.Current.MainPage = new NavigationPage(new DashBoard());
                                // need to add popup or a screen to select specific account from logged in Ad Account.
                            }
                        }
                        else
                        {
                            UserDialogPopup popupnav = new UserDialogPopup("Failed", rest_result.ErrorMessage, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);

                        }
                    }
                    else
                    {
                        IsBusy = false;
                        UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to login.", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }

                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex.Message);
            }

        }

        //public void GoogleAdLogin()
        //{
        ////    GoogleAdsClient client = new GoogleAdsClient();
        //    Google.Ads.GoogleAds.V11.Services.UserListService.UserListServiceClient userListService;
        //    var customerService = Google.Ads.GoogleAds.V11.Services.ListAccessibleCustomersRequest.Parser;
        //}

        #region Commands
        private Command _SettingsCmd;
        public Command SettingsCmd
        {
            get { return _SettingsCmd ?? (_SettingsCmd = new Command(() => SettingsMethod())); }
        }

        private void SettingsMethod()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                nav.PushAsync(new SettingsView());
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion
    }

}

