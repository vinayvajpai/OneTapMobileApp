using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace OneTapMobile.Popups
{
    public partial class PostcodeSearchListView : PopupPage
    {
        //public bool ListForLoc { get; set; } = true;
        public Models.FBCitiesResult FBCitySelected;
        public Models.FBInterestResult FBInterestSelected;

        ViewModels.PostcodeSearchListViewModel m_viewmodel;
        public PostcodeSearchListView(List<Models.FBCitiesResult> fBCities=null)
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new PostcodeSearchListViewModel(fBCities);
            this.CloseWhenBackgroundIsClicked = true;
         
        }

        protected override void OnAppearing()
        {
            try
            {
                IsBusy = false;
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
                _ = m_viewmodel.IsListForPostcodeMethod(m_viewmodel.ListForLoc);
            });
        }

        private void SearchLoc_Completed(object sender, TextChangedEventArgs e)
        {
            Task.Run(async () =>
            {
                await m_viewmodel.IsListForPostcodeMethod(m_viewmodel.ListForLoc);
            });
        }
    }
}

