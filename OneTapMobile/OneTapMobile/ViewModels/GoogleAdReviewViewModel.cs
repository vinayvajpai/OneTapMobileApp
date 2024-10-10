using OneTapMobile.Global;
using OneTapMobile.Views;
using OneTapMobile.Views.AI;
using OneTapMobile.Views.ErrorPage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class GoogleAdReviewViewModel: BaseViewModel
    {
        #region properties

        public INavigation nav;

        public string _Industry;
        public string Industry
        {
            get
            {
                return _Industry;
            }
            set
            {
                _Industry = value;
                OnPropertyChanged("Industry");
            }
        }
        public string _Headline_1;
        public string Headline_1
        {
            get
            {
                return _Headline_1;
            }
            set
            {
                _Headline_1 = value;
                OnPropertyChanged("Headline_1");
            }
        }
        public string _Headline_2;
        public string Headline_2
        {
            get
            {
                return _Headline_2;
            }
            set
            {
                _Headline_2 = value;
                OnPropertyChanged("Headline_2");
            }
        }
        public string _Headline_3;
        public string Headline_3
        {
            get
            {
                return _Headline_3;
            }
            set
            {
                _Headline_3 = value;
                OnPropertyChanged("Headline_3");
            }
        }
        public string _Description_1;
        public string Description_1
        {
            get
            {
                return _Description_1;
            }
            set
            {
                _Description_1 = value;
                OnPropertyChanged("Description_1");
            }
        }
        public string _Description_2;
        public string Description_2
        {
            get
            {
                return _Description_2;
            }
            set
            {
                _Description_2 = value;
                OnPropertyChanged("Description_2");
            }
        }
        public string _Keywords;
        public string Keywords
        {
            get
            {
                return _Keywords;
            }
            set
            {
                _Keywords = value;
                OnPropertyChanged("Keywords");
            }
        }

        public string _WebURL;
        public string WebURL
        {
            get
            {
                return _WebURL;
            }
            set
            {
                _WebURL = value;
                OnPropertyChanged("WebURL");
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
            catch(Exception ex)
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

        private async void EditAd()
        {
            //try
            //{
            //    if (IsTap)
            //        return;
            //    IsTap = true;
            //    await nav.PopToRootAsync();
            //}
            //catch (Exception ex)
            //{
            //    IsTap = false;
            //    Debug.WriteLine(ex.Message);
            //}

            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                await Helper.PopToPage<TapAndFillView>(this.nav);
                ////nav.PushAsync(new TapAndFillView());
                //for (var counter = 1; counter < 3; counter++)
                //{
                //    nav.RemovePage(nav.NavigationStack[nav.NavigationStack.Count - 2]);
                //}
                //await nav.PopAsync();
                IsBusy = false;
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
                if (IsTap)
                    return;
                IsTap = true;

                if (Helper.keywordCampaign != null)
                {
                    Industry = Helper.keywordCampaign.Industry;
                    Keywords = Helper.keywordCampaign.Keywords;
                    Headline_1 = Helper.keywordCampaign.Headline1;
                    Headline_2 = Helper.keywordCampaign.Headline2;
                    Headline_3 = Helper.keywordCampaign.Headline3;
                    Description_1 = Helper.keywordCampaign.Description1;
                    Description_2 = Helper.keywordCampaign.Description2;
                    WebURL = Helper.keywordCampaign.WebsiteURL;
                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }

        private async void RegenerateAd()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                await Helper.PopToPage<KeywordCampWriteAdView>(this.nav);

                ////nav.PushAsync(new TapAndFillView());
                //for (var counter = 1; counter < 2; counter++)
                //{
                //    nav.RemovePage(nav.NavigationStack[nav.NavigationStack.Count - 1]);
                //}
                //await nav.PopAsync();
                IsBusy = false;
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

