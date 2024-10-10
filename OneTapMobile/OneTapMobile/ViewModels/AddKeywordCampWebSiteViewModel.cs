using OneTapMobile.Global;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using OneTapMobile.Views.AI;
using OneTapMobile.Views.ErrorPage;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class AddKeywordCampWebSiteViewModel : BaseViewModel
    {
        #region properties

        public INavigation nav;

        private string campNameTxt;
        public string CampNameTxt
        {
            get => campNameTxt; 
            set => SetProperty(ref campNameTxt, value);
        }
        
        
        private string webURLTxt;

        public string WebURLTxt
        {
            get => webURLTxt; 
            set => SetProperty(ref webURLTxt, value);
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
        #endregion

        #region Methods
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

                Helper.keywordCampaign.CampName = CampNameTxt;

                if (WebURLTxt.ToLower().Contains("https://"))
                {
                    Helper.keywordCampaign.WebsiteURL = WebURLTxt.ToLower();
                }
                else
                {
                    Helper.keywordCampaign.WebsiteURL = "https://" + WebURLTxt.ToLower();
                }

                if (Regex.IsMatch(Helper.keywordCampaign.WebsiteURL, @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$"))
                {
                    //nav.PushAsync(new GoAdprofileTemplateView());
                    nav.PushAsync(new TapAndFillView());
                }
                else
                {
                    var popupnav = new UserDialogPopup(Constant.PopupTitle, "please input a valid Web URL", "Ok");
                    PopupNavigation.Instance.PushAsync(popupnav);
                    IsTap = false;
                }
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
