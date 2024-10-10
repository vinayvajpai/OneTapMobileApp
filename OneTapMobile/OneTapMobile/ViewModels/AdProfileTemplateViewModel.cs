using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.LocalDataBase;
using OneTapMobile.LocalDataBases.DataBaseModel;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace OneTapMobile.ViewModels
{
    public class AdProfileTemplateViewModel : BaseViewModel
    {

        #region Constructor
        public AdProfileTemplateViewModel(ImageSource _adImage)
        {
            adImage = _adImage;
            GetTemplateList();
        }
        #endregion

        #region Properties

        public ImageSource adImage;
        public INavigation nav;

        InitDatabaseTable db = new InitDatabaseTable();

        private ObservableCollection<AdProfileResponseResult> _AdProfileList = new ObservableCollection<AdProfileResponseResult>();
        public ObservableCollection<AdProfileResponseResult> AdProfileList
        {
            get
            {
                return _AdProfileList;
            }
            set
            {
                _AdProfileList = value;
                OnPropertyChanged("AdProfileList");
            }
        }


        public string _FBPageName;
        public string FBPageName
        {
            get
            {
                return _FBPageName;
            }
            set
            {
                _FBPageName = value;
                OnPropertyChanged("FBPageName");
            }
        }

        private AdProfileResponseResult _CurrentTemplete;

        public AdProfileResponseResult CurrentTemplate
        {
            get { return _CurrentTemplete; }
            set { _CurrentTemplete = value; OnPropertyChanged("CurrentTemplate"); }
        }

        #endregion

        #region Commands

        private ICommand continueBtnCommand;
        public ICommand ContinueBtnCommand
        {
            get
            {
                if (continueBtnCommand == null)
                {
                    continueBtnCommand = new Command(ContinueBtn);
                }

                return continueBtnCommand;
            }
        }


        private ICommand _SkipTemplateCmd;
        public ICommand SkipTemplateCmd
        {
            get
            {
                if (_SkipTemplateCmd == null)
                {
                    _SkipTemplateCmd = new Command(SkipTemplateCommand);
                }

                return _SkipTemplateCmd;
            }
        }

        private ICommand _BackCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_BackCommand == null)
                {
                    _BackCommand = new Command(BackBtnCommand);
                }

                return _BackCommand;
            }
        }

        private Command helpCommand;
        public ICommand HelpCommand
        {
            get
            {
                if (helpCommand == null)
                {
                    helpCommand = new Command(HelpCmd);
                }

                return helpCommand;
            }
        }

        #endregion

        #region methods
        private void HelpCmd()
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

        private void BackBtnCommand()
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
        async void GetTemplateList()
        {
            if (!Conn)
            {
                return;
            }
            try
            {
                IsBusy = true;
                AdProfileRequestModel adProfileRequestModel = new AdProfileRequestModel
                {
                    user_id = Convert.ToString(Constant.OneTapUserId)
                };

                string url = "user/facebook-ads-templates";
                Rest_ResponseModel rest_result = await WebService.WebService.PostData(adProfileRequestModel, url, true);
                if (rest_result != null)
                {
                    if (rest_result.status_code == 200)
                    {
                        var AdprofileResponseResult = JsonConvert.DeserializeObject<AdProfileResponseModel>(rest_result.response_body);

                        if (Helper.CreateCampType == "image")
                            FBPageName = Helper.imageCampaign.facebook_page_name;
                        else
                            FBPageName = Helper.videoCampaign.facebook_page_name;


                        foreach (var Templatedata in AdprofileResponseResult.result)
                        {
                            _AdProfileList.Add(new AdProfileResponseResult
                            {
                                fbPageName = FBPageName,
                                button_title = Templatedata.button_title,
                                ad_profile = Templatedata.ad_profile,
                                campaign_name = Templatedata.campaign_name,
                                created_at = Templatedata.created_at,
                                headline = Templatedata.headline,
                                id = Templatedata.id * 100,
                                industry = Templatedata.industry,
                                primary_text = Templatedata.primary_text,
                                status = Templatedata.status,
                                updated_at = Templatedata.updated_at,
                                AdImageVideo = adImage
                            });
                        }

                        if (Helper.CreateCampType.ToLower() == "image")
                        {
                            var previousImageCamp = db.Connection.Table<ImageCampaignModel>();
                            if (previousImageCamp != null)
                            {
                                var ImageTemplate = previousImageCamp.ToList();
                                foreach (var Templatedata in ImageTemplate)
                                {
                                    if (Templatedata != null)
                                    {
                                        _AdProfileList.Add(new AdProfileResponseResult
                                        {
                                            fbPageName = FBPageName,
                                            button_title = Templatedata.ButtonTitle,
                                            ad_profile = string.Empty,
                                            campaign_name = Templatedata.CampName,
                                            created_at = DateTime.Now,
                                            headline = Templatedata.Headline,
                                            id = Templatedata.Id,
                                            industry = string.Empty,
                                            primary_text = Templatedata.PrimaryText,
                                            status = 1,
                                            updated_at = null,
                                            AdImageVideo = adImage
                                        });
                                    }
                                }
                            }

                        }

                        if (Helper.CreateCampType.ToLower() == "video")
                        {
                            var previousVideoCamp = db.Connection.Table<VideoCampaignModel>();
                            if (previousVideoCamp != null)
                            {
                                var Template = previousVideoCamp.ToList();
                                foreach (var Templatedata in Template)
                                {
                                    _AdProfileList.Add(new AdProfileResponseResult
                                    {
                                        fbPageName = FBPageName,
                                        button_title = Templatedata.ButtonTitle,
                                        ad_profile = string.Empty,
                                        campaign_name = Templatedata.CampName,
                                        created_at = DateTime.Now,
                                        headline = Templatedata.Headline,
                                        id = Templatedata.Id,
                                        industry = string.Empty,
                                        primary_text = Templatedata.PrimaryText,
                                        status = 1,
                                        updated_at = null,
                                        AdImageVideo = adImage
                                    });
                                }
                            }
                        }
                        AdProfileList = _AdProfileList;
                        IsBusy = false;
                    }
                    else
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                    }
                }
                else
                {
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "can't communicate with server", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    IsBusy = false;
                }

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsBusy = false;
            }
        }



        private void ContinueBtn(object obj)
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                if (CurrentTemplate != null)
                {
                    Helper.imageCampaign.Headline = Helper.videoCampaign.Headline = CurrentTemplate.headline;
                    Helper.imageCampaign.CampName = Helper.videoCampaign.CampName = CurrentTemplate.campaign_name;
                    Helper.imageCampaign.PrimaryText = Helper.videoCampaign.PrimaryText = CurrentTemplate.primary_text;
                    Helper.imageCampaign.ButtonTitle = Helper.videoCampaign.ButtonTitle = CurrentTemplate.button_title;
                    Helper.imageCampaign.TargetLocation = Helper.videoCampaign.TargetLocation = null;
                    Helper.imageCampaign.DemoInterest = Helper.videoCampaign.DemoInterest = null;
                    Helper.imageCampaign.Budget = Helper.videoCampaign.Budget= string.Empty;
                    Helper.imageCampaign.BudgetType = Helper.videoCampaign.BudgetType= string.Empty;
                    Helper.imageCampaign.StartCampDate = Helper.videoCampaign.StartCampDate = "Choose Date";
                    Helper.imageCampaign.EndCampDate = Helper.videoCampaign.EndCampDate = "Choose Date";
                    Helper.imageCampaign.StartAgeRange = Helper.videoCampaign.StartAgeRange = 0;
                    Helper.imageCampaign.EndAgeRange = Helper.videoCampaign.EndAgeRange = 0;
                    Helper.imageCampaign.WebURL = Helper.videoCampaign.WebURL = string.Empty;
                    Helper.imageCampaign.postcodes = Helper.videoCampaign.postcodes = null;
                    if (CurrentTemplate.id % 100 != 0)
                    {

                        if (Helper.CreateCampType.ToLower() == "image")
                        {
                            var previousImageCamp = db.Connection.Table<ImageCampaignModel>();
                            if (previousImageCamp != null)
                            {
                                var temp = previousImageCamp.ToList().Where(x => x.Id == CurrentTemplate.id).FirstOrDefault();

                                Helper.imageCampaign.WebURL = temp.WebURL;
                                Helper.imageCampaign.Gender = temp.Gender;
                                Helper.imageCampaign.StartAgeRange = temp.StartAgeRange;
                                Helper.imageCampaign.EndAgeRange = temp.EndAgeRange;
                                Helper.imageCampaign.StartCampDate = temp.StartCampDate;
                                Helper.imageCampaign.EndCampDate = temp.EndCampDate;
                                Helper.imageCampaign.Budget = temp.Budget;
                                Helper.imageCampaign.BudgetType = temp.BudgetType;
                                Helper.imageCampaign.Gender = temp.Gender;
                                Helper.imageCampaign.PrimaryText = temp.PrimaryText;
                                // Helper.imageCampaign.Objective = temp.Objective;
                                Helper.imageCampaign.ButtonTitle = temp.ButtonTitle;
                                Helper.imageCampaign.postcodes = new List<PostCodesResult>();
                                Helper.imageCampaign.TargetLocation = new List<FBCitiesResult>();
                                Helper.imageCampaign.DemoInterest = new List<FBInterestResult>();

                                var previouslocations = JsonConvert.DeserializeObject<List<FBImageLocation>>(temp.TargetLocation);

                                if (previouslocations != null)
                                {
                                    foreach (var element in previouslocations)
                                    {


                                        Helper.imageCampaign.TargetLocation.Add(new FBCitiesResult
                                        {
                                            key = element.key,
                                            name = element.name,
                                            type = element.type,
                                            country_code = element.country_code,
                                            country_name = element.country_name,
                                            region = element.region,
                                            region_id = Convert.ToInt32(element.region_id),
                                        });
                                    }

                                    var previousInterests = JsonConvert.DeserializeObject<List<FBImageInterest>>(temp.DemoInterest);

                                    foreach (var element in previousInterests)
                                    {
                                        Helper.imageCampaign.DemoInterest.Add(new FBInterestResult
                                        {
                                            id = element.id,
                                            name = element.name,
                                        });
                                    }

                                    var previousPostcodes = JsonConvert.DeserializeObject<List<PostCodesResult>>(temp.PostCodes);
                                    List<PostCodesResult> PostCodes = new List<PostCodesResult>();
                                    foreach (var item in previousPostcodes)
                                    {
                                        if (item != null)
                                        {
                                            Helper.imageCampaign.postcodes.Add(new PostCodesResult
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
                                }
                            }
                        }


                        if (Helper.CreateCampType.ToLower() == "video")
                        {
                            var previousVideoCamp = db.Connection.Table<VideoCampaignModel>();
                            if (previousVideoCamp != null)
                            {

                                var temp = previousVideoCamp.ToList().Where(x => x.Id.ToString().ToLower() == CurrentTemplate.id.ToString().ToLower()).FirstOrDefault();

                                Helper.videoCampaign.WebURL = temp.WebURL;
                                Helper.videoCampaign.Gender = temp.Gender;
                                Helper.videoCampaign.StartAgeRange = temp.StartAgeRange;
                                Helper.videoCampaign.EndAgeRange = temp.EndAgeRange;
                                Helper.videoCampaign.Gender = temp.Gender;
                                Helper.videoCampaign.StartCampDate = temp.StartCampDate;
                                Helper.videoCampaign.EndCampDate = temp.EndCampDate;
                                Helper.videoCampaign.Budget = temp.Budget;
                                Helper.videoCampaign.BudgetType = temp.BudgetType;
                                Helper.videoCampaign.PrimaryText = temp.PrimaryText;
                                //Helper.videoCampaign.Objective = temp.Objective;
                                Helper.videoCampaign.ButtonTitle = temp.ButtonTitle;

                                Helper.videoCampaign.TargetLocation = new List<FBCitiesResult>();
                                Helper.videoCampaign.DemoInterest = new List<FBInterestResult>();

                                var previouslocations = JsonConvert.DeserializeObject<List<FBVideoLocation>>(temp.TargetLocation);

                                if (previouslocations != null)
                                {
                                    foreach (var element in previouslocations)
                                    {
                                        Helper.videoCampaign.TargetLocation.Add(new FBCitiesResult
                                        {
                                            key = element.key,
                                            name = element.name,
                                            type = element.type,
                                            country_code = element.country_code,
                                            country_name = element.country_name,
                                            region = element.region,
                                            region_id = element.region_id,
                                        });
                                    }

                                    var previousInterests = JsonConvert.DeserializeObject<List<FBVideoInterest>>(temp.DemoInterest);

                                    foreach (var element in previousInterests)
                                    {

                                        Helper.videoCampaign.DemoInterest.Add(new FBInterestResult
                                        {
                                            id = element.id,
                                            name = element.name,
                                        });
                                    }

                                    var previousPostcodes = JsonConvert.DeserializeObject<List<PostCodesResult>>(temp.PostCodes);
                                    List<PostCodesResult> PostCodes = new List<PostCodesResult>();
                                    foreach (var item in previousPostcodes)
                                    {
                                        if (item != null)
                                        {
                                            Helper.imageCampaign.postcodes.Add(new PostCodesResult
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
                                }
                            }
                        }
                    }
                }

                nav.PushAsync(new AddCampaignDetailView());
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }

        private void SkipTemplateCommand(object obj)
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                if (CurrentTemplate != null)
                {
                    Helper.imageCampaign.Headline = string.Empty;
                    Helper.videoCampaign.Headline = string.Empty;
                    Helper.imageCampaign.CampName = string.Empty;
                    Helper.videoCampaign.CampName = string.Empty;
                    Helper.imageCampaign.WebURL = string.Empty;
                    Helper.imageCampaign.PrimaryText = string.Empty;
                    Helper.videoCampaign.PrimaryText = string.Empty;
                    Helper.imageCampaign.ButtonTitle = string.Empty;
                    Helper.videoCampaign.ButtonTitle = string.Empty;
                    Helper.videoCampaign.WebURL = string.Empty;
                }

                nav.PushAsync(new AddCampaignDetailView());
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

