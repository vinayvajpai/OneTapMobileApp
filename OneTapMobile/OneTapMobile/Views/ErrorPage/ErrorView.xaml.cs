using OneTapMobile.Global;
using OneTapMobile.Popups;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views.ErrorPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ErrorView : ContentPage
    {
        bool IsTap = false;
        public ErrorView()
        {
            InitializeComponent();
        }

        #region back button pressed method
        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Helper.Goback(this.Navigation);
        }
        #endregion

        #region sign out button pressed method
        void SignOut_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (IsTap)
                    return;
                IsTap= true;
                if (Constant.IsLoggedOut == true)
                {
                    var popupnav1 = new UserDialogPopup("Confirmation", "want to go back to Home page?", true, true, "OK", "Cancel");
                    popupnav1.eventOK += Popupnav_eventOK;
                    PopupNavigation.Instance.PushAsync(popupnav1);
                    IsTap = false;

                }
                else
                {
                    var popupnav = new UserDialogPopup("Confirmation", "Are you sure you want to logout?", true, true, "OK", "Cancel");
                    popupnav.eventOK += Popupnav_eventOK;
                    PopupNavigation.Instance.PushAsync(popupnav);
                    IsTap = false;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        #endregion

        #region popup Ok button clicked override method
        private void Popupnav_eventOK(object sender, EventArgs e)
        {
            Constant.IsLoggedOut = true;
            Helper.facebookProfile = null;
            Constant.FbAdPageadded = false;
            Constant.FbRightArrowVisible = true;
            Constant.SkipNowVisisble = false;
            App.Current.MainPage = new NavigationPage(new LoginView());
            IsTap =false;
        }
        #endregion
    }
}