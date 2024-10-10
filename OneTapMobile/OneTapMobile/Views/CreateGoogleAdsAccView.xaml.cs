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

namespace OneTapMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateGoogleAdsAccView : ContentPage
	{
        bool IsTap = false;
        public CreateGoogleAdsAccView()
		{
			InitializeComponent ();
		}

        private void SignOut_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                    var popupnav = new UserDialogPopup("Confirmation", "Are you sure you want to logout?", true, true, "OK", "Cancel");
                    popupnav.eventOK += Popupnav_eventOK;
                    PopupNavigation.Instance.PushAsync(popupnav);
                    IsTap = false;
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private void Popupnav_eventOK(object sender, EventArgs e)
        {
            Constant.IsLoggedOut = true;
            Helper.facebookProfile = null;
            Constant.FbAdPageadded = false;
            Constant.FbRightArrowVisible = true;
            Constant.SkipNowVisisble = false;
            App.Current.MainPage = new NavigationPage(new LoginView());
            IsTap = false;
        }


    }
}