using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoogleAdsTarLocPopup : PopupPage
    {
        public GoAdsTarLocResult FBCitySelected;
        GoAdsTarLocViewModel m_viewmodel;
        public GoogleAdsTarLocPopup()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new GoAdsTarLocViewModel();
            this.CloseWhenBackgroundIsClicked = true;
        }

        protected override void OnAppearing()
        {
            if(m_viewmodel != null)
                _= m_viewmodel.SearchLocations();

            base.OnAppearing();
            SearchLoc.Focus();
        }

        private void SearchLoc_Completed(object sender, EventArgs e)
        {
            _ = m_viewmodel.SearchLocationsFromMaster();
        }

        private void SearchLoc_TextChanged(object sender, TextChangedEventArgs e)
        {
            _ = m_viewmodel.SearchLocationsFromMaster();
        }
    }
}