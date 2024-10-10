using OneTapMobile.Global;
using OneTapMobile.Views;
using OneTapMobile.Views.AI;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class AdReviewViewModel : BaseViewModel
    {

        #region properties
        public INavigation nav;

        private ImageSource _CompanyIcon = string.Empty;
        public ImageSource CompanyIcon
        {
            get
            {
                return _CompanyIcon;
            }
            set
            {
                _CompanyIcon = value;
                OnPropertyChanged("CompanyIcon");
            }
        }

        private string _CampNametxt = string.Empty;
        public string CampNametxt
        {
            get
            {
                return _CampNametxt;
            }
            set
            {
                _CampNametxt = value;
                OnPropertyChanged("CampNametxt");
            }
        }
        private string _FacebookPageName = string.Empty;
        public string FacebookPageName
        {
            get
            {
                return _FacebookPageName;
            }
            set
            {
                _FacebookPageName = value;
                OnPropertyChanged("FacebookPageName");
            }
        }


        private string _VideoSource;
        public string VideoSource
        {
            get
            {
                return _VideoSource;
            }
            set
            {
                _VideoSource = value;
                OnPropertyChanged("VideoSource");
            }
        }

        private bool _ShowImagePreview = true;
        public bool ShowImagePreview
        {
            get
            {
                return _ShowImagePreview;
            }
            set
            {
                _ShowImagePreview = value;
                OnPropertyChanged("ShowImagePreview");
            }
        }

        private bool _ShowVideoPreview = false;
        public bool ShowVideoPreview
        {
            get
            {
                return _ShowVideoPreview;
            }
            set
            {
                _ShowVideoPreview = value;
                OnPropertyChanged("ShowVideoPreview");
            }
        }


        private ImageSource _ButtonImage = string.Empty;
        public ImageSource ButtonImage
        {
            get
            {
                return _ButtonImage;
            }
            set
            {
                _ButtonImage = value;
                OnPropertyChanged("ButtonImage");
            }
        }

        private string _ButtonTitleTxt = string.Empty;
        public string ButtonTitleTxt
        {
            get
            {
                return _ButtonTitleTxt;
            }
            set
            {
                _ButtonTitleTxt = value;
                OnPropertyChanged("ButtonTitleTxt");
            }
        }


        private ImageSource _UploadedImage;
        public ImageSource UploadedImage
        {
            get
            {
                return _UploadedImage;
            }
            set
            {
                _UploadedImage = value;
                OnPropertyChanged("UploadedImage");
            }
        }

        private string _HeadLineText = string.Empty;
        public string HeadLineText
        {
            get
            {
                return _HeadLineText;
            }
            set
            {
                _HeadLineText = value;
                OnPropertyChanged("HeadLineText");
            }
        }

        private string _PrimaryText = string.Empty;
        public string PrimaryText
        {
            get
            {
                return _PrimaryText;
            }
            set
            {
                _PrimaryText = value;
                OnPropertyChanged("PrimaryText");
            }
        }



        private string _Description = string.Empty;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        private string _WebSiteUrl = string.Empty;
        public string WebSiteUrl
        {
            get
            {
                return _WebSiteUrl;
            }
            set
            {
                _WebSiteUrl = value;
                OnPropertyChanged("WebSiteUrl");
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

        private Command editAdCommand;

        public ICommand EditAdCommand
        {
            get
            {
                if (editAdCommand == null)
                {
                    editAdCommand = new Command(EditAd);
                }

                return editAdCommand;
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

        private Command regenerateAdCmd;

        public ICommand RegenerateAdCmd
        {
            get
            {
                if (regenerateAdCmd == null)
                {
                    regenerateAdCmd = new Command(RegenerateAd);
                }

                return regenerateAdCmd;
            }
        }

        #endregion

        #region methods

        private async void RegenerateAd()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                await Helper.PopToPage<AddCampaignDetailView>(this.nav);

                IsBusy = false;
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

                nav.PushAsync(new CampaignBudgetView());
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
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

        public async Task GetAdReviewData()
        {
            try
            {
                if (Helper.CreateCampType.ToLower() == "image")
                {
                    HeadLineText = Helper.imageCampaign.Headline;
                    CompanyIcon = "";
                    CampNametxt = Helper.imageCampaign.CampName;
                    FacebookPageName = Helper.imageCampaign.facebook_page_name;
                    WebSiteUrl = Helper.imageCampaign.WebURL;
                    ButtonImage = "ThumbsUpPink";

                    //ButtonTitleTxt = Helper.imageCampaign.ButtonTitle.Replace("_"," ");
                    ButtonTitleTxt = CapFirstCharecter(Helper.imageCampaign.ButtonTitle.Replace("_", " "));
                    UploadedImage = Helper.imageCampaign.CroppedImage;
                    PrimaryText = Helper.imageCampaign.PrimaryText;
                    ShowVideoPreview = false;
                    ShowImagePreview = true;
                }
                else if (Helper.CreateCampType.ToLower() == "video")
                {
                    HeadLineText = Helper.videoCampaign.Headline;
                    CompanyIcon = "";
                    CampNametxt = Helper.videoCampaign.CampName;
                    FacebookPageName = Helper.videoCampaign.facebook_page_name;
                    WebSiteUrl = Helper.videoCampaign.WebURL;
                    ButtonImage = "ThumbsUpPink";
                    //ButtonTitleTxt = Helper.videoCampaign.ButtonTitle.Replace("_", " ");
                    ButtonTitleTxt = CapFirstCharecter(Helper.imageCampaign.ButtonTitle.Replace("_", " "));
                    PrimaryText = Helper.videoCampaign.PrimaryText;
                    UploadedImage = Helper.videoCampaign.SelectedThumb;
                    // VideoSource = Helper.videoCampaign.Video.FullPath;
                    ShowVideoPreview = true;
                    ShowImagePreview = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private async void EditAd()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                //await nav.PopToRootAsync();
                await Helper.PopToPage<TapAndFillView>(this.nav);
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private string CapFirstCharecter(string LetterString)
        {
            try
            {
                return LetterString.CapitalizeFirst();

                //var Capitals = LetterString.ToUpper();
                //var Smalls = LetterString.ToLower();
                //var ind = LetterString.IndexOf(" ");

                //if (ind != -1)
                //{ 
                //    var Secondcapital = Smalls.Replace(Smalls[ind + 1], Capitals[ind + 1]);
                //    Debug.WriteLine(Secondcapital);

                //    var charecters = Secondcapital.ToCharArray();

                //    charecters[0] = Capitals[0];

                //    Debug.WriteLine(new string(charecters));
                //    return new string(charecters);
                //}
                //else
                //{
                //    return LetterString;
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return LetterString;
            }
        }
        #endregion
    }
}
