using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels.AI
{
    public class WriteyouradViewModel : BaseViewModel
    {
        public WriteyouradViewModel(GoAdResponse model)
        {            
            headline1Txt = model.headline_1;
            headline2Txt = model.headline_2;
            headline3Txt = model.headline_3;
            headline4Txt = model.description_1;
            headline4Txt = model.description_1;
            campaignName = model.description_2;
        }

        #region properties
       
        public INavigation nav;

        private string headline1Txt;

        public string Headline1Txt { get => headline1Txt; set => SetProperty(ref headline1Txt, value); }

        private string headline2Txt;

        public string Headline2Txt { get => headline2Txt; set => SetProperty(ref headline2Txt, value); }

        private string headline3Txt;

        public string Headline3Txt { get => headline3Txt; set => SetProperty(ref headline3Txt, value); }
        
        private string headline4Txt;

        public string Headline4Txt { get => headline4Txt; set => SetProperty(ref headline4Txt, value); }

        private string campaignName;

        public string CampaignName { get => campaignName; set => SetProperty(ref campaignName, value); }
        

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
               
                nav.PushAsync(new AddKeywordsView());
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
