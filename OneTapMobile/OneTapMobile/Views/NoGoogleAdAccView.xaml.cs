using Newtonsoft.Json;
using OneTapMobile.Authentication;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Services;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WebAuthenticator = Xamarin.Essentials.WebAuthenticator;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoGoogleAdAccView : ContentPage
    {
        private const string GOOGLE_ADS_API_SCOPE = "https://www.googleapis.com/auth/adwords";
        public static OAuth2Authenticator Auth;
        private Interface.IGoogleAuthenticationDelegate _authenticationDelegate;
        private bool IsTap = false;
        public Thickness SafeAreaSpacing
        {
            get { return DependencyService.Get<INotchService>().HasNotch() ? new Thickness(10, 55) : new Thickness(10, 20); }
        }

        public NoGoogleAdAccView()
        {
            InitializeComponent();
        }
        private void TryAnotherAccountTapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new ListPopup(Helper.GocustomerAccList, true));
            //SetUpGoogleAdsLogin();
        }

        protected override void OnAppearing()
        {
            ContainerStack.Margin = SafeAreaSpacing;
            IsTap = false;
        }

        private void SkipNowTapped(object sender, EventArgs e)
        {
            try
            {
                if (IsTap)
                    return;
                else
                {
                    var nav = new NavigationPage(new DashBoard());
                    App.Current.MainPage = nav;
                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (IsTap)
                    return;
                else
                {
                    Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        async private void CreateGoogleAcc_Tapped(object sender, EventArgs e)
        {
            if (IsTap)
                return;
            else
            {
                if (Helper.GocustomerAccList != null)
                {
                    if (Helper.GocustomerAccList.Count > 0)
                    {
                        var objGoogleAcc = Helper.GocustomerAccList.Where(g => g.is_manager == true).FirstOrDefault();
                        if (objGoogleAcc != null)
                        {
                           await this.Navigation.PushAsync(new TryAnotherGoogleAdAccView());
                        }
                        else
                        {
                            IsTap = false;
                            UserDialogPopup popupnav = new UserDialogPopup("Failed", "Google Ads manager account not found for this user, Please create manager account at: https://ads.google.com/signup?sf=manager", "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                        }
                    }
                }

                //SetUpGoogleAdsLogin();
            }
        }

        async void SetUpGoogleAdsLogin()
        {
            try
            {
                GoLoginLoader.IsVisible = true;
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
                        Helper.GoRefreshToken = rt;
                        Device.BeginInvokeOnMainThread(async() =>
                        await GoogleAdAccountList(rt));
                    }
                    else
                    {
                        GoLoginLoader.IsVisible = false;
                        UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to login", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }
                }
                else
                {
                    GoLoginLoader.IsVisible = false;
                    UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to login", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                }


                //Console.Write("Enter the client ID: ");
                //string clientId = "566011961148-316hc3opclcf8go0f731uqtsmdr5h1pv.apps.googleusercontent.com";//"566011961148-ak6pd2gh7ahdb35mlergkg7h8gkg0daa.apps.googleusercontent.com";
                //string redirectURL = "com.googleusercontent.apps.566011961148-316hc3opclcf8go0f731uqtsmdr5h1pv:/oauth2callback";
                //if (Device.RuntimePlatform == Device.Android)
                //{
                //    clientId = "566011961148-qp77k6421vvbajt6p91qc724tkgfe16r.apps.googleusercontent.com";
                //    redirectURL = "com.onetapsocial.onetap:/oauth2redirect";
                //}

                //// Accept the client ID from user.
                //Console.Write("Enter the client secret: ");
                //string clientSecret = "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";
                //var authenticator = new OAuth2Authenticator(clientId, null, "email",
                //                new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                //                new Uri(redirectURL),
                //                new Uri("https://oauth2.googleapis.com/token"),
                //                isUsingNativeUI: true);
                //Auth = authenticator;
                //authenticator.Completed += Authenticator_Completed;
                //authenticator.Error += Authenticator_Error;
                //var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                //Auth = authenticator;
                //presenter.Login(authenticator);
            }
            catch (Exception ex)
            {
                IsTap = false;
                GoLoginLoader.IsVisible = false;
                Debug.WriteLine(ex.Message);
            }

        }

        private void Authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {

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
                    //_authenticationDelegate.OnAuthenticationCompleted(token);
                    //Device.BeginInvokeOnMainThread(() =>
                    ////GoogleAdAccountList(token));
                }
                else
                {
                    GoLoginLoader.IsVisible = false;
                    //_authenticationDelegate.OnAuthenticationCanceled();
                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                GoLoginLoader.IsVisible = false;
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
                            GoLoginLoader.IsVisible = false;
                            if (googleAdResponseResult != null)
                            {
                                if (googleAdResponseResult.status == true)
                                {
                                    if (googleAdResponseResult.result != null)
                                    {
                                        if (googleAdResponseResult.result.Count != 0)
                                        {
                                            ListPopup popupnav = new ListPopup(googleAdResponseResult.result, true);
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
                                    //App.Current.MainPage = new NavigationPage(new DashBoard());
                                    // need to add popup or a screen to select specific account from logged in Ad Account.
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
                                IsTap = false;
                                IsBusy = false;
                                UserDialogPopup popupnav = new UserDialogPopup("Failed", rest_result.ErrorMessage, "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                            }
                        }
                        else
                        {
                            IsTap = false;
                            IsBusy = false;
                            GoLoginLoader.IsVisible = false;
                            UserDialogPopup popupnav = new UserDialogPopup("Failed", rest_result.ErrorMessage, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);

                        }
                    }
                    else
                    {
                        IsTap = false;
                        IsBusy = false;
                        GoLoginLoader.IsVisible = false;
                        UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to login.", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }

                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }
    }
}

