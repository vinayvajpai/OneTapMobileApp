using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
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
    public partial class TryAnotherGoogleAdAccView : ContentPage
    {
        private TryAnotherGoogleAdAccViewModel m_viewmodel;
        public TryAnotherGoogleAdAccView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new TryAnotherGoogleAdAccViewModel();
            TimeZoneEnty.Focused += TimeZoneEnty_Focused;
            CurrencyEntry.Focused += CurrencyEntry_Focused;
        }

        private void CurrencyEntry_Focused(object sender, FocusEventArgs e)
        {
            var currencyZoneView = new CurrencyTimeZonePopup(true);
            PopupNavigation.Instance.PushAsync(currencyZoneView);
            currencyZoneView.eventDone += Currency_eventDone;
            TimeZoneEnty.Unfocus();
        }

        private void Currency_eventDone(object sender, string e)
        {
            CurrencyEntry.Text = e;
        }

        private void TimeZoneEnty_Focused(object sender, FocusEventArgs e)
        {
            var currencyZoneView = new CurrencyTimeZonePopup(false);
            PopupNavigation.Instance.PushAsync(currencyZoneView);
            currencyZoneView.eventDone += TimeZone_eventDone;
            TimeZoneEnty.Unfocus();
        }

        private void TimeZone_eventDone(object sender, string e)
        {
            TimeZoneEnty.Text = e;
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                
                if (m_viewmodel != null)
                {
                    m_viewmodel.nav = this.Navigation;
                    m_viewmodel.IsBusy = false;
                    m_viewmodel.IsChecking = false;
                    m_viewmodel.IsTap = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        //private void NextButton_Tapped(object sender, EventArgs e)
        //{
        //    IsSuccess = true;
        //    Task.Delay(2000);
        //    Navigation.PushAsync(new DashBoard());
        //}
    }
}