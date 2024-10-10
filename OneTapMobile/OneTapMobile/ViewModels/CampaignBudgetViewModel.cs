using FFImageLoading;
using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.LocalDataBase;
using OneTapMobile.LocalDataBases;
using OneTapMobile.LocalDataBases.DataBaseModel;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using OneTapMobile.Views.ErrorPage;
using RestSharp;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace OneTapMobile.ViewModels
{
    public class CampaignBudgetViewModel : BaseViewModel
    {

        #region constructor
        public CampaignBudgetViewModel()
        {

            // these are for default selection of daily budget type.

            if(Helper.CreateCampType.ToLower() == "image")
            {
                if(string.IsNullOrWhiteSpace(Helper.imageCampaign.BudgetType))
                    PerformDailyOptionCmd();
            } 

            if(Helper.CreateCampType.ToLower() == "video")
            {
                if(string.IsNullOrWhiteSpace(Helper.videoCampaign.BudgetType))
                    PerformDailyOptionCmd();
            }
            
            if(Helper.CreateCampType.ToLower() == "keywords")
            {
                if(string.IsNullOrWhiteSpace(Helper.keywordCampaign.BudgetType))
                    PerformDailyOptionCmd();
            }
        }
        #endregion

        #region properties

        public INavigation nav;

        //private Command paypalOptionCmd;
        //public ICommand PaypalOptionCmd
        //{
        //    get
        //    {
        //        if (paypalOptionCmd == null)
        //        {
        //            paypalOptionCmd = new Command(PerformPaypalOptionCmd);
        //        }
        //        return paypalOptionCmd;
        //    }
        //}

        InitDatabaseTable db = new InitDatabaseTable();

        private string startDate = "Choose Date";
        public string StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");

            }
        }

        private string endDate = "Choose Date";
        public string EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        //private Command stripeOptionCmd;
        //public ICommand StripeOptionCmd
        //{
        //    get
        //    {
        //        if (stripeOptionCmd == null)
        //        {
        //            stripeOptionCmd = new Command(PerformStripeOptionCmd);
        //        }
        //        return stripeOptionCmd;
        //    }
        //}

        private Command lifetimeOptionCmd;
        public ICommand LifetimeOptionCmd
        {
            get
            {
                if (lifetimeOptionCmd == null)
                {
                    lifetimeOptionCmd = new Command(PerformLifetimeOptionCmd);
                }
                return lifetimeOptionCmd;
            }
        }

        //// Stripe frame border and right check icon properties

        //private bool stripeRightGray = false;
        //public bool StripeRightGray { get => stripeRightGray; set => SetProperty(ref stripeRightGray, value); }

        //private bool stripeRightGreen = true;
        //public bool StripeRightGreen { get => stripeRightGreen; set => SetProperty(ref stripeRightGreen, value); }

        //private Color stripeFrmBorder = Color.FromHex("#8826C7");
        //public Color StripeFrmBorder { get => stripeFrmBorder; set => SetProperty(ref stripeFrmBorder, value); }

        //// Paypal frame border and right check icon properties

        //private bool paypalRightGray = true;
        //public bool PaypalRightGray { get => paypalRightGray; set => SetProperty(ref paypalRightGray, value); }

        //private bool paypalRightGreen = false;
        //public bool PaypalRightGreen { get => paypalRightGreen; set => SetProperty(ref paypalRightGreen, value); }

        //private Color paypalFrmBorder;
        //public Color PaypalFrmBorder { get => paypalFrmBorder; set => SetProperty(ref paypalFrmBorder, value); }

        // Daily frame border and right check icon properties

        private bool dailyRightGray = false;
        public bool DailyRightGray { get => dailyRightGray; set => SetProperty(ref dailyRightGray, value); }

        private bool dailyRightGreen = true;
        public bool DailyRightGreen { get => dailyRightGreen; set => SetProperty(ref dailyRightGreen, value); }

        private Color dailyFrmBorder = Color.FromHex("#8826C7");
        public Color DailyFrmBorder { get => dailyFrmBorder; set => SetProperty(ref dailyFrmBorder, value); }

        // Lifetime frame border and right check icon properties

        private bool lifetimeRightGray = true;
        public bool LifetimeRightGray { get => lifetimeRightGray; set => SetProperty(ref lifetimeRightGray, value); }

        private bool lifetimeRightGreen = false;
        public bool LifetimeRightGreen { get => lifetimeRightGreen; set => SetProperty(ref lifetimeRightGreen, value); }

        private Color lifetimeFrmBorder;
        public Color LifetimeFrmBorder { get => lifetimeFrmBorder; set => SetProperty(ref lifetimeFrmBorder, value); }

        // Budget value

        private string budgetTxt = String.Empty;
        public string BudgetTxt { get => budgetTxt; set => SetProperty(ref budgetTxt, value); }

        #endregion

        #region commands
        private Command backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new Command(Back);
                }
                return backCommand;
            }
        }

        private Command helpCommand;
        public ICommand HelpCommand
        {
            get
            {
                if (helpCommand == null)
                {
                    helpCommand = new Command(Help);
                }
                return helpCommand;
            }
        }

        private Command dailyOptionCmd;
        public ICommand DailyOptionCmd
        {
            get
            {
                if (dailyOptionCmd == null)
                {
                    dailyOptionCmd = new Command(PerformDailyOptionCmd);
                }
                return dailyOptionCmd;
            }
        }
        private Command continueBtnCommand;
        public ICommand ContinueBtnCommand
        {
            get
            {
                if (continueBtnCommand == null)
                {
                    continueBtnCommand = new Command(async () => await ContinueBtn());
                }
                return continueBtnCommand;
            }
        }

        #endregion

        #region methods
        //private void PerformPaypalOptionCmd()
        //{
        //    PaypalFrmBorder = Color.FromHex("#8826C7");
        //    StripeFrmBorder = Color.FromHex("#0000ffff");

        //    PaypalRightGray = false;
        //    PaypalRightGreen = true;

        //    StripeRightGray = true;
        //    StripeRightGreen = false;

        //    Helper.videoCampaign.PaymentMethod = "Paypal";
        //    Helper.imageCampaign.PaymentMethod = "Paypal";
        //    Helper.keywordCampaign.PaymentMethod = "Paypal";

        //}

        //private void PerformStripeOptionCmd()
        //{
        //    PaypalFrmBorder = Color.FromHex("#0000ffff");
        //    StripeFrmBorder = Color.FromHex("#8826C7");

        //    StripeRightGray = false;
        //    StripeRightGreen = true;

        //    PaypalRightGray = true;
        //    PaypalRightGreen = false;

        //    Helper.videoCampaign.PaymentMethod = "Stripe";
        //    Helper.imageCampaign.PaymentMethod = "Stripe";
        //    Helper.keywordCampaign.PaymentMethod = "Stripe";

        //}

        public void PerformDailyOptionCmd()
        {
            DailyFrmBorder = Color.FromHex("#8826C7");
            LifetimeFrmBorder = Color.FromHex("#0000ffff");

            DailyRightGray = false;
            DailyRightGreen = true;

            LifetimeRightGray = true;
            LifetimeRightGreen = false;

            Helper.videoCampaign.BudgetType = "Daily";
            Helper.imageCampaign.BudgetType = "Daily";
            Helper.keywordCampaign.BudgetType = "Daily";
        }

        public void PerformLifetimeOptionCmd()
        {
            DailyFrmBorder = Color.FromHex("#0000ffff");
            LifetimeFrmBorder = Color.FromHex("#8826C7");

            DailyRightGray = true;
            DailyRightGreen = false;

            LifetimeRightGray = false;
            LifetimeRightGreen = true;

            Helper.videoCampaign.BudgetType = "Lifetime";
            Helper.imageCampaign.BudgetType = "Lifetime";
            Helper.keywordCampaign.BudgetType = "Lifetime";
        }


        private async Task ContinueBtn()
        {
            try
            {
                if(await Helper.CheckSession())
                {
                    return;
                }

                if (IsTap)
                    return;
                IsTap = true;

                IsBusy = true;
                if (budgetTxt != string.Empty)
                {
                    if (!budgetTxt.Contains("."))
                    {
                        Helper.imageCampaign.Budget = BudgetTxt;
                        Helper.videoCampaign.Budget = BudgetTxt;
                        Helper.keywordCampaign.Budget = BudgetTxt;
                    }
                    else
                    {
                        IsTap = false;
                        popupnav = new UserDialogPopup("Message", "budget value should be integer", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                        return;
                    }
                }
                else
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup("Message", "please enter budget value", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    IsBusy = false;
                    return;
                }
                if (startDate == "Choose Date" || endDate == "Choose Date")
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup("Message", "please select Start or End Date", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    IsBusy = false;
                    return;
                }
                else if (Convert.ToDateTime(StartDate) >= Convert.ToDateTime(EndDate))
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup("Message", "start Date must be less then End Date", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    IsBusy = false;
                    return;
                }
                else
                {
                    if (Convert.ToDateTime(startDate) < Convert.ToDateTime(endDate))
                    {
                        if (Helper.CreateCampType.ToLower().ToLower() == "image")
                        {
                            try
                            {
                                var ImageCamp = Helper.imageCampaign;
                                List<FBImageLocation> Targetloc = new List<FBImageLocation>();
                                foreach (var item in ImageCamp.TargetLocation)
                                {
                                    if (item != null)
                                    {
                                        Targetloc.Add(new FBImageLocation
                                        {
                                            country_code = item.country_code,
                                            country_name = item.country_name,
                                            key = item.key,
                                            name = item.name,
                                            region = item.region,
                                            region_id = item.region_id,
                                            type = item.type
                                        });
                                    }
                                }
                                List<PostCodesResult> PostCodes = new List<PostCodesResult>();
                                foreach (var item in ImageCamp.postcodes)
                                {
                                    if (item != null)
                                    {
                                        PostCodes.Add(new PostCodesResult
                                        {
                                            country_code = item.country_code,
                                            country_name = item.country_name,
                                            key = item.key,
                                            name = item.name,
                                            region = item.region,
                                            region_id = item.region_id,
                                            type = item.type,
                                            supports_region = item.supports_region,
                                            primary_city  =item.primary_city,
                                            primary_city_id = item.primary_city_id,
                                            supports_city = item.supports_city
                                        });
                                    }
                                }

                                List<FBImageInterest> ImageInterest = new List<FBImageInterest>();
                                foreach (var item in ImageCamp.DemoInterest)
                                {
                                    if (item != null)
                                    {
                                        ImageInterest.Add(new FBImageInterest
                                        {
                                            id = item.id,
                                            name = item.name
                                        });
                                    }
                                }


                                FBImageAudienceData fbImageAudienceData = new FBImageAudienceData
                                {
                                    age_min = ImageCamp.StartAgeRange,
                                    age_max = ImageCamp.EndAgeRange,
                                    gender = ImageCamp.Gender,
                                    locations = Targetloc,
                                    interests = ImageInterest,
                                    postcodes = PostCodes
                                };



                                FBImageBudgetData fbImageBudget_Data = new FBImageBudgetData
                                {
                                    bid_amount = Convert.ToInt16(ImageCamp.Budget),
                                    budget_amount = Convert.ToInt16(ImageCamp.Budget),
                                    budget_type = ImageCamp.BudgetType,
                                    start_time = Convert.ToString(ImageCamp.StartCampDate),
                                    end_time = Convert.ToString(ImageCamp.EndCampDate)
                                };

                                string base64String = Convert.ToBase64String(Helper.CampimageByte, 0, Helper.CampimageByte.Length);

                                FBImageFields fbImageFields = new FBImageFields
                                {
                                    button_title = ImageCamp.ButtonTitle,
                                    campaign_type = Helper.CreateCampType.ToLower(),
                                    facebook_page_id = Helper.imageCampaign.facebook_page_id,
                                    image_base64_string = base64String,
                                    name = ImageCamp.CampName,
                                    objective = "LINK_CLICKS", // LINK_CLICKS is used as upload image objective.
                                    primary_text = ImageCamp.PrimaryText,
                                    website_url = ImageCamp.WebURL,
                                    headline = ImageCamp.Headline,

                                };

                                FBImgCreateCampRequestModel fbImgCreateCampRequestModel = new FBImgCreateCampRequestModel
                                {
                                    ads_type = "facebook_ads",
                                    audience_data = fbImageAudienceData,
                                    budget_data = fbImageBudget_Data,
                                    fb_access_token = Helper.facebookProfile.Token,
                                    fb_ad_account_id = Helper.profileModel.fb_ad_account_id,
                                    fields = fbImageFields,
                                    user_id = Convert.ToString(Constant.OneTapUserId)
                                };
                                try
                                {

                                    string url = "user/create-campaign";

                                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(fbImgCreateCampRequestModel, url, true);
                                    if (rest_result != null)
                                    {
                                        if (rest_result.status_code == 200)
                                        {
                                            var createCampResponse = JsonConvert.DeserializeObject<CreateCampResponseModel>(rest_result.response_body);

                                            if (createCampResponse != null)
                                            {
                                                if (createCampResponse.status)
                                                {
                                                    var a = createCampResponse.result;
                                                    IsBusy = false;
                                                    IsChecking = true;
                                                    await Task.Delay(1000);
                                                    IsChecking = false;
                                                    await nav.PopToRootAsync();
                                                    MessagingCenter.Send<object, bool>(this, "RefreshCampaignList", true);
                                                }
                                                else
                                                {
                                                    IsTap = false;
                                                    // Store to database the created campaign
                                                    db.Save(ImageModelForDB(Helper.imageCampaign));
                                                    var popupnav = new UserDialogPopup(Constant.PopupTitle, createCampResponse.message, "Ok");
                                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                                    IsBusy = false;

                                                }
                                            }
                                            else
                                            {
                                                IsTap = false;

                                                // Store to database the created campaign
                                                db.Save(ImageModelForDB(Helper.imageCampaign));

                                                var popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                                                await PopupNavigation.Instance.PushAsync(popupnav);
                                                IsBusy = false;
                                            }
                                        }
                                        else
                                        {
                                            IsTap = false;

                                            // Store to database the created campaign
                                            db.Save(ImageModelForDB(Helper.imageCampaign));

                                            var popupnav = new UserDialogPopup(Constant.PopupTitle, "Can't communicate to server", "Ok");
                                            await PopupNavigation.Instance.PushAsync(popupnav);
                                            IsBusy = false;
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Store to database the created campaign
                                    db.Save(ImageModelForDB(Helper.imageCampaign));

                                    IsTap = false;
                                    Debug.WriteLine(ex.Message);
                                    IsBusy = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                // Store to database the created campaign
                                db.Save(ImageModelForDB(Helper.imageCampaign));

                                IsTap = false;
                                IsBusy = false;
                                Debug.WriteLine(ex.Message);
                            }
                        }
                        else if (Helper.CreateCampType.ToLower() == "video")
                        {
                            try
                            {

                                var UploadResponse = await UploadVideoAndThumb();
                                var Uploadresult = UploadResponse.result;

                                if (Uploadresult != null)
                                {
                                    Helper.videoCampaign.video_id = Uploadresult.video_id;
                                    Helper.videoCampaign.video_url = Uploadresult.video_url;
                                    Helper.videoCampaign.thumb_url = Uploadresult.thumb_url;
                                }
                                else
                                {
                                    IsTap = false;
                                    var popupnav = new UserDialogPopup(Constant.PopupTitle, "Video upload caused Issues, please try again!!", "Ok");
                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                    IsBusy = false;
                                }
                                var VideoCamp = Helper.videoCampaign;
                                List<FBVideoLocation> Targetloc = new List<FBVideoLocation>();
                                foreach (var item in VideoCamp.TargetLocation)
                                {
                                    if (item != null)
                                    {
                                        Targetloc.Add(new FBVideoLocation
                                        {
                                            country_code = item.country_code,
                                            country_name = item.country_name,
                                            key = item.key,
                                            name = item.name,
                                            region = item.region,
                                            region_id = item.region_id,
                                            type = item.type
                                        });
                                    }
                                }

                                List<FBVideoInterest> VideoInterest = new List<FBVideoInterest>();
                                foreach (var item in VideoCamp.DemoInterest)
                                {
                                    if (item != null)
                                    {
                                        VideoInterest.Add(new FBVideoInterest
                                        {
                                            id = item.id,
                                            name = item.name
                                        });
                                    }
                                }

                                List<PostCodesResult> PostCodes = new List<PostCodesResult>();
                                foreach (var item in VideoCamp.postcodes)
                                {
                                    if (item != null)
                                    {
                                        PostCodes.Add(new PostCodesResult
                                        {
                                            country_code = item.country_code,
                                            country_name = item.country_name,
                                            key = item.key,
                                            name = item.name,
                                            region = item.region,
                                            region_id = item.region_id,
                                            type = item.type,
                                            supports_region = item.supports_region,
                                            primary_city = item.primary_city,
                                            primary_city_id = item.primary_city_id,
                                            supports_city = item.supports_city
                                        });
                                    }
                                }


                                FBVideoAudienceData fbVideoAudienceData = new FBVideoAudienceData
                                {
                                    age_min = VideoCamp.StartAgeRange,
                                    age_max = VideoCamp.EndAgeRange,
                                    gender = VideoCamp.Gender,
                                    locations = Targetloc,
                                    interests = VideoInterest,
                                    postcodes = PostCodes
                                };

                                FBVideoBudgetData fbVideoBudget_Data = new FBVideoBudgetData
                                {
                                    bid_amount = Convert.ToInt16(VideoCamp.Budget),
                                    budget_amount = Convert.ToInt16(VideoCamp.Budget),
                                    budget_type = VideoCamp.BudgetType,
                                    start_time = Convert.ToString(VideoCamp.StartCampDate),
                                    end_time = Convert.ToString(VideoCamp.EndCampDate)
                                };

                                FBVideoFields fbVideoFields = new FBVideoFields
                                {
                                    button_title = VideoCamp.ButtonTitle,
                                    campaign_type = Helper.CreateCampType.ToLower(),
                                    facebook_page_id = Helper.videoCampaign.facebook_page_id,
                                    name = VideoCamp.CampName,
                                    objective = "VIDEO_VIEWS", // only this will accept videos
                                    primary_text = VideoCamp.PrimaryText,
                                    video_id = VideoCamp.video_id,
                                    video_thumb_url = VideoCamp.thumb_url,
                                    website_url = VideoCamp.WebURL,
                                    headline = VideoCamp.Headline,
                                };

                                FbVidCreateCampRequestModel fbVidCreateCampRequestModel = new FbVidCreateCampRequestModel
                                {
                                    ads_type = "facebook_ads",
                                    audience_data = fbVideoAudienceData,
                                    budget_data = fbVideoBudget_Data,
                                    fb_access_token = Helper.facebookProfile.Token,
                                    fb_ad_account_id = Helper.profileModel.fb_ad_account_id,
                                    fields = fbVideoFields,
                                    user_id = Convert.ToString(Constant.OneTapUserId)
                                };
                                try
                                {
                                    string url = "user/create-campaign";

                                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(fbVidCreateCampRequestModel, url, true);
                                    if (rest_result != null)
                                    {
                                        if (rest_result.status_code == 200)
                                        {
                                            var createCampResponse = JsonConvert.DeserializeObject<CreateCampResponseModel>(rest_result.response_body);

                                            if (createCampResponse != null)
                                            {
                                                if (createCampResponse.status)
                                                {
                                                    var a = createCampResponse.result;
                                                    IsBusy = false;
                                                    IsChecking = true;
                                                    await Task.Delay(1000);
                                                    IsChecking = false;
                                                    await nav.PopToRootAsync();
                                                    MessagingCenter.Send<object, bool>(this, "RefreshCampaignList", true);
                                                }
                                                else
                                                {
                                                    IsTap = false;

                                                    // Store to database the created campaign
                                                    db.Save(VideoModelForDB(Helper.videoCampaign));

                                                    var popupnav = new UserDialogPopup(Constant.PopupTitle, createCampResponse.message, "Ok");
                                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                                    IsBusy = false;

                                                }
                                            }
                                            else
                                            {
                                                IsTap = false;

                                                // Store to database the created campaign
                                                db.Save(VideoModelForDB(Helper.videoCampaign));

                                                var popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                                                await PopupNavigation.Instance.PushAsync(popupnav);
                                                IsBusy = false;
                                            }
                                        }
                                        else
                                        {
                                            IsTap = false;

                                            // Store to database the created campaign
                                            db.Save(VideoModelForDB(Helper.videoCampaign));

                                            var popupnav = new UserDialogPopup(Constant.PopupTitle, "Can't communicate to server", "Ok");
                                            await PopupNavigation.Instance.PushAsync(popupnav);
                                            IsBusy = false;
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    IsTap = false;

                                    // Store to database the created campaign
                                    db.Save(VideoModelForDB(Helper.videoCampaign));

                                    Debug.WriteLine(ex.Message);
                                    IsBusy = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                IsTap = false;
                                IsBusy = false;

                                // Store to database the created campaign
                                db.Save(VideoModelForDB(Helper.videoCampaign));

                                Debug.WriteLine(ex.Message);
                            }

                        }
                        else if (Helper.CreateCampType.ToLower() == "keywords")
                        {
                            try
                            {
                                var KeywordCamp = Helper.keywordCampaign;

                                GoFields goFields = new GoFields
                                {
                                    campaign_type = Helper.CreateCampType.ToLower(),
                                    objective = "SEARCH",// hard coded because we don't have entry field for that. 
                                    name = Helper.keywordCampaign.CampName,
                                };

                                GoBudgetData goBudgetData = new GoBudgetData
                                {
                                    bid_amount = 0,
                                    budget_amount = Convert.ToInt32(Helper.keywordCampaign.Budget),
                                    budget_type = Helper.keywordCampaign.BudgetType.ToLower(),
                                    end_time = Helper.keywordCampaign.EndCampDate,
                                    start_time = Helper.keywordCampaign.StartCampDate,
                                };


                                List<string> tarloc = new List<string>();
                                foreach(var location in Helper.keywordCampaign.TargetLocation)
                                {
                                    tarloc.Add(location.id.ToString());
                                }


                                GoKeywordsData goKeywordsData = new GoKeywordsData
                                {
                                    keywords = Helper.keywordCampaign.KeywordTheme,
                                    location_ids = tarloc,
                                    website_url = Helper.keywordCampaign.WebsiteURL,
                                };

                                GoResponsiveAdsData goResponsiveAdsData = new GoResponsiveAdsData
                                {
                                    description_1 = Helper.keywordCampaign.Description1,
                                    description_2 = Helper.keywordCampaign.Description2,
                                    headline_1 = Helper.keywordCampaign.Headline1,
                                    headline_2 = Helper.keywordCampaign.Headline2,
                                    headline_3 = Helper.keywordCampaign.Headline3,
                                };


                                CreateGoCampRequestModel createGoCampRequestModel = new CreateGoCampRequestModel
                                {
                                    ads_type = "google_ads",// google-ads
                                    budget_data = goBudgetData,
                                    keywords_data = goKeywordsData,
                                    fields = goFields,
                                    google_customer_id = Helper.profileModel.google_ad_customer_id,
                                    google_manager_id = Helper.profileModel.google_ad_manager_id,
                                    google_refresh_token = Helper.GoRefreshToken,
                                    responsive_ads_data = goResponsiveAdsData,
                                    user_id = Convert.ToString(Constant.OneTapUserId),
                                };
                                try
                                {
                                    string url = "user/create-campaign";

                                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(createGoCampRequestModel, url, true);
                                    if (rest_result != null)
                                    {
                                        if (rest_result.status_code == 200)
                                        {
                                            var createCampResponse = JsonConvert.DeserializeObject<CreateCampResponseModel>(rest_result.response_body);

                                            if (createCampResponse != null)
                                            {
                                                if (createCampResponse.status)
                                                {
                                                    var a = createCampResponse.result;
                                                    IsBusy = false;
                                                    IsChecking = true;
                                                    await Task.Delay(1000);
                                                    IsChecking = false;
                                                    await nav.PopToRootAsync(true);
                                                    MessagingCenter.Send<object, bool>(this, "RefreshCampaignList", true);
                                                }
                                                else
                                                {
                                                    IsTap = false;

                                                    // Store to database the created campaign
                                                    db.Save(KeywordModelForDB(Helper.keywordCampaign));

                                                    var popupnav = new UserDialogPopup(Constant.PopupTitle, createCampResponse.message, "Ok");
                                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                                    IsBusy = false;

                                                }
                                            }
                                            else
                                            {
                                                IsTap = false;

                                                // Store to database the created campaign
                                                db.Save(KeywordModelForDB(Helper.keywordCampaign));

                                                var popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                                                await PopupNavigation.Instance.PushAsync(popupnav);
                                                IsBusy = false;
                                            }
                                        }
                                        else
                                        {
                                            IsTap = false;

                                            // Store to database the created campaign
                                            db.Save(KeywordModelForDB(Helper.keywordCampaign));

                                            var popupnav = new UserDialogPopup(Constant.PopupTitle, "Can't communicate to server", "Ok");
                                            await PopupNavigation.Instance.PushAsync(popupnav);
                                            IsBusy = false;
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Store to database the created campaign
                                    db.Save(KeywordModelForDB(Helper.keywordCampaign));

                                    IsTap = false;
                                    Debug.WriteLine(ex.Message);
                                    IsBusy = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                IsTap = false;
                                IsBusy = false;

                                // Store to database the created campaign
                                db.Save(KeywordModelForDB(Helper.keywordCampaign));

                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
                IsBusy = false;
            }
        }

        private void Help()
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

        private void Back()
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


        public async Task<FBVideoUploadResponse> UploadVideoAndThumb()
        {
            var options = new RestClientOptions("http://staging.onetapsocial.co/api/user/upload-facebook-ads-video")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1
            };
            var client = new RestClient(options);

            var request = new RestRequest();

            // HEADER IS REQUIRED FOR HTTPS REQUEST (HERE IT IS BEARER TOKEN)
            request.AddHeader("Authorization", "Bearer " + Constant.Token);

            //THE FILES (PARAMETERS) THAT WE WANT TO UPLOAD IN FORMS
            if (Device.RuntimePlatform == Device.Android)
                request.AddFile("ads_video", System.IO.Path.Combine(Helper.thumbfolder, Helper.videoCampaign.Video.FullPath));



            if (Device.RuntimePlatform == Device.iOS)
                request.AddFile("ads_video", Helper.videoCampaign.Video.FullPath);

            request.AddFile("video_thumb", System.IO.Path.Combine(Helper.thumbfolder, Helper.videoCampaign.Thumbname));


            // THE OTHER PARAMETERS THAT ARE NOT FILE BUT SIMPLE STRINGS
            request.AddParameter("fb_ad_account_id", Helper.profileModel.fb_ad_account_id);
            request.AddParameter("fb_access_token", Helper.facebookProfile.Token);

            // RESPONSE WILL BE THE OUTPUT OF CLIENT.EXECUTEASYNC TAKE ALL PARAMETERS CANCELLATIONTOKEN AND METHOD (POST)
            // METHOD TYPE IS COMPULSORY PART.
            var response = await client.ExecuteAsync<RestResponse>(request, Method.Post, cancellationToken: default);
            RestResponseBase restResponseBase = response as RestResponseBase;
            restResponseBase.Content = response.Content;
            Console.WriteLine(response);
            var res = JsonConvert.DeserializeObject<FBVideoUploadResponse>(restResponseBase.Content);
            return res;
        }

        public ImageCampaignModel ImageModelForDB(ImageCampaign imageCampaign)
        {
            if(db.Connection.Table<ImageCampaignModel>() != null)
            {
                db.BulkDelete<ImageCampaignModel>();
            }

            List<FBImageInterest> ImageInterest = new List<FBImageInterest>();
            foreach (var item in imageCampaign.DemoInterest)
            {
                if (item != null)
                {
                    ImageInterest.Add(new FBImageInterest
                    {
                        id = item.id,
                        name = item.name
                    });
                }
            }

            List<FBImageLocation> Targetloc = new List<FBImageLocation>();
            foreach (var item in imageCampaign.TargetLocation)
            {
                if (item != null)
                {
                    Targetloc.Add(new FBImageLocation
                    {
                        country_code = item.country_code,
                        country_name = item.country_name,
                        key = item.key,
                        name = item.name,
                        region = item.region,
                        region_id = item.region_id,
                        type = item.type
                    });
                }
            }

            List<PostCodesResult> PostCodes = new List<PostCodesResult>();
            foreach (var item in imageCampaign.postcodes)
            {
                if (item != null)
                {
                    PostCodes.Add(new PostCodesResult
                    {
                        country_code = item.country_code,
                        country_name = item.country_name,
                        key = item.key,
                        name = item.name,
                        region = item.region,
                        region_id = item.region_id,
                        type = item.type,
                        supports_region = item.supports_region,
                        primary_city = item.primary_city,
                        primary_city_id = item.primary_city_id,
                        supports_city = item.supports_city
                    });
                }
            }

            var temptargetloc = JsonConvert.SerializeObject(Targetloc,Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var temptargetInt = JsonConvert.SerializeObject(ImageInterest, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var tempPostcode = JsonConvert.SerializeObject(PostCodes,Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            ImageCampaignModel ImageDbModel = new ImageCampaignModel
            {
                Objective = imageCampaign.Objective,
                facebook_page_id = imageCampaign.facebook_page_id,
                facebook_page_name = imageCampaign.facebook_page_name,
                Image = string.Empty,
                CroppedImage = string.Empty,
                CampName = imageCampaign.CampName,
                PrimaryText = imageCampaign.PrimaryText,
                Headline = imageCampaign.Headline,
                WebURL = imageCampaign.WebURL,
                ButtonTitle = imageCampaign.ButtonTitle,
                InstaId = imageCampaign.InstaId,
                StartAgeRange = imageCampaign.StartAgeRange,
                EndAgeRange = imageCampaign.EndAgeRange,
                Gender = imageCampaign.Gender,
                Budget = imageCampaign.Budget,
                BudgetType = imageCampaign.BudgetType,
                PaymentMethod = imageCampaign.PaymentMethod,
                StartCampDate = imageCampaign.StartCampDate,
                EndCampDate = imageCampaign.EndCampDate,
                TargetLocation = temptargetloc,
                DemoInterest = temptargetInt,
                PostCodes = tempPostcode
            };
            return ImageDbModel;
        }

        public VideoCampaignModel VideoModelForDB(VideoCampaign videoCampaign)
        {

            if (db.Connection.Table<VideoCampaignModel>() != null)
            {
                db.BulkDelete<VideoCampaignModel>();
            }


            List<FBVideoInterest> VideoInterest = new List<FBVideoInterest>();
            foreach (var item in videoCampaign.DemoInterest)
            {
                if (item != null)
                {
                    VideoInterest.Add(new FBVideoInterest
                    {
                        id = item.id,
                        name = item.name
                    });
                }
            }

            List<FBVideoLocation> Targetloc = new List<FBVideoLocation>();
            foreach (var item in videoCampaign.TargetLocation)
            {
                if (item != null)
                {
                    Targetloc.Add(new FBVideoLocation
                    {
                        country_code = item.country_code,
                        country_name = item.country_name,
                        key = item.key,
                        name = item.name,
                        region = item.region,
                        region_id = item.region_id,
                        type = item.type
                    });
                }
            }

            List<PostCodesResult> PostCodes = new List<PostCodesResult>();
            foreach (var item in videoCampaign.postcodes)
            {
                if (item != null)
                {
                    PostCodes.Add(new PostCodesResult
                    {
                        country_code = item.country_code,
                        country_name = item.country_name,
                        key = item.key,
                        name = item.name,
                        region = item.region,
                        region_id = item.region_id,
                        type = item.type,
                        supports_region = item.supports_region,
                        primary_city = item.primary_city,
                        primary_city_id = item.primary_city_id,
                        supports_city = item.supports_city
                    });
                }
            }

            var tempTargetLocation = JsonConvert.SerializeObject(Targetloc);
            var tempTargetinterest = JsonConvert.SerializeObject(VideoInterest);
            var tempPostcode = JsonConvert.SerializeObject(PostCodes);
            VideoCampaignModel VideoDbModel = new VideoCampaignModel
            {
                Objective = videoCampaign.Objective,
                facebook_page_id = videoCampaign.facebook_page_id,
                facebook_page_name = videoCampaign.facebook_page_name,
                Video = string.Empty,
                SelectedThumb = string.Empty,
                CampName = videoCampaign.CampName,
                PrimaryText = videoCampaign.PrimaryText,
                Headline = videoCampaign.Headline,
                WebURL = videoCampaign.WebURL,
                ButtonTitle = videoCampaign.ButtonTitle,
                InstaId = videoCampaign.InstaId,
                StartAgeRange = videoCampaign.StartAgeRange,
                EndAgeRange = videoCampaign.EndAgeRange,
                Gender = videoCampaign.Gender,
                Budget = videoCampaign.Budget,
                BudgetType = videoCampaign.BudgetType,
                PaymentMethod = videoCampaign.PaymentMethod,
                StartCampDate = videoCampaign.StartCampDate,
                EndCampDate = videoCampaign.EndCampDate,
                video_id = string.Empty,
                Thumbname = string.Empty,
                video_url = string.Empty,
                thumb_url = string.Empty,
                TargetLocation = tempTargetLocation,
                DemoInterest = tempTargetinterest,
                PostCodes = tempPostcode
            };

            return VideoDbModel;
        }

        public KeywordCampaignModel KeywordModelForDB(KeywordCampaign keywordCampaign)
        {
            if (db.Connection.Table<KeywordCampaignModel>() != null)
            {
                db.BulkDelete<KeywordCampaignModel>();
            }

            List<GoadforLocresult> locationresult = new List<GoadforLocresult>();
            foreach (var item in keywordCampaign.TargetLocation.ToList())
            {
                if (item != null)
                {
                    locationresult.Add(new GoadforLocresult { id = item.id, name = item.name });
                }
            }

            var tempTargetLocation = JsonConvert.SerializeObject(locationresult);
            var tempkeywordtheme = JsonConvert.SerializeObject(keywordCampaign.KeywordTheme);

            KeywordCampaignModel keywordCampaignmodel = new KeywordCampaignModel
            {
                Objective = keywordCampaign.Objective,
                CampName = keywordCampaign.CampName,
                Headline1 = keywordCampaign.Headline1,
                Headline2 = keywordCampaign.Headline2,
                Headline3 = keywordCampaign.Headline3,
                WebsiteURL = keywordCampaign.WebsiteURL,
                Description1 = keywordCampaign.Description1,
                Description2 = keywordCampaign.Description2,
                Budget = keywordCampaign.Budget,
                BudgetType = keywordCampaign.BudgetType,
                PaymentMethod = keywordCampaign.PaymentMethod,
                StartCampDate = keywordCampaign.StartCampDate,
                EndCampDate = keywordCampaign.EndCampDate,
                TargetLocation =tempTargetLocation,
                KeywordTheme = tempkeywordtheme
            };

            return keywordCampaignmodel;

        }

        class GoadforLocresult
        {
            public int id;
            public string name;
        }


        #endregion

    }
}
