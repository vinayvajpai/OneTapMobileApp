using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using OneTapMobile.Views.ErrorPage;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class KeywordCampWriteAdViewModel : BaseViewModel
    {
        List<string> listInputs = new List<string>();
        public KeywordCampWriteAdViewModel()
        {
        }

        public KeywordCampWriteAdViewModel(List<string> _listInputs)
        {
            this.listInputs = _listInputs;
        }

        #region properties

        public INavigation nav;

        private string headline1Txt;

        public string Headline1Txt { get => headline1Txt; set => SetProperty(ref headline1Txt, value); }

        private string headline2Txt;

        public string Headline2Txt { get => headline2Txt; set => SetProperty(ref headline2Txt, value); }

        private string headline3Txt;

        public string Headline3Txt { get => headline3Txt; set => SetProperty(ref headline3Txt, value); }

        private string desc1Txt;

        public string Desc1Txt { get => desc1Txt; set => SetProperty(ref desc1Txt, value); }

        private string desc2Txt;

        public string Desc2Txt { get => desc2Txt; set => SetProperty(ref desc2Txt, value); }

        #endregion

        #region commands

        private Command helpCommand;

        public Command HelpCommand
        {
            get
            {
                return helpCommand ?? (helpCommand = new Command(() => Help()));
            }
        }

        private Command backCommand;

        public Command BackCommand
        {
            get
            {
                return backCommand ?? (backCommand = new Command(() => Back()));
            }
        }

        private Command continueBtnCommand;

        public Command ContinueBtnCommand
        {
            get
            {
                return continueBtnCommand ?? (continueBtnCommand = new Command(() => ContinueBtn()));
            }
        }

        private Command _GenerateAdCmd;
        public Command GenerateAdCommand
        {
            get { return _GenerateAdCmd ?? (_GenerateAdCmd = new Command(async () => await GenerateAd())); }
        }

        #endregion

        #region methods
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

        private void ContinueBtn()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                Helper.keywordCampaign.Headline1 = Headline1Txt;
                Helper.keywordCampaign.Headline2 = Headline2Txt;
                Helper.keywordCampaign.Headline3 = Headline3Txt;
                Helper.keywordCampaign.Description1 = Desc1Txt;
                Helper.keywordCampaign.Description2 = Desc2Txt;
                nav.PushAsync(new AddKeywordsView());
            }
            catch (Exception ex)
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
                AIRequestModel postmodel = new AIRequestModel();
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
                        if (item.Contains("Description 1:") || item.Contains("Desc 1:"))
                        {
                            item.Replace("\"", "");
                            Description1 = item.ToString().Remove(0, 14);
                            Description1 = Description1.TrimStart();
                            if (!string.IsNullOrEmpty(Description1) && Description1.Length > 90)
                                Description1 = Description1.Substring(0, 89);
                        }
                        if (item.Contains("Description 2:") || item.Contains("Desc 2:"))
                        {
                            item.Replace("\"", "");
                            Description2 = item.ToString().Remove(0, 14);
                            Description2 = Description2.TrimStart();
                            if (!string.IsNullOrEmpty(Description2) && Description2.Length > 90)
                                Description2 = Description2.Substring(0, 89);
                        }
                    }


                    Headline1Txt = Headline1;
                    Headline2Txt = Headline2;
                    Headline3Txt = Headline3;
                    Desc1Txt = Description1;
                    Desc2Txt = Description2;

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

                    await nav.PushAsync(new AddCampaignDetailView(model));
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
