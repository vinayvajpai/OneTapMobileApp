using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        private LoginViewModel m_viewmodel;
        public LoginView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new LoginViewModel();
        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                ClearData();
                IsBusy = false;
                if (m_viewmodel != null)
                {
                    m_viewmodel.nav = this.Navigation;
                    m_viewmodel.IsEnable = true;
                    m_viewmodel.IsTap = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public void ClearData()
        {
            Helper.facebookProfile = null;
            Constant.Token = null;
        }
        protected override bool OnBackButtonPressed()
        {
            var popupnav = new UserDialogPopup("OneTap", "Are you sure you want to exit the application?", true, true, "Ok", "Cancel");
            popupnav.eventOK += Popupnav_eventOK;
            popupnav.eventCancel += Popupnav_eventCancel;
            PopupNavigation.Instance.PushAsync(popupnav);
            return true;
        }
        private void Popupnav_eventCancel(object sender, EventArgs e)
        {
        }
        private void Popupnav_eventOK(object sender, EventArgs e)
        {
            DependencyService.Get<ICloseApplication>().CloseApp();
        }
    }
}