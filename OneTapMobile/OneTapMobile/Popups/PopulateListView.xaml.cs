using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.ViewModels;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopulateListView : PopupPage
    {
        //public bool ListForLoc { get; set; } = true;
        public FBCitiesResult FBCitySelected;
        public FBInterestResult FBInterestSelected;

        PopulateListViewModel m_viewmodel;
        public PopulateListView(bool ListForLocation)
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new PopulateListViewModel();
            this.CloseWhenBackgroundIsClicked = true;
            m_viewmodel.ListForLoc = ListForLocation;
        }

        protected override void OnAppearing()
        {
            try
            {
                IsBusy = false;
                if (m_viewmodel.ListForLoc)
                {
                    m_viewmodel.SearchBoxTitle = "Search Locations";
                    CitiesList.IsVisible = true;
                    InterestList.IsVisible = false;
                }
                else
                {
                    m_viewmodel.SearchBoxTitle = "Search Interests";
                    CitiesList.IsVisible = false;
                    InterestList.IsVisible = true;
                }
                base.OnAppearing();
                SearchLoc.Focus();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void SearchLoc_Completed(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                _ = m_viewmodel.IsListForLocMethod(m_viewmodel.ListForLoc);
            });
        }

        private void SearchLoc_Completed(object sender, TextChangedEventArgs e)
        {
            Task.Run(async () =>
            {
                await m_viewmodel.IsListForLocMethod(m_viewmodel.ListForLoc);
            });
        }
    }
}