using OneTapMobile.Global;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using Plugin.Connectivity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OneTapMobile.Services;
using System.Diagnostics;
using OneTapMobile.Interface;
using Newtonsoft.Json;
using OneTapMobile.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OneTapMobile.LocalDataBase;
using OneTapMobile.LocalDataBases.DataBaseModel;
using System.Linq;
using Rg.Plugins.Popup.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace OneTapMobile
{
    public partial class App : Application
    {
        public const string NotificationHubName = "OneTapNotificationHub";

        public static object NoOfMessages { get; set; }

        public App()
        {
            Xamarin.Forms.Device.SetFlags(new string[] { "MediaElement_Experimental" });
            InitializeingServices();
            InitializeComponent();
         
            
            var statusBarStyleManager = DependencyService.Get<IStatusBarColorService>();
            statusBarStyleManager.SetTheme("#AC47ED");
            if (Application.Current.Properties.ContainsKey("LoginUserData"))
            {
                if (Application.Current.Properties.ContainsKey("profileModel"))
                {
                    Helper.profileModel = JsonConvert.DeserializeObject<UserProfileModel>(Convert.ToString(Application.Current.Properties["profileModel"]));
                }
                if (Application.Current.Properties.ContainsKey("facebookProfile"))
                {
                    Helper.facebookProfile = JsonConvert.DeserializeObject<ProviderData>(Convert.ToString(Application.Current.Properties["facebookProfile"]));

                    if(Helper.profileModel != null)
                    Helper.facebookProfile.Id = Helper.profileModel.fb_ad_account_id;
                }
                if (Application.Current.Properties.ContainsKey("GoRefreshToken"))
                {
                    Helper.GoRefreshToken = Convert.ToString(Application.Current.Properties["GoRefreshToken"]);
                }
                if (Application.Current.Properties.ContainsKey("FBcustomerAccList"))
                {
                    var FBAdResponseResult = JsonConvert.DeserializeObject<FBAccIdListResponse>(Convert.ToString(Application.Current.Properties["FBcustomerAccList"]));
                    Helper.FBcustomerAccList = FBAdResponseResult.result;
                }
                if(Application.Current.Properties.ContainsKey("facebook_page_id"))
                {
                    Helper.imageCampaign.facebook_page_id = Helper.videoCampaign.facebook_page_id = Convert.ToString(Application.Current.Properties["facebook_page_id"]);
                    Helper.imageCampaign.facebook_page_name = Helper.videoCampaign.facebook_page_name = Convert.ToString(Application.Current.Properties["facebook_page_name"]);
                }

                if (Application.Current.Properties.ContainsKey("google_ads_customers"))
                {
                    //var goCustomerListResult = JsonConvert.DeserializeObject<GoCustomerListResponse>(Convert.ToString(Application.Current.Properties["google_ads_customers"]));
                    InitDatabaseTable db = new InitDatabaseTable();
                    var goCustomerListResult = JsonConvert.DeserializeObject<GoCustomerListResponse>(db.Connection.Table<GoogleAdsCustomersDBModel>().FirstOrDefault().GoogleAdsCustomersData);
                    Helper.GocustomerAccList = goCustomerListResult.result;
                }

                if (Application.Current.Properties.ContainsKey("SelectedGoAdCustDetail"))
                {
                    // Helper.SelectedGoAdCustDetail = JsonConvert.DeserializeObject<GoCustomerListResult>(Convert.ToString(Application.Current.Properties["SelectedGoAdCustDetail"]));
                    //InitDatabaseTable db = new InitDatabaseTable();
                    //var SelectedAccount = db.Connection.Table<SelectedGoAdCustDetailDBModel>().FirstOrDefault();
                    //GoCustomerListResult selectedGoAdCustDetail = new GoCustomerListResult();
                    Helper.SelectedGoAdCustDetail = new GoCustomerListResult();
                    Helper.SelectedGoAdCustDetail.customer_id = Convert.ToString(Application.Current.Properties["SelectedGoAdCustDetailcustomerid"]);
                    Helper.SelectedGoAdCustDetail.manager_id = Convert.ToString(Application.Current.Properties["SelectedGoAdCustDetailmanagerid"]);
                    Helper.SelectedGoAdCustDetail.Tick = Convert.ToString(Application.Current.Properties["SelectedGoAdCustDetailTick"]);
                    Helper.SelectedGoAdCustDetail.IsSubAcc = Convert.ToBoolean(Application.Current.Properties["SelectedGoAdCustDetailIsSubAcc"]);
                    Helper.SelectedGoAdCustDetail.ExpandArrow = Convert.ToString(Application.Current.Properties["SelectedGoAdCustDetailExpandArrow"]);
                    Helper.SelectedGoAdCustDetail.time_zone = Convert.ToString(Application.Current.Properties["SelectedGoAdCustDetailtimezone"]);
                    Helper.SelectedGoAdCustDetail.is_manager = Convert.ToBoolean(Application.Current.Properties["SelectedGoAdCustDetailismanager"]);
                    Helper.SelectedGoAdCustDetail.descriptive_name = Convert.ToString(Application.Current.Properties["SelectedGoAdCustDetaildescriptivename"]);
                    Helper.SelectedGoAdCustDetail.currency_code = Convert.ToString(Application.Current.Properties["SelectedGoAdCustDetailcurrencycode"]);
                }

                var globalAccessResult = Helper.GetLoginUserData();
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
                  MainPage = new NavigationPage(new DashBoard());
            }
            else
            {
                MainPage = new NavigationPage(new LoginView());
                //MainPage = new NavigationPage(new AddCampaignDetailView());
            }
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                var Conn = args.IsConnected ? true : false;
                if (Conn == false)
                {
                    if (PopupNavigation.Instance.PopupStack.Count >= 1)
                    {
                        PopupNavigation.Instance.PopAllAsync();

                        if (PopupNavigation.Instance.PopupStack.Count == 0)
                        {
                           var popupnav = new UserDialogPopup(Constant.PopupTitle, "Internet not available", "Ok");
                            PopupNavigation.Instance.PushAsync(popupnav);
                            //IsChecking = false;
                        }
                    }
                    else
                    {
                        var popupnav = new UserDialogPopup(Constant.PopupTitle, "Internet not available", "Ok");
                        PopupNavigation.Instance.PushAsync(popupnav);
                        //IsChecking = false;
                    }
                }
            };
            AppCenter.Start("android=bed9ce51-0847-46c0-b4ae-102ec43d97c7;" +
                  "ios=b0616935-ba41-4aa6-9974-69a29fba30db;",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnStart()
        {
            Constant.IsConnected = CrossConnectivity.Current.IsConnected;
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                Constant.IsConnected = args.IsConnected;
            };
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        void InitializeingServices()
        {
            try
            {
                DependencyService.Register<ILoginService, LoginService>();
                DependencyService.Register<IEmailLoginService, EmailLoginService>();
                DependencyService.Register<IForgotPasswordService, ForgotPasswordService>();
                DependencyService.Register<IFBPageListService, FBPageListService>();
                DependencyService.Register<IFBPageCategoryService, FBPageCategoryService>();
                DependencyService.Register<ICreateAccountService, CreateAccountService>();
                DependencyService.Register<IChangePasswordService, ChangePasswordService>();
                DependencyService.Register<IFBCampListService, FBCampListService>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
