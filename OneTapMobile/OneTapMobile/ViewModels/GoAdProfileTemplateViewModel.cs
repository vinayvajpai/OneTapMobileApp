using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.LocalDataBase;
using OneTapMobile.LocalDataBases.DataBaseModel;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using OneTapMobile.Views.ErrorPage;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class GoAdProfileTemplateViewModel : BaseViewModel
    {
        #region constructor
        public GoAdProfileTemplateViewModel()
        {
            GetGoTemplateList();
        }
        #endregion

        #region properties And Commands

        public INavigation nav;

        InitDatabaseTable db = new InitDatabaseTable();

        private ObservableCollection<GoAdTempResponseResult> _GoAdProfileList = new ObservableCollection<GoAdTempResponseResult>();
        public ObservableCollection<GoAdTempResponseResult> GoAdProfileList
        {
            get
            {
                return _GoAdProfileList;
            }
            set
            {
                _GoAdProfileList = value;
                OnPropertyChanged("GoAdProfileList");
            }
        }

        private GoAdTempResponseResult _CurrentTemplete;

        public GoAdTempResponseResult CurrentTemplate
        {
            get { return _CurrentTemplete; }
            set { _CurrentTemplete = value; OnPropertyChanged("CurrentTemplate"); }
        }


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
                    helpCommand = new Command(Help);
                }

                return helpCommand;
            }
        }

        #endregion

        #region methods
        async void GetGoTemplateList()
        {
            if (!Conn)
            {
                return;
            }
            try
            {
                IsBusy = true;
                GoAdTempRequestModel goAdTempRequestModel = new GoAdTempRequestModel
                {
                    user_id = Convert.ToString(Constant.OneTapUserId)
                };

                string url = "user/google-ads-templates";
                Rest_ResponseModel rest_result = await WebService.WebService.PostData(goAdTempRequestModel, url, true);
                if (rest_result != null)
                {
                    if (rest_result.status_code == 200)
                    {
                        var goAdTempResponseresult = JsonConvert.DeserializeObject<GoAdTempResponsemodel>(rest_result.response_body);

                        foreach (var Templatedata in goAdTempResponseresult.result)
                        {
                            _GoAdProfileList.Add(new GoAdTempResponseResult
                            {
                                status = Templatedata.status,
                                id = Templatedata.id * 100,
                                ad_profile = Templatedata.ad_profile,
                                created_at = Templatedata.created_at,
                                updated_at = Templatedata.updated_at,
                                description_1 = Templatedata.description_1,
                                description_2 = Templatedata.description_2,
                                headline_1 = Templatedata.headline_1,
                                headline_2 = Templatedata.headline_2,
                                headline_3 = Templatedata.headline_3,
                                industry = Templatedata.industry,
                                keywords = Templatedata.keywords
                            });
                        }

                        var previousKeywordCamp = db.Connection.Table<KeywordCampaignModel>();
                        if (previousKeywordCamp != null)
                        {
                            var KeywordCamp = previousKeywordCamp.ToList();
                            foreach (var Templatedata in KeywordCamp)
                            {
                                if (Templatedata != null)
                                {
                                    _GoAdProfileList.Add(new GoAdTempResponseResult
                                    {
                                        status = 1,
                                        id = Templatedata.Id,
                                        ad_profile = string.Empty,
                                        created_at = DateTime.Now,
                                        updated_at = DateTime.Now,
                                        description_1 = Templatedata.Description1,
                                        description_2 = Templatedata.Description2,
                                        headline_1 = Templatedata.Headline1,
                                        headline_2 = Templatedata.Headline2,
                                        headline_3 = Templatedata.Headline3,
                                        industry = Templatedata.Industry,
                                        keywords = Templatedata.Keywords
                                    });
                                }
                            }
                        }

                        GoAdProfileList = _GoAdProfileList;
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

        private void ContinueBtn()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                Helper.keywordCampaign.Headline1 = CurrentTemplate.headline_1;
                Helper.keywordCampaign.Headline2 = CurrentTemplate.headline_2;
                Helper.keywordCampaign.Headline3 = CurrentTemplate.headline_3;
                Helper.keywordCampaign.Description1 = CurrentTemplate.description_1;
                Helper.keywordCampaign.Description2 = CurrentTemplate.description_2;
                Helper.keywordCampaign.Industry = CurrentTemplate.industry;
                Helper.keywordCampaign.Keywords = CurrentTemplate.keywords;
                Helper.keywordCampaign.KeywordTheme = null;
                Helper.keywordCampaign.TargetLocation = null;
                Helper.keywordCampaign.Budget = string.Empty;
                Helper.keywordCampaign.BudgetType = "daily";
                Helper.keywordCampaign.StartCampDate = "Choose Date";
                Helper.keywordCampaign.EndCampDate = "Choose Date";


                var previousKeywordCamp = db.Connection.Table<KeywordCampaignModel>();
                if (previousKeywordCamp != null)
                {
                    if (CurrentTemplate.id % 100 != 0)
                    {
                        var temp = previousKeywordCamp.ToList().Where(x => x.Id == CurrentTemplate.id).FirstOrDefault();
                        if (temp != null)
                        {
                            Helper.keywordCampaign.Headline1 = temp.Headline1;
                            Helper.keywordCampaign.Headline2 = temp.Headline2;
                            Helper.keywordCampaign.Headline3 = temp.Headline3;
                            Helper.keywordCampaign.Description1 = temp.Description1;
                            Helper.keywordCampaign.Description2 = temp.Description2;
                            Helper.keywordCampaign.Industry = temp.Industry;
                            Helper.keywordCampaign.Keywords = temp.Keywords;
                            Helper.keywordCampaign.Budget = temp.Budget;
                            Helper.keywordCampaign.BudgetType = temp.BudgetType;
                            Helper.keywordCampaign.StartCampDate = temp.StartCampDate;
                            Helper.keywordCampaign.EndCampDate = temp.EndCampDate;
                            Helper.keywordCampaign.KeywordTheme = JsonConvert.DeserializeObject<List<string>>(temp.KeywordTheme);
                            Helper.keywordCampaign.TargetLocation = JsonConvert.DeserializeObject<List<GoAdsTarLocResult>>(temp.TargetLocation);
                        }
                    }
                }

                //nav.PushAsync(new KeywordCampWriteAdView());
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

                Helper.keywordCampaign.Headline1 = string.Empty;
                Helper.keywordCampaign.Headline2 = string.Empty;
                Helper.keywordCampaign.Headline3 = string.Empty;
                Helper.keywordCampaign.Description1 = string.Empty;
                Helper.keywordCampaign.Description2 = string.Empty;
                Helper.keywordCampaign.Industry = string.Empty;
                Helper.keywordCampaign.Keywords = string.Empty;
                Helper.keywordCampaign.KeywordTheme = null;
                Helper.keywordCampaign.TargetLocation = null;
                Helper.keywordCampaign.Budget = string.Empty;
                Helper.keywordCampaign.StartCampDate = "Choose Date";
                Helper.keywordCampaign.EndCampDate = "Choose Date";
                Helper.keywordCampaign.BudgetType = "daily";

                //nav.PushAsync(new KeywordCampWriteAdView());
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
