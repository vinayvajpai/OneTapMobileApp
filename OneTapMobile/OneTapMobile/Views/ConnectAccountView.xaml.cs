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
    public partial class ConnectAccountView : ContentPage
    {
        ConnectAccountViewModel m_viewmodel;
        public ConnectAccountView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new ConnectAccountViewModel();

            MessagingCenter.Subscribe<object, bool>(this, "ChangeIsTapToFalse", (sender, arg) =>
            {
                m_viewmodel.IsTap = false;
                m_viewmodel.IsBusy = false;
            });
            
            MessagingCenter.Subscribe<object, bool>(this, "MakeGoogleChecked", (sender, arg) =>
            {
                if(m_viewmodel != null)
                {
                    m_viewmodel.GoogleAdChecked();
                }
            });
        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                IsBusy = false;
                if (m_viewmodel != null)
                {
                    IsBusy = false;
                    m_viewmodel.IsTap = false;
                    m_viewmodel.FbAdChecked();
                    m_viewmodel.GoogleAdChecked();
                }
                m_viewmodel.nav = this.Navigation;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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