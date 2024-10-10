using OneTapMobile.Global;
using OneTapMobile.Popups;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeshBoardView : ContentPage
    {
        public DeshBoardView()
        {
            InitializeComponent();
        }

        private async void SignOut_Tapped(object sender, EventArgs e)
        {
            var popupnav = new UserDialogPopup("Confrim", "Are you sure you want to logout?", true, true, "OK", "Cancel");
            popupnav.eventOK += Popupnav_eventOK;
            await PopupNavigation.Instance.PushAsync(popupnav);
        }

        private void Popupnav_eventOK(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new LoginView());
        }
    }
}