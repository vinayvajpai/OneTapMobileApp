using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Services;
using OneTapMobile.Views;
using Plugin.FacebookClient;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using OneTapMobile.Authentication;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Auth;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using OneTapMobile.LocalDataBase;
using OneTapMobile.LocalDataBases.DataBaseModel;
using WebAuthenticator = Xamarin.Essentials.WebAuthenticator;

namespace OneTapMobile.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private const string GOOGLE_ADS_API_SCOPE = "https://www.googleapis.com/auth/adwords";
        public static OAuth2Authenticator Auth;
        public int count = 0; 
        private Interface.IGoogleAuthenticationDelegate _authenticationDelegate;

        #region constructor
        public ProfileViewModel()
        {
            try
            {
                if (Helper.facebookProfile != null)
                {
                    FBAccountId = Helper.profileModel.fb_ad_account_id;
                    FBUserName = Helper.facebookProfile.FullName;
                }
                if(Helper.FBcustomerAccList != null)
                {

                }

                if (Helper.GoogleProfile != null)
                {
                    //GoogleUserName = Helper.GoogleProfile.Name;
                }
                if (!string.IsNullOrWhiteSpace(Helper.profileModel.google_ad_customer_id))
                {
                    GoogleCustomerId = Helper.profileModel.google_ad_customer_id;
                }

                if (Helper.profileModel != null)
                    profilePic = Helper.profileModel.ProfileImage !=null ? Helper.profileModel.ProfileImage: "ProfilePic";

                //UserName = Helper.profileModel.UserName;


                MessagingCenter.Subscribe<object, bool>(this, "ChangeIsTapToFalse", (sender, arg) =>
                {
                    if (!arg)
                    {
                        IsTap = arg;
                        IsBusy = arg;
                    }
                });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            this._loginService = DependencyService.Get<ILoginService>();

        }
        #endregion

        #region Properties

        public INavigation nav;

        private readonly ILoginService _loginService;

        private ImageSource profilePic = "ProfilePic";
        public ImageSource ProfilePic { get => profilePic; set => SetProperty(ref profilePic, value); }

        private string loaderText = "please Wait...";
        public string LoaderText { get => loaderText; set => SetProperty(ref loaderText, value); }

        private string _FBArrowImage = "ArrowForward";
        public string FBArrowImage { get => _FBArrowImage; set => SetProperty(ref _FBArrowImage, value); }

        private string _GoArrowImage = "ArrowForward";
        public string GoArrowImage { get => _GoArrowImage; set => SetProperty(ref _GoArrowImage, value); }


        private bool fBConnectOption;
        public bool FBConnectOption { get => fBConnectOption; set => SetProperty(ref fBConnectOption, value); }


        private bool gConnectOption;
        public bool GConnectOption { get => gConnectOption; set => SetProperty(ref gConnectOption, value); }


        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }


        private string userLocation = "Not Found";
        public string UserLocation { get => userLocation; set => SetProperty(ref userLocation, value); }


        private string totalSpent = string.Concat("$", Helper.profileModel.amount_spent);
        public string TotalSpent { get => totalSpent; set => SetProperty(ref totalSpent, value); }


        private string fBSpent = string.Concat("$", Helper.profileModel.facebook_spent);
        public string FBSpent { get => fBSpent; set => SetProperty(ref fBSpent, value); }


        private string googleSpent = string.Concat("$", Helper.profileModel.google_spent);
        public string GoogleSpent { get => googleSpent; set => SetProperty(ref googleSpent, value); }


        private string fBUserName;
        public string FBUserName { get => fBUserName; set => SetProperty(ref fBUserName, value); }


        private string googleUserName;
        public string GoogleUserName { get => googleUserName; set => SetProperty(ref googleUserName, value); }


        private string fBAccountId;
        public string FBAccountId
        {
            get
            {
                return fBAccountId;
            }
            set
            {
                fBAccountId = value;
                if (!string.IsNullOrWhiteSpace(value) && value != "No Account")
                {
                    fBAccountId = Helper.profileModel.fb_ad_account_id;
                    FBDownArrow = true;
                }
                else
                {
                    fBAccountId = "No Account";
                    FBDownArrow = false;
                }

                FBConnectOption = !FBDownArrow;
                OnPropertyChanged("FBAccountId");

            }
        }

        private string googleCustomerId;
        public string GoogleCustomerId
        {
            get
            {
                return googleCustomerId;
            }
            set
            {
                googleCustomerId = value;
                if (!string.IsNullOrWhiteSpace(value) && value != "No Account")
                {
                    googleCustomerId = Helper.profileModel.google_ad_customer_id;
                    GoogleDownArrow = true;
                }
                else
                {
                    googleCustomerId = "No Account";
                    GoogleDownArrow = false;
                }
                GConnectOption = !GoogleDownArrow;

                OnPropertyChanged("GoogleCustomerId");
            }
        }

        private ObservableCollection<GoCustomerListResult> _AdAccountsList = new ObservableCollection<GoCustomerListResult>();
        public ObservableCollection<GoCustomerListResult> AdAccountsList
        {
            get { return _AdAccountsList; }
            set { _AdAccountsList = value; OnPropertyChanged("AdAccountsList"); }
        }


        private GoCustomerListResult _SelectedAccount;
        public GoCustomerListResult SelectedAccount
        {
            get
            {
                return _SelectedAccount;
            }
            set
            {

                _SelectedAccount = value;
                OnPropertyChanged("SelectedAccount");
                if (SelectedAccount != null)
                {
                    AdAccountsList.ToList().Where(p => p.customer_id == SelectedAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                    AdAccountsList.ToList().Where(p => p.customer_id != SelectedAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                }
            }
        }

        private bool googleDownArrow;

        public bool GoogleDownArrow { get => googleDownArrow; set => SetProperty(ref googleDownArrow, value); }

        private bool fBDownArrow;

        public bool FBDownArrow { get => fBDownArrow; set => SetProperty(ref fBDownArrow, value); }


        private bool showFBAdAccNo = false;

        public bool ShowFBAdAccNo { get => showFBAdAccNo; set => SetProperty(ref showFBAdAccNo, value); }


        private bool showGoogleAdAccNo = false;

        public bool ShowGoogleAdAccNo { get => showGoogleAdAccNo; set => SetProperty(ref showGoogleAdAccNo, value); }

        #endregion

        #region commands

        private Command _SettingsCmd;
        public Command SettingsCmd
        {
            get { return _SettingsCmd ?? (_SettingsCmd = new Command(() => SettingsMethod())); }
        }

        private ICommand backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new Command(PerformBackBtn);
                }
                return backCommand;
            }
        }

        private Command fbConnectCmd;

        public ICommand FbConnectCmd
        {
            get
            {
                if (fbConnectCmd == null)
                {
                    fbConnectCmd = new Command(PerformFbConnectCmd);
                }

                return fbConnectCmd;
            }
        }

        private Command googleConnectCmd;

        public ICommand GoogleConnectCmd
        {
            get
            {
                if (googleConnectCmd == null)
                {
                    googleConnectCmd = new Command(PerformGoogleConnectCmd);
                }

                return googleConnectCmd;
            }
        }

        private Command fbDownArrowCmd;

        public ICommand FbDownArrowCmd
        {
            get
            {
                if (fbDownArrowCmd == null)
                {
                    fbDownArrowCmd = new Command(PerformFbDownArrowCmd);
                }

                return fbDownArrowCmd;
            }
        }



        private Command googleDownArrowCmd;

        public ICommand GoogleDownArrowCmd
        {
            get
            {
                if (googleDownArrowCmd == null)
                {
                    googleDownArrowCmd = new Command(PerformGoogleDownArrowCmd);
                }

                return googleDownArrowCmd;
            }
        }


        #endregion

        #region Open Settings Page Method

        private void SettingsMethod()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                nav.PushAsync(new SettingsView());
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
            }
        }

        #endregion

        #region back button pressed command

        private void PerformBackBtn(object obj)
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                nav.PopAsync();
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Methods

        private void PerformFbDownArrowCmd()
        {
          
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                if (Helper.FBcustomerAccList.Count != 0)
                    PopupNavigation.Instance.PushAsync(new FBAccIdListPopup(Helper.FBcustomerAccList));
                else
                    AddFBAdMethod();


                FBArrowImage = "ArrowForward";
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private void PerformGoogleDownArrowCmd()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                this.nav.PushAsync(new NoGoogleAdAccView());
                GoArrowImage = "ArrowForward";
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        public void PerformGoogleConnectCmd()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                SetUpGoogleAdsLogin();
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private async void PerformFbConnectCmd()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                AddFBAdMethod();
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Connecting to FB Account

        private void AddFBAdMethod()
        {
            IsBusy = true;
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
                });
            }
            else
            {
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
                    FullName = data["name"].ToString(),
                    Picture = Convert.ToString(data["picture"]["data"]["url"]),
                    Email = data["email"].ToString(),
                    Token = CrossFacebookClient.Current.ActiveToken,
                    Id = data["id"].ToString()
                };
                Helper.facebookProfile = FbProfile;
                proRoot.provider_data = FbProfile;
                Helper.SavePropertyData("facebookProfile", JsonConvert.SerializeObject(Helper.facebookProfile));

                GetFBAdAccMethod();
                //IsBusy = false;
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
                            Helper.SavePropertyData("FBcustomerAccList", rest_result.response_body);
                            FBAccIdListPopup popupnav = new FBAccIdListPopup(FBAdResponseResult.result);
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            IsTap = false;
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
                            IsBusy = false;
                            IsTap = false;
                            UserDialogPopup popupnav = new UserDialogPopup("Failed", rest_result.ErrorMessage, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                        }
                    }
                    else
                    {
                        IsBusy = false;
                        IsTap = false;
                        UserDialogPopup popupnav = new UserDialogPopup("Failed", rest_result.ErrorMessage, "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }
                }
                else
                {
                    IsBusy = false;
                    IsTap = false;
                    UserDialogPopup popupnav = new UserDialogPopup("Failed", "OneTap Login Id Not Found.", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region connecting to google account
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
                Helper.PageName = "ProfileView";

                var url = new Uri(Constant.GoogleAuthUrl);
                var callbackUrl = new Uri("onetapmobile://");
                var authResult = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions
                {
                    Url = url,
                    CallbackUrl = callbackUrl,
                    //PrefersEphemeralWebBrowserSession = true
                });

                if (authResult?.Properties.Count > 0)
                {
                    string error = authResult?.Properties["error"];

                    if (error.Equals("0"))
                    {
                        string rt = authResult?.Properties["refresh_token"];

                        //Helper.GoRefreshToken = rt;

                        if (string.IsNullOrEmpty(rt) && !string.IsNullOrEmpty(google_first_Refresh_token))
                            Helper.GoRefreshToken = google_first_Refresh_token;
                        else
                            Helper.GoRefreshToken = rt;

                        Device.BeginInvokeOnMainThread(async () =>
                        await GoogleAdAccountList(Helper.GoRefreshToken));

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

            catch (TaskCanceledException ex)
            {
                //
                //UserDialogPopup popupnav = new UserDialogPopup("message", "something went wrong, please try again !!", "Ok");
               // await PopupNavigation.Instance.PushAsync(popupnav);
                IsBusy = false;
                IsTap = false;
            }
            
            catch (Exception ex)
            {
                IsBusy = false;
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }

        private void Authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            IsBusy = false;
        }

        private void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            try
            {
                if (e.IsAuthenticated)
                {
                    var token = new GoogleOAuthToken
                    {
                        TokenType = e.Account.Properties["token_type"],
                        AccessToken = e.Account.Properties["access_token"],
                        expires_in = e.Account.Properties["expires_in"],
                        refresh_token = e.Account.Properties["refresh_token"],
                        scope = e.Account.Properties["scope"],
                        id_token = e.Account.Properties["id_token"]
                    };

                    Helper.GoRefreshToken = token.refresh_token;
                    Helper.SavePropertyData("GoRefreshToken", Helper.GoRefreshToken);
                    //_authenticationDelegate.OnAuthenticationCompleted(token);
                    //Device.BeginInvokeOnMainThread(() =>
                    //GoogleAdAccountList(token));
                }
                else
                {
                    IsBusy = false;
                    //_authenticationDelegate.OnAuthenticationCanceled();
                }
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
                                if (googleAdResponseResult.result != null)
                                {
                                    if (googleAdResponseResult.result.Count != 0)
                                    {
                                        Helper.SavePropertyData("google_ads_customers", rest_result.response_body);

                                        ListPopup popupnav = new ListPopup(googleAdResponseResult.result);
                                        await PopupNavigation.Instance.PushAsync(popupnav);

                                        // users accounts list save to db
                                        InitDatabaseTable db = new InitDatabaseTable();
                                        GoogleAdsCustomersDBModel saveGoogleAdsCustomers = new GoogleAdsCustomersDBModel();
                                        saveGoogleAdsCustomers.GoogleAdsCustomersData = rest_result.response_body;
                                        db.BulkDelete<GoogleAdsCustomersDBModel>();
                                        db.Save(saveGoogleAdsCustomers);
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
                                    IsBusy = false;
                                    IsTap = false;
                                    UserDialogPopup popupnav;
                                    if (string.IsNullOrEmpty(googleAdResponseResult.message)) { 
                                         popupnav = new UserDialogPopup("Message", "check default browser for correct Gmail account or This may not be an advertising account.", "Ok");
                                    }
                                    else
                                        popupnav = new UserDialogPopup("Message", googleAdResponseResult.message , "Ok");

                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                }
                            }
                            else
                            {
                                IsBusy = false;
                                IsTap = false;
                                UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to login.", "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
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
                            IsTap = false;
                            UserDialogPopup popupnav = new UserDialogPopup("Failed", rest_result.ErrorMessage, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                        }
                    }
                    else
                    {
                        IsBusy = false;
                        IsTap = false;
                        UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to login.", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }

                }
                IsTap = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }

        #endregion
    }
}
