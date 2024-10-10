using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Services;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class DashBoardViewModel : BaseViewModel
    {
        #region constructor
        public DashBoardViewModel()
        {
            if (Application.Current.Properties.ContainsKey("WelcomeView"))
            {
                WelcomeViewVisible = false;
            }
            MessagingCenter.Subscribe<object, bool>(this, "WelcomeView", (sender, arg) =>
            {
                    WelcomeViewVisible = arg;
            });

            if (Helper.profileModel != null)
                profilePic = Helper.profileModel.ProfileImage != null ? Helper.profileModel.ProfileImage : "ProfilePic";
        }

        #endregion

        #region properties
        public INavigation nav { get; set; }

        public bool IsFacebookAd = false;
        public bool IsGoogleAd = false;
        public string FBAccessToken;

        private readonly FBCampListService _FBCampListService;

        private ObservableCollection<Campaign> _CampaignList = new ObservableCollection<Campaign>();
        public ObservableCollection<Campaign> CampaignList
        {
            get { return _CampaignList; }
            set { _CampaignList = value; OnPropertyChanged("CampaignList"); }
        }

        private ImageSource profilePic = "ProfilePic";
        public ImageSource ProfilePic { get => profilePic; set => SetProperty(ref profilePic, value); }


        private bool _StartNewCampBtn = true;
        public bool StartNewCampBtn
        {
            get
            {
                return _StartNewCampBtn;
            }
            set
            {
                _StartNewCampBtn = value;
                OnPropertyChanged("StartNewCampBtn");
            }
        }



        private double _ProgGraphHeight = 150;
        public double ProgGraphHeight
        {
            get
            {
                return _ProgGraphHeight;
            }
            set
            {
                _ProgGraphHeight = value;
                OnPropertyChanged("ProgGraphHeight");
            }
        }


        private bool _WelcomeViewVisible = true;
        public bool WelcomeViewVisible
        {
            get
            {
                return _WelcomeViewVisible;
            }
            set
            {
                _WelcomeViewVisible = value;
                OnPropertyChanged("WelcomeViewVisible");
            }
        }

        private bool _AddNewCampBtn = false;
        public bool AddNewCampBtn
        {
            get
            {
                return _AddNewCampBtn;
            }
            set
            {
                _AddNewCampBtn = value;
                OnPropertyChanged("AddNewCampBtn");
            }
        }


        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                OnPropertyChanged("IsRefreshing");
            }
        }

        private double _MainProgressBar = 0.0;
        public double MainProgressBar
        {
            get
            {
                return _MainProgressBar;
            }
            set
            {
                _MainProgressBar = value;
                OnPropertyChanged("MainProgressBar");
            }
        }


        private string _DailyPercent = "0";
        public string DailyPercent
        {
            get
            {
                return _DailyPercent;
            }
            set
            {
                _DailyPercent = value;
                OnPropertyChanged("DailyPercent");
            }
        }

        private string _OptimizedScore = "0";
        public string OptimizedScore
        {
            get
            {
                return _OptimizedScore;
            }
            set
            {
                _OptimizedScore = value;
                OnPropertyChanged("OptimizedScore");
            }
        }

        //private string _CurrencySymbol = "$";
        //public string CurrencySymbol
        //{
        //    get
        //    {
        //        return _CurrencySymbol;
        //    }
        //    set
        //    {
        //        _CurrencySymbol = value;
        //        OnPropertyChanged("CurrencySymbol");
        //    }
        //}

        private string _SpentAmount = "0";
        public string SpentAmount
        {
            get
            {
                return _SpentAmount;
            }
            set
            {
                _SpentAmount = value;
                OnPropertyChanged("SpentAmount");
            }
        }

        private string _BudgetAmount = "0";
        public string BudgetAmount
        {
            get
            {
                return _BudgetAmount;
            }
            set
            {
                _BudgetAmount = value;
                OnPropertyChanged("BudgetAmount");
            }
        }

        private double _SpentAmountProgress = 0.0;
        public double SpentAmountProgress
        {
            get
            {
                return _SpentAmountProgress;
            }
            set
            {
                _SpentAmountProgress = value;
                OnPropertyChanged("SpentAmountProgress");
            }
        }


        private CampaignData _SelectedCamp;
        public CampaignData SelectedCamp
        {
            get
            {
                return _SelectedCamp;
            }
            set
            {

                _SelectedCamp = value;
                OnPropertyChanged("SelectedCamp");
                if (SelectedCamp != null)
                {
                    SelectedCamp = null;
                }

            }
        }

        #endregion

        #region commands

        private Command profilePicTapped;
        public ICommand ProfilePicTapped
        {
            get
            {
                if (profilePicTapped == null)
                {
                    profilePicTapped = new Command(async () => await PerformProfilePicTapped());
                }

                return profilePicTapped;
            }
        }



        private Command refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new Command(PerformRefreshCommand);
                }

                return refreshCommand;
            }
        }

        private Command<Campaign> _CampSelectedCmd;

        public Command<Campaign> CampSelectedCmd
        {
            get
            {
                if (_CampSelectedCmd == null)
                {
                    _CampSelectedCmd = new Command<Campaign>((id) =>
                    {
                        CampSelectedCommand(id);
                    });
                }

                return _CampSelectedCmd;
            }
        }

        private Command bookTapped;

        public ICommand BookTapped
        {
            get
            {
                if (bookTapped == null)
                {
                    bookTapped = new Command(PerformBookTapped);
                }

                return bookTapped;
            }
        }

        private Command notificationTapped;

        public ICommand NotificationTapped
        {
            get
            {
                if (notificationTapped == null)
                {
                    notificationTapped = new Command(PerformNotificationTapped);
                }

                return notificationTapped;
            }
        }

        #endregion

        #region methods
        private async Task PerformProfilePicTapped()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                await nav.PushAsync(new ProfileView());
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

    

        private void PerformBookTapped()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                nav.PushAsync(new HelpGuideView());
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

      

        private void PerformNotificationTapped()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                nav.PushAsync(new NotificationView());
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        public async void GetCampaignData()
        {
            if (await Helper.CheckSession())
            {
                return;
            }

            if (!Conn)
            {
                return;
            }
            if (Helper.facebookProfile != null || !string.IsNullOrWhiteSpace(Helper.profileModel.google_ad_customer_id))
            {
                try
                {
                    if (Helper.facebookProfile != null)
                    {
                        IsFacebookAd = true;
                        FBAccessToken = Helper.facebookProfile.Token;
                    }
                    else
                    {
                        FBAccessToken = null;
                    }

                    if (Helper.profileModel.google_ad_customer_id != null)
                        IsGoogleAd = true;


                    IsBusy = true;
                    CampListRequestModel CampListRequestModel = new CampListRequestModel
                    {
                        fb_access_token = FBAccessToken,
                        user_id = Convert.ToString(Constant.OneTapUserId),
                        google_ad_customer_id = Helper.profileModel.google_ad_customer_id,
                        is_facebook_ad = IsFacebookAd,
                        google_manager_id = Helper.profileModel.google_ad_manager_id,
                        is_google_ad = IsGoogleAd,
                        google_refresh_token = Helper.GoRefreshToken,
                        fb_ad_account_id = Helper.profileModel.fb_ad_account_id
                    };
                    try
                    {
                        string url = "user/campaigns";
                        Rest_ResponseModel rest_result = await WebService.WebService.PostData(CampListRequestModel, url, true);
                        if (rest_result != null)
                        {
                            if (rest_result.status_code == 200)
                            {
                                var CampListResponseModel = JsonConvert.DeserializeObject<CampListResponseModel>(rest_result.response_body);

                                if (CampListResponseModel != null)
                                {
                                    if (CampListResponseModel.status)
                                    {
                                        List<Campaign> list = new List<Campaign>();
                                        list = CampListResponseModel.result.campaigns;
                                        var tempCampaignList = new ObservableCollection<Campaign>(list);
                                        tempCampaignList.ToList().ForEach(c =>
                                        {
                                            if (c.type == "facebook")
                                            {
                                                if (Helper.profileModel.currency.ToUpper() == "INR")
                                                {
                                                    c.CurrencySymbol = "₹";
                                                }
                                                else if (Helper.profileModel.currency.ToUpper() == "AUD")
                                                {
                                                    c.CurrencySymbol = "$";
                                                }
                                                else
                                                {
                                                    c.CurrencySymbol = NumberFormatInfo.CurrentInfo.CurrencySymbol;
                                                }

                                                if (!c.instagram_publish)
                                                    c.campaign_Icon = "FBIconBlue";
                                                else
                                                    c.campaign_Icon = "InstaIcon";
                                            }
                                            else
                                            {
                                                if (Helper.SelectedGoAdCustDetail.currency_code.ToUpper() == "INR")
                                                {
                                                    c.CurrencySymbol = "₹";
                                                }
                                                else if (Helper.SelectedGoAdCustDetail.currency_code.ToUpper() == "AUD")
                                                {
                                                    c.CurrencySymbol = "$";
                                                }
                                                else
                                                {
                                                    c.CurrencySymbol = NumberFormatInfo.CurrentInfo.CurrencySymbol;
                                                }
                                                c.campaign_Icon = "Google";
                                            }
                                        });
                                        CampaignList = new ObservableCollection<Campaign>(tempCampaignList.ToList().OrderByDescending(x => x.created_time));

                                        MainProgressBar = Convert.ToDouble(CampListResponseModel.result.today_optimization_score_percentage) / 100;
                                        DailyPercent = Convert.ToString(CampListResponseModel.result.today_optimization_score_percentage);
                                        OptimizedScore = Convert.ToString(CampListResponseModel.result.overall_optimization_score_percentage);
                                        SpentAmount = Convert.ToDecimal(CampListResponseModel.result.amount_spent).ToString("0.00");
                                        Helper.profileModel.amount_spent = CampListResponseModel.result.amount_spent;
                                        if (IsFacebookAd)
                                        {
                                            Helper.profileModel.facebook_spent =Convert.ToDouble(CampListResponseModel.result.facebook_spent.ToString("0.00"));
                                            //Helper.profileModel.fb_ad_account_id = CampListResponseModel.result.fb_ad_account_id;
                                        }
                                        if (IsGoogleAd)
                                        {
                                            Helper.profileModel.google_spent = Convert.ToDouble(CampListResponseModel.result.google_spent.ToString("0.00"));
                                        }
                                        SpentAmountProgress = CampListResponseModel.result.amount_spent / CampListResponseModel.result.total_budget;
                                        BudgetAmount = Convert.ToDouble(CampListResponseModel.result.total_budget).ToString("0.00");
                                        Helper.SavePropertyData("profileModel", JsonConvert.SerializeObject(Helper.profileModel));
                                        MessagingCenter.Send<object, bool>(this, "RefreshFbAdAccountIddata", true);

                                        if (CampaignList.Count() == 0)
                                        {
                                            AddNewCampBtn = false;
                                            StartNewCampBtn = true;
                                        }
                                        else
                                        {
                                            AddNewCampBtn = true;
                                            StartNewCampBtn = false;
                                        }
                                        IsBusy = false;

                                    }
                                    else
                                    {
                                        popupnav = new UserDialogPopup(Constant.PopupTitle, CampListResponseModel.message, "Ok");
                                        await PopupNavigation.Instance.PushAsync(popupnav);
                                        IsBusy = false;
                                    }
                                }
                                else
                                {
                                    popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                    IsBusy = false;
                                }
                            }
                            else
                            {
                                popupnav = new UserDialogPopup("Message", "Sorry ! no Campaign Found in that account please try again ", "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                            }

                        }
                        else
                        {
                            popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            IsBusy = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        IsChecking = false;
                        Debug.WriteLine(ex.Message);

                    }
                    finally
                    {
                        IsBusy = false;
                        IsChecking = false;
                    }
                }

                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    IsshowEmpty = true;
                    IsBusy = false;

                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private void PerformRefreshCommand(object obj)
        {
            IsRefreshing = true;
            GetCampaignData();
            IsRefreshing = false;
        }


        private void CampSelectedCommand(Campaign SelectedItem)
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                if (SelectedItem != null)
                    nav.PushAsync(new CampaignOverviewView(SelectedItem));
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
