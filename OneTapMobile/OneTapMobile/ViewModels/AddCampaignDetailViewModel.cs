using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using OneTapMobile.Views.ErrorPage;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
namespace OneTapMobile.ViewModels
{
    public class AddCampaignDetailViewModel : BaseViewModel
    {
        List<string> listInputs = new List<string>();
        #region constructor
        public AddCampaignDetailViewModel()
        {
            ButtonTitleList();
        }

        public AddCampaignDetailViewModel(List<string> _listInputs)
        {
            ButtonTitleList();
            this.listInputs = _listInputs;
        }

        #endregion
        #region properties
        public INavigation nav;

        public string _CampNameTxt;
        public string CampNameTxt
        {
            get
            {
                return _CampNameTxt;
            }
            set
            {
                _CampNameTxt = value;
                OnPropertyChanged("CampNameTxt");
            }
        }

        private string primaryTxt;
        public string PrimaryTxt
        {
            get => primaryTxt;
            set => SetProperty(ref primaryTxt, value);
        }
        
        
        private string _WebURL;
        public string WebURL
        {
            get => _WebURL;
            set => SetProperty(ref _WebURL, value);
        }
        
        private string headline;
        public string Headline
        {
            get => headline;
            set => SetProperty(ref headline, value);
        }

        private IList<string> buttonTitle = new List<string>();
        public IList<string> ButtonTitle
        {
            get => buttonTitle;
            set => SetProperty(ref buttonTitle, value);
        }

        private string selectedBtnTitle;
        public string SelectedBtnTitle 
        { 
            get => selectedBtnTitle; 
            set => SetProperty(ref selectedBtnTitle, value); 
        }

        private bool instaGrayTick = true;
        public bool InstaGrayTick
        {
            get => instaGrayTick;
            set => SetProperty(ref instaGrayTick, value);
        }

        private bool instaGreenTick = false;
        public bool InstaGreenTick
        {
            get => instaGreenTick;
            set => SetProperty(ref instaGreenTick, value);
        }

        private bool instaSelected = false;
        public bool InstaSelected
        {
            get => instaSelected;
            set => SetProperty(ref instaSelected, value);
        }

        #endregion

        #region Commands
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

        private Command backCommand;
        public Command BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new Command(Backbtn);
                }
                return backCommand;
            }
        }

        private Command helpCommand;
        public Command HelpCommand
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

        private Command addInstagramAccCMd;
        public Command AddInstagramAccCMd
        {
            get
            {
                if (addInstagramAccCMd == null)
                {
                    addInstagramAccCMd = new Command(PerformAddInstagramAccCMd);
                }
                return addInstagramAccCMd;
            }
        }

        private Command _GenerateAdCmd;
        public Command GenerateAdCommand
        {
            get { return _GenerateAdCmd ?? (_GenerateAdCmd = new Command(async () => await GenerateAd())); }
        }

        #endregion

        #region methods
        private void PerformAddInstagramAccCMd()
        { 
            if (!InstaSelected)
            {
                InstaGrayTick = false;
                InstaGreenTick = true;
                InstaSelected = true;
            }
        else
            {
                InstaGrayTick = true;
                InstaGreenTick = false;
                InstaSelected = false;
            }

        }
        public void ButtonTitleList()
        {
            buttonTitle = new List<string>();
            if(Helper.videoCampaign.Video != null)
            {
                if (!string.IsNullOrEmpty(Helper.videoCampaign.ButtonTitle))
                {
                    buttonTitle.Add(Helper.videoCampaign.ButtonTitle.Replace("_", " ").ToLower());
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Helper.imageCampaign.ButtonTitle))
                {
                    buttonTitle.Add(Helper.imageCampaign.ButtonTitle.Replace("_", " ").ToLower());
                }
            }
            buttonTitle.Add("open link");
            buttonTitle.Add("shop now");
            buttonTitle.Add("learn more");
            buttonTitle.Add("get offer");
            buttonTitle.Add("contact us");
            buttonTitle.Add("see more");
            // buttonTitle.Add("Watch Video");
            //buttonTitle.Add("Download App");
            //buttonTitle.Add("Learn More");
            //buttonTitle.Add("Watch Video");
            //buttonTitle.Add("See Offers");
            //buttonTitle.Add("Shop Now");
            var unique_items = new HashSet<string>(buttonTitle);
                buttonTitle = unique_items.ToList();
            ButtonTitle = buttonTitle;
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
                Debug.WriteLine(ex.Message);
                IsTap = false;
            }
            
        }
        private void Backbtn()
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
                Debug.WriteLine(ex.Message);
                IsTap = false;
            }

        }

        private void ContinueBtn()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                IsBusy = true;

                //if (InstaSelected)
                //{
                //    Helper.imageCampaign.InstaId = Helper.videoCampaign.InstaId = "Shubhansh_15";
                //}
                //else
                //{
                //    Helper.imageCampaign.InstaId = Helper.videoCampaign.InstaId = null;
                //}

                if (!string.IsNullOrWhiteSpace(CampNameTxt ?? primaryTxt ?? headline) && Helper.imageCampaign.ButtonTitle != "Select Button Title")
                {
                    Helper.imageCampaign.CampName = Helper.videoCampaign.CampName = CampNameTxt;
                    Helper.imageCampaign.PrimaryText = Helper.videoCampaign.PrimaryText = primaryTxt;
                    Helper.imageCampaign.Headline = Helper.videoCampaign.Headline = headline;
                    Helper.imageCampaign.WebURL = Helper.videoCampaign.WebURL = WebURL;
                    Helper.imageCampaign.ButtonTitle = Helper.videoCampaign.ButtonTitle = buttonTitle[Convert.ToInt32(selectedBtnTitle)].Replace(" ","_").ToUpper();
                    nav.PushAsync(new CreateYourAudienceView());
                    IsBusy = false;
                }
                else
                {
                    IsBusy = false;
                    IsTap = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "please Select Button Title!", "Ok");
                    PopupNavigation.Instance.PushAsync(popupnav);

                }
            }
            catch (Exception)
            {
                IsTap = false;
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
                            await FacebookAdsAI(AIResponseModel);
                           
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
                    PrimaryTxt = PrimaryText;
                    headline = Headline;
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
