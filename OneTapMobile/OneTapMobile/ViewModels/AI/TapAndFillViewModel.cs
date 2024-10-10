using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using OneTapMobile.Views.AI;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static OneTapMobile.ViewModels.AddKeywordsViewModel;

namespace OneTapMobile.ViewModels.AI
{
    public class TapAndFillViewModel : BaseViewModel
    {
        #region Properties

        public INavigation nav;
        List<string> listInputs = new List<string>();
        private ObservableCollection<TagsNameModel> _CompanyNameList = new ObservableCollection<TagsNameModel>();
        public ObservableCollection<TagsNameModel> CompanyNameList
        {
            get
            {
                return _CompanyNameList;
            }
            set
            {
                _CompanyNameList = value;
                OnPropertyChanged("CompanyNameList");
            }
        }

        private ObservableCollection<TagsNameModel> _WhoToReachList = new ObservableCollection<TagsNameModel>();
        public ObservableCollection<TagsNameModel> WhoToReachList
        {
            get
            {
                return _WhoToReachList;
            }
            set
            {
                _WhoToReachList = value;
                OnPropertyChanged("WhoToReachList");
            }
        }

        private ObservableCollection<TagsNameModel> _CompanyGoodAtList = new ObservableCollection<TagsNameModel>();
        public ObservableCollection<TagsNameModel> CompanyGoodAtList
        {
            get
            {
                return _CompanyGoodAtList;
            }
            set
            {
                _CompanyGoodAtList = value;
                OnPropertyChanged("CompanyGoodAtList");
            }
        }

        private string _CompanyNameTxt;
        public string CompanyNameTxt
        {
            get
            {
                return _CompanyNameTxt;
            }
            set
            {
                _CompanyNameTxt = value;
                OnPropertyChanged("CompanyNameTxt");
            }
        }

        private string _WhoToReachTxt;
        public string WhoToReachTxt
        {
            get
            {
                return _WhoToReachTxt;
            }
            set
            {
                _WhoToReachTxt = value;
                OnPropertyChanged("WhoToReachTxt");
            }
        }

        private string _CompanyGoodAtTxt;
        public string CompanyGoodAtTxt
        {
            get
            {
                return _CompanyGoodAtTxt;
            }
            set
            {
                _CompanyGoodAtTxt = value;
                OnPropertyChanged("CompanyGoodAtTxt");
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
        private Command continueBtnCommand;
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

        private Command _GenerateAdCmd;
        public Command GenerateAdCommand
        {
            get { return _GenerateAdCmd ?? (_GenerateAdCmd = new Command(async () => await GenerateAd())); }
        }

        #endregion

        #region Methods
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
            catch (System.Exception ex)
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



                // To Be implemented 


                // nav.PushAsync(new GoogleAdReviewView());
            }
            catch (System.Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        #region Generate Ad data from open AI
        async Task GenerateAd()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                IsBusy = true;
                //List<string> list = new List<string>();
                AIRequestModel postmodel = new AIRequestModel();
                listInputs.Clear();
                foreach (var item in CompanyNameList)
                {
                    if (item != null)
                    {
                        listInputs.Add(item.DisplayName);
                    }
                }
                foreach (var item in WhoToReachList)
                {
                    if (item != null)
                    {
                        listInputs.Add(item.DisplayName);
                    }
                }
                foreach (var item in CompanyGoodAtList)
                {
                    if (item != null)
                    {
                        listInputs.Add(item.DisplayName);
                    }
                }

                var d = CompanyNameList.Select(z => z.DisplayName).Where(x => !string.IsNullOrEmpty(x));
                var AddDeatilsphrase = listInputs.Aggregate((partialPhrase, word) => $"{partialPhrase} {word}");
                if (Helper.CreateCampType.ToLower() == "keywords")
                {
                    postmodel.prompt = Constant.GoogleAdsRSAformattext + " " + AddDeatilsphrase;
                }
                else
                {
                    postmodel.prompt = Constant.FaceBookAdsRSAformat + " " + AddDeatilsphrase;
                }
                Rest_ResponseModel rest_result = await WebService.WebService.AIPostData(postmodel, Constant.openaiApi, true, false);
                if (rest_result != null)
                {
                    if (rest_result.status_code == 200)
                    {
                        IsTap = false;
                        var AIResponseModel = JsonConvert.DeserializeObject<OpenAIModel>(rest_result.response_body);
                        if (AIResponseModel != null)
                        {
                            if (Helper.CreateCampType.ToLower() == "keywords")
                            {
                                await GoogleAdsAI(AIResponseModel);
                            }
                            else
                            {
                                await FacebookAdsAI(AIResponseModel);
                            }
                        }
                        IsBusy = false;
                    }
                    else if (rest_result.status_code == 401)
                    {
                        IsTap = false;
                        var errorModel = JsonConvert.DeserializeObject<RootErrorModel>(rest_result.response_body);
                        popupnav = new UserDialogPopup(Constant.PopupTitle, errorModel.error.message, "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                    }
                    else
                    {
                        IsTap = false;
                        popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                    }
                }
                else
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
                IsBusy = false;
            }

        }

        #endregion

        #region Generate For Google Ad AI

        public async Task GoogleAdsAI(OpenAIModel AIResponseModel)
        {
            if (AIResponseModel.choices != null)
            {
                var choice = AIResponseModel.choices.Where(x => x.text != null).ToList().FirstOrDefault();
                if (choice.text != null)
                {
                    string str = choice.text;
                    //string[] delimiters = { "Headline 1:", "Headline 2:","Headline 3:", "Description 1:", "Description 2:" };
                    string[] delimiters = { "\n" };
                    string[] result = str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    string Headline1 = string.Empty;
                    string Headline2 = string.Empty;
                    string Headline3 = string.Empty;
                    string Description1 = string.Empty;
                    string Description2 = string.Empty;
                    foreach (var item in result)
                    {
                        if (item.Contains("Headline 1"))
                        {
                            item.Replace("\"", "");
                            Headline1 = item.ToString().Remove(0, 11);
                            Headline1 = Headline1.TrimStart();
                            if (!string.IsNullOrEmpty(Headline1) && Headline1.Length >= 30)
                                Headline1 = Headline1.Substring(0, 29);
                        }
                        if (item.Contains("Headline 2"))
                        {
                            item.Replace("\"", "");
                            Headline2 = item.ToString().Remove(0, 11);
                            Headline2 = Headline2.TrimStart();
                            if (!string.IsNullOrEmpty(Headline2) && Headline2.Length >= 30)
                                Headline2 = Headline2.Substring(0, 29);
                        }
                        if (item.Contains("Headline 3"))
                        {
                            item.Replace("\"", "");
                            Headline3 = item.ToString().Remove(0, 11);
                            Headline3 = Headline3.TrimStart();
                            if (!string.IsNullOrEmpty(Headline3) && Headline3.Length >= 30)
                                Headline3 = Headline3.Substring(0, 29);
                        }
                        if (item.Contains("Description 1:"))
                        {
                            item.Replace("\"", "");
                            Description1 = item.ToString().Remove(0, 14);
                            Description1 = Description1.TrimStart();
                            if (!string.IsNullOrEmpty(Description1) && Description1.Length > 90)
                                Description1 = Description1.Substring(0, 89);
                        }
                        if (item.Contains("Description 2:"))
                        {
                            item.Replace("\"", "");
                            Description2 = item.ToString().Remove(0, 14);
                            Description2 = Description2.TrimStart();
                            if (!string.IsNullOrEmpty(Description2) && Description2.Length > 90)
                                Description2 = Description2.Substring(0, 89);
                        }
                    }
                    GoAdResponse model = new GoAdResponse
                    {
                        headline_1 = Headline1,
                        headline_2 = Headline2,
                        headline_3 = Headline3,
                        description_1 = Description1,
                        description_2 = Description2,
                    };

                    await nav.PushAsync(new KeywordCampWriteAdView(model,listInputs));
                }
            }
        }

        #endregion

        #region Generate For Facebook Ad AI
        public async Task FacebookAdsAI(OpenAIModel AIResponseModel)
        {
            try
            {
                var choice = AIResponseModel.choices.Where(x => x.text != null).ToList().FirstOrDefault();
                if (choice.text != null)
                {
                    string str = choice.text;
                    string[] delimiters = { "\n" };
                    string[] result = str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    string Headline = string.Empty;
                    string PrimaryText = string.Empty;
                    foreach (var item in result)
                    {
                        if (item.Contains("Headline"))
                        {
                            item.Replace("\"", "");
                            Headline = item.ToString().Remove(0, 9);
                            Headline = Headline.TrimStart();
                            if (!string.IsNullOrEmpty(Headline) && Headline.Length > 40)
                            {
                                Headline = Headline.Substring(0, 40);
                            }
                        }
                        if (item.Contains("Primary Text"))
                        {
                            item.Replace("\"", "");
                            PrimaryText = item.ToString().Remove(0, 13);
                            PrimaryText = PrimaryText.TrimStart();
                        }
                    }
                    FBAdResponse model = new FBAdResponse
                    {
                        headline = Headline,
                        primarytext = PrimaryText,
                    };

                    await nav.PushAsync(new AddCampaignDetailView(model,listInputs));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #endregion

    }
}
