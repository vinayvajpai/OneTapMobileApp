using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class CampaignOverviewViewModel : BaseViewModel
    {
        #region Constructor
        public CampaignOverviewViewModel(Campaign selecteditem)
        {
            this.selecteditem = selecteditem;
        }

        #endregion

        #region properties

        public INavigation nav;

        private Campaign selecteditem;


        private List<ReportDatum> _ChartDataList = new List<ReportDatum>();
        public List<ReportDatum> ChartDataList
        {
            get { return _ChartDataList; }
            set { _ChartDataList = value; OnPropertyChanged("ChartDataList"); }
        }

        private string campPercent = "0";
        public string CampPercent { get => campPercent; set => SetProperty(ref campPercent, value); }


        private string answer1 = "0";
        public string Answer1 { get => answer1; set => SetProperty(ref answer1, value); }


        private string answer2 = "0";
        public string Answer2 { get => answer2; set => SetProperty(ref answer2, value); }


        private string answer3 = "0";
        public string Answer3 { get => answer3; set => SetProperty(ref answer3, value); }


        private string question1 = "Example Questions 1?";
        public string Question1 { get => question1; set => SetProperty(ref question1, value); }


        private string question2 = "Example Questions 2?";
        public string Question2 { get => question2; set => SetProperty(ref question2, value); }


        private string question3 = "Example Questions 3?";
        public string Question3 { get => question3; set => SetProperty(ref question3, value); }


        private ImageSource thumbNailImage = "ProfilePic";
        public ImageSource ThumbNailImage { get => thumbNailImage; set => SetProperty(ref thumbNailImage, value); }

        private ImageSource _CampTypeIcon = "FBIconWhite";
        public ImageSource CampTypeIcon { get => _CampTypeIcon; set => SetProperty(ref _CampTypeIcon, value); }


        private ImageSource answerIcon1 = "PhoneSmall";
        public ImageSource AnswerIcon1 { get => answerIcon1; set => SetProperty(ref answerIcon1, value); }


        private ImageSource answerIcon2 = "TrendUpViolet";
        public ImageSource AnswerIcon2 { get => answerIcon2; set => SetProperty(ref answerIcon2, value); }


        private ImageSource answerIcon3 = "Plane";
        public ImageSource AnswerIcon3 { get => answerIcon3; set => SetProperty(ref answerIcon3, value); }


        private string todaysSpent = "0";
        public string TodaysSpent { get => todaysSpent; set => SetProperty(ref todaysSpent, value); }


        private ImageSource _CampaignWall;
        public ImageSource CampaignWall
        {
            get { return _CampaignWall; }
            set
            {
                _CampaignWall = value;
                OnPropertyChanged("CampaignWall");
            }
        }

        private bool _CampStatus;
        public bool CampStatus
        {
            get { return _CampStatus; }
            set
            {
                _CampStatus = value;
                OnPropertyChanged("CampStatus");
            }
        }

        private int _CampToggedCount = 0 ;
        public int CampToggedCount
        {
            get { return _CampToggedCount; }
            set
            {
                _CampToggedCount = value;
                OnPropertyChanged("CampToggedCount");
            }
        }

        private bool _ShowGraph = true;
        public bool ShowGraph
        {
            get { return _ShowGraph; }
            set
            {
                _ShowGraph = value;
                OnPropertyChanged("ShowGraph");
            }
        }

        private bool _FirstTimeCalled = false;
        public bool FirstTimeCalled
        {
            get { return _FirstTimeCalled; }
            set
            {
                _FirstTimeCalled = value;
                OnPropertyChanged("FirstTimeCalled");
            }
        }

        private bool _ShowAnswerIcon1 = true;
        public bool ShowAnswerIcon1
        {
            get { return _ShowAnswerIcon1; }
            set
            {
                _ShowAnswerIcon1 = value;
                OnPropertyChanged("ShowAnswerIcon1");
            }
        }

        private bool _ShowAnswerIcon2 = true;
        public bool ShowAnswerIcon2
        {
            get { return _ShowAnswerIcon2; }
            set
            {
                _ShowAnswerIcon2 = value;
                OnPropertyChanged("ShowAnswerIcon2");
            }
        }

        private bool _ShowAnswerIcon3 = true;
        public bool ShowAnswerIcon3
        {
            get { return _ShowAnswerIcon3; }
            set
            {
                _ShowAnswerIcon3 = value;
                OnPropertyChanged("ShowAnswerIcon3");
            }
        }

        private bool _NoGrpah = false;
        public bool NoGrpah
        {
            get { return _NoGrpah; }
            set
            {
                _NoGrpah = value;
                OnPropertyChanged("NoGrpah");
            }
        }

        private double _ChartHeight = 150;
        public double ChartHeight
        {
            get { return _ChartHeight; }
            set
            {
                _ChartHeight = value;
                OnPropertyChanged("ChartHeight");
            }
        }

        private string totalSpent = "0";
        public string TotalSpent { get => totalSpent; set => SetProperty(ref totalSpent, value); }

        private string _SelectedReport_type = "week";
        public string SelectedReport_type
        {
            get
            {
                return _SelectedReport_type;
            }
            set
            {
                _SelectedReport_type = value;
                OnPropertyChanged("SelectedReport_type");
            }
        }

        private double _SpentProgress = 0;
        public double SpentProgress
        {
            get
            {
                return _SpentProgress;
            }
            set
            {
                _SpentProgress = value;
                OnPropertyChanged("SpentProgress");
            }
        }

        private string weekFrameBackground = "#FFFFFF";
        public string WeekFrameBackground
        {
            get
            {
                return weekFrameBackground;
            }
            set
            {
                weekFrameBackground = value;
                OnPropertyChanged("WeekFrameBackground");
            }
        }


        private string monthFrameBackground = "Transparent";
        public string MonthFrameBackground
        {
            get
            {
                return monthFrameBackground;
            }
            set
            {
                monthFrameBackground = value;
                OnPropertyChanged("MonthFrameBackground");
            }
        }


        private string yearFrameBackground = "Transparent";
        public string YearFrameBackground
        {
            get
            {
                return yearFrameBackground;
            }
            set
            {
                yearFrameBackground = value;
                OnPropertyChanged("YearFrameBackground");
            }
        }


        private string _PageTitleTxt = "Campaign Overview";
        public string PageTitleTxt
        {
            get { return _PageTitleTxt; }
            set
            {
                _PageTitleTxt = value;
                OnPropertyChanged("PageTitleTxt");
            }
        }

        private string _CampImage = "loadstarter";
        public string CampImage
        {
            get { return _CampImage; }
            set
            {
                _CampImage = value;
                OnPropertyChanged("CampImage");
            }
        }

        private string _CampaignName = "Campaign name Not available";
        public string CampaignName
        {
            get { return _CampaignName; }
            set
            {
                _CampaignName = value;
                OnPropertyChanged("CampaignName");
            }
        }

        private string _CreatedTime = "Recently Created";
        public string CreatedTime
        {
            get { return _CreatedTime; }
            set
            {
                _CreatedTime = value;
                OnPropertyChanged("CreatedTime");
            }
        }

        private double _ProgressBarData;
        public double ProgressBarData
        {
            get { return _ProgressBarData; }
            set
            {
                _ProgressBarData = value;
                OnPropertyChanged("ProgressBarData");
            }
        }

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

        private Command _TimePeriodCmd;
        public Command TimePeriodCmd
        {
            get
            {
                if (_TimePeriodCmd == null)
                {
                    _TimePeriodCmd = new Command<string>(async (cp) => await PerformTimePeriodCmd(cp)); ;
                }

                return _TimePeriodCmd;
            }
        }

        #endregion

        #region methods
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

        private async Task PerformTimePeriodCmd(string obj)
        {
            WeekFrameBackground = "#00000000";
            MonthFrameBackground = "#00000000";
            YearFrameBackground = "#00000000";
            switch (obj.ToLower())
            {
                case "week":
                    WeekFrameBackground = "#FFFFFF";
                    SelectedReport_type = "week";
                    await GetCampData();
                    break;
                case "month":
                    MonthFrameBackground = "#FFFFFF";
                    SelectedReport_type = "month";
                    await GetCampData();
                    break;
                case "year":
                    YearFrameBackground = "#FFFFFF";
                    SelectedReport_type = "year";
                    await GetCampData();
                    break;
                default:
                    WeekFrameBackground = "#FFFFFF";
                    SelectedReport_type = "week";
                    break;
            }

        }


        public async Task GetCampData()
        {
            if (!Conn)
            {
                return;
            }
            try
            {
                if (selecteditem == null)
                {
                    IsBusy = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "Campaign Detail not found", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    return;
                }
                else
                {
                    FbOverviewRequestModel FboverviewRequestModel = new FbOverviewRequestModel();
                    GoOverviewRequestModel GoogleoverviewRequestModel = new GoOverviewRequestModel();
                    if (selecteditem.type.ToLower() == "facebook" && !string.IsNullOrWhiteSpace(Helper.facebookProfile.Token))
                    {
                        FboverviewRequestModel.ads_type = selecteditem.type;
                        FboverviewRequestModel.campaign_id = Convert.ToString(selecteditem.id);
                        FboverviewRequestModel.fb_access_token = Helper.facebookProfile.Token;
                        FboverviewRequestModel.report_type = SelectedReport_type;
                        FboverviewRequestModel.user_id = Convert.ToString(Constant.OneTapUserId);
                        CampTypeIcon = "FBIconWhite";
                    }

                    if (selecteditem.type.ToLower() == "google" && !string.IsNullOrWhiteSpace(Helper.SelectedGoAdCustDetail.customer_id) && !string.IsNullOrWhiteSpace(Helper.GoRefreshToken))
                    {
                        GoogleoverviewRequestModel.ads_type = selecteditem.type;
                        GoogleoverviewRequestModel.campaign_id = Convert.ToString(selecteditem.id);
                        GoogleoverviewRequestModel.google_ad_customer_id = Helper.profileModel.google_ad_customer_id;
                        GoogleoverviewRequestModel.google_ad_Manager_id = Helper.profileModel.google_ad_manager_id;
                        GoogleoverviewRequestModel.google_refresh_token = Helper.GoRefreshToken;
                        GoogleoverviewRequestModel.report_type = SelectedReport_type;
                        GoogleoverviewRequestModel.user_id = Convert.ToString(Constant.OneTapUserId);
                        CampTypeIcon = "GoogleWhiteSmall";
                    }

                    IsBusy = true;
                    Rest_ResponseModel rest_result = null;
                    string url = "user/campaign-report-data";

                    if (selecteditem.type.ToLower() == "facebook")
                    {
                        rest_result = await WebService.WebService.PostData(FboverviewRequestModel, url, true);
                    }
                    else if (selecteditem.type.ToLower() == "google")
                    {
                        rest_result = await WebService.WebService.PostData(GoogleoverviewRequestModel, url, true);
                    }

                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var overviewResponseModel = JsonConvert.DeserializeObject<OverviewResponseModel>(rest_result.response_body);

                            if (overviewResponseModel != null)
                            {
                                if (overviewResponseModel.status)
                                {
                                    CampToggedCount = CampToggedCount + 1;
                                    IsBusy = false;
                                    CampaignName = selecteditem.name;
                                    ChartDataList = overviewResponseModel.result.report_data;

                                    Question1 = overviewResponseModel.result.question_data[0].question;
                                    Answer1 = overviewResponseModel.result.question_data[0].answer;

                                    if (overviewResponseModel.result.question_data[0].icon_type.ToLower() != "currency")
                                    {
                                        AnswerIcon1 = IconNameFinder(overviewResponseModel.result.question_data[0].icon_type);
                                    }
                                    else
                                    {
                                        AnswerIcon1 = "";
                                        ShowAnswerIcon1 = false;
                                        Answer1 = selecteditem.CurrencySymbol + Answer1;
                                    }


                                    Question2 = overviewResponseModel.result.question_data[1].question;
                                    Answer2 = overviewResponseModel.result.question_data[1].answer;

                                    if (overviewResponseModel.result.question_data[1].icon_type.ToLower() != "currency")
                                    {
                                        AnswerIcon2 = IconNameFinder(overviewResponseModel.result.question_data[1].icon_type);
                                    }
                                    else
                                    {
                                        AnswerIcon2 = "";
                                        ShowAnswerIcon2 = false;
                                        Answer2 = selecteditem.CurrencySymbol + Answer2;
                                    }

                                    Question3 = overviewResponseModel.result.question_data[2].question;
                                    Answer3 = overviewResponseModel.result.question_data[2].answer;

                                    if (overviewResponseModel.result.question_data[2].icon_type.ToLower() != "currency")
                                    {
                                        AnswerIcon3 = IconNameFinder(overviewResponseModel.result.question_data[2].icon_type);
                                    }
                                    else
                                    {
                                        AnswerIcon3 = "";
                                        ShowAnswerIcon3 = false;
                                        Answer3 = selecteditem.CurrencySymbol + Answer3;
                                    }
                                    
                                    PageTitleTxt = overviewResponseModel.result.other_data.campaign_type;
                                    //TotalSpent =string.Format("#.#",overviewResponseModel.result.other_data.total_spent);
                                    TotalSpent = Convert.ToDecimal(overviewResponseModel.result.other_data.total_spent).ToString("0.00");
                                    TodaysSpent = Convert.ToDecimal(overviewResponseModel.result.other_data.today_spend).ToString("0.00");
                                    CampPercent = Convert.ToString(overviewResponseModel.result.other_data.optimization_score_per);
                                    ProgressBarData = Convert.ToDouble(overviewResponseModel.result.other_data.optimization_score_per) / 100;
                                    CreatedTime = overviewResponseModel.result.other_data.created_time;
                                    // campaign status 0 means disabled and 1 means enabled
                                    CampStatus = overviewResponseModel.result.other_data.status== "0"? false : true;
                                    if(CampStatus == false)
                                    {
                                        CampToggedCount++;
                                    }

                                    var TotalSpentValue = Convert.ToDouble(TotalSpent);
                                    var TodaysSpentValue = Convert.ToDouble(TodaysSpent);

                                    if (TotalSpentValue != 0 && TotalSpentValue > TodaysSpentValue)
                                    {
                                        SpentProgress = TodaysSpentValue / TotalSpentValue;
                                    }
                                    else if (TodaysSpentValue == TotalSpentValue && TotalSpentValue != 0)
                                    {
                                        SpentProgress = 1.0;
                                    }

                                    if (SpentProgress == 0)
                                    {
                                        SpentProgress = 0.1;
                                    }

                                    if (!string.IsNullOrWhiteSpace(overviewResponseModel.result.other_data.thumbnail_url))
                                        ThumbNailImage = overviewResponseModel.result.other_data.thumbnail_url;


                                    if (!string.IsNullOrWhiteSpace(overviewResponseModel.result.other_data.bg_image_url))
                                    {
                                        CampaignWall = overviewResponseModel.result.other_data.bg_image_url;
                                    }
                                    else
                                    {
                                        CampaignWall = "";
                                    }
                                }
                                else
                                {
                                    popupnav = new UserDialogPopup(Constant.PopupTitle, overviewResponseModel.message, "Ok");
                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                    IsBusy = false;
                                }
                            }
                            else
                            {
                                popupnav = new UserDialogPopup(Constant.PopupTitle, "Something went wrong Please try again later", "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                            }
                        }
                        else
                        {
                            popupnav = new UserDialogPopup(Constant.PopupTitle, "Can't Communicate to server!!", "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            IsBusy = false;
                        }
                    }
                    else
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, "Can't Communicate to server!!", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                    }
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex.Message);
            }


        }
        public async void ChangeCampstatusCmd()
        {
            try
            {
                if (selecteditem == null)
                {
                    IsBusy = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "Campaign Detail not found", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    return;
                }
                else
                {
                    FBCamapStatusRequestModel fbCamapStatusRequestModel = new FBCamapStatusRequestModel();
                    GoCamapStatusRequestModel goCamapStatusRequestModel = new GoCamapStatusRequestModel();
                    if (selecteditem.type.ToLower() == "facebook" && !string.IsNullOrWhiteSpace(Helper.facebookProfile.Token))
                    {
                        fbCamapStatusRequestModel.user_id = Convert.ToString(Constant.OneTapUserId);
                        fbCamapStatusRequestModel.fb_access_token = Helper.facebookProfile.Token;
                        fbCamapStatusRequestModel.campaign_id = Convert.ToString(selecteditem.id);
                        fbCamapStatusRequestModel.ads_type = selecteditem.type;
                        fbCamapStatusRequestModel.status = Convert.ToInt32(CampStatus);
                    }

                    if (selecteditem.type.ToLower() == "google" && !string.IsNullOrWhiteSpace(Helper.SelectedGoAdCustDetail.customer_id) && !string.IsNullOrWhiteSpace(Helper.GoRefreshToken))
                    {
                        goCamapStatusRequestModel.ads_type = selecteditem.type;
                        goCamapStatusRequestModel.campaign_id = Convert.ToString(selecteditem.id);
                        goCamapStatusRequestModel.google_ad_customer_id = Helper.profileModel.google_ad_customer_id;
                        goCamapStatusRequestModel.google_ad_manager_id = Helper.profileModel.google_ad_manager_id;
                        goCamapStatusRequestModel.google_refresh_token = Helper.GoRefreshToken;
                        goCamapStatusRequestModel.user_id = Convert.ToString(Constant.OneTapUserId);
                        goCamapStatusRequestModel.status = Convert.ToInt32(CampStatus);
                    }

                    IsBusy = true;
                    Rest_ResponseModel rest_result = null;
                    string url = "user/change-campaign-status";

                    if (selecteditem.type.ToLower() == "facebook")
                    {
                        rest_result = await WebService.WebService.PostData(fbCamapStatusRequestModel, url, true);
                    }
                    else if (selecteditem.type.ToLower() == "google")
                    {
                        rest_result = await WebService.WebService.PostData(goCamapStatusRequestModel, url, true);
                    }

                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var campStatusResponseModel = JsonConvert.DeserializeObject<CampStatusResponseModel>(rest_result.response_body);

                            if (campStatusResponseModel != null)
                            {
                                if (campStatusResponseModel.status)
                                {
                                    Debug.WriteLine(campStatusResponseModel.message);
                                    popupnav = new UserDialogPopup(Constant.PopupTitle, campStatusResponseModel.message, "Ok");
                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                    IsBusy = false;
                                }
                                else
                                {
                                    popupnav = new UserDialogPopup(Constant.PopupTitle, campStatusResponseModel.message, "Ok");
                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                    IsBusy = false;
                                }
                            }
                            else
                            {
                                popupnav = new UserDialogPopup(Constant.PopupTitle, "Server issues please try again later", "Ok");
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
                        popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public ImageSource IconNameFinder(string IconType)
        {
            if (IconType != null)
            {
                switch (IconType)
                {
                    case "mouse_click":
                        return "Plane";

                    case "eye":
                        return "VoiletEye";

                    case "times":
                        return "X";

                    case "growth":
                        return "TrendUpViolet";

                    default:
                        return "";
                }
            }
            return "";
        }

        #endregion
    }
}
