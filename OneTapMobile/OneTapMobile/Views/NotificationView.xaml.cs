using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationView : ContentPage
    {
        UserDialogPopup popupnav;
        public NotificationView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new NotificationViewModel();
            m_viewmodel.AddNotification();
            
        }

        NotificationViewModel m_viewmodel;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                if (m_viewmodel != null)
                {
                    m_viewmodel.nav = this.Navigation;
                    m_viewmodel.IsTap = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}