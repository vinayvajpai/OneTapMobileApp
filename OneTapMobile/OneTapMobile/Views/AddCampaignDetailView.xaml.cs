using OneTapMobile.CustomControl;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.ViewModels;
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
    public partial class AddCampaignDetailView : ContentPage
    {
        AddCampaignDetailViewModel m_viewmodel;
        public AddCampaignDetailView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new AddCampaignDetailViewModel();
            if(Helper.imageCampaign != null)
            {
                if (Helper.CreateCampType.ToLower() == "image")
                {
                    ButtonTitle.SelectedIndex = 0;
                    m_viewmodel.PrimaryTxt = Helper.imageCampaign.PrimaryText;
                    m_viewmodel.CampNameTxt = Helper.imageCampaign.CampName;
                    m_viewmodel.Headline = Helper.imageCampaign.Headline;
                    m_viewmodel.WebURL = Helper.imageCampaign.WebURL;
                }
            }
            if(Helper.videoCampaign != null)
            {
                if (Helper.CreateCampType.ToLower() == "video")
                {
                    ButtonTitle.SelectedIndex = 0;
                    m_viewmodel.PrimaryTxt = Helper.videoCampaign.PrimaryText;
                    m_viewmodel.CampNameTxt = Helper.videoCampaign.CampName;
                    m_viewmodel.Headline = Helper.videoCampaign.Headline;
                    m_viewmodel.WebURL = Helper.videoCampaign.WebURL;
                }
            }
        }

        public AddCampaignDetailView(FBAdResponse fbmodel)
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new AddCampaignDetailViewModel();
            if (Helper.imageCampaign != null)
            {
                if (Helper.CreateCampType.ToLower() == "image")
                {
                    ButtonTitle.SelectedIndex = 0;
                    m_viewmodel.PrimaryTxt = fbmodel.primarytext;
                    m_viewmodel.Headline = fbmodel.headline;
                   // m_viewmodel.CampNameTxt = Helper.imageCampaign.CampName;
                   // m_viewmodel.WebURL = Helper.imageCampaign.WebURL;
                }
            }
            if (Helper.videoCampaign != null)
            {
                if (Helper.CreateCampType.ToLower() == "video")
                {
                    ButtonTitle.SelectedIndex = 0;
                    m_viewmodel.PrimaryTxt = fbmodel.primarytext;
                    m_viewmodel.Headline = fbmodel.headline;
                   // m_viewmodel.CampNameTxt = Helper.videoCampaign.CampName;
                   // m_viewmodel.WebURL = Helper.videoCampaign.WebURL;
                }
            }
        }

        public AddCampaignDetailView(FBAdResponse fbmodel, List<string> listInputs)
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new AddCampaignDetailViewModel(listInputs) ;
            if (Helper.imageCampaign != null)
            {
                if (Helper.CreateCampType.ToLower() == "image")
                {
                    ButtonTitle.SelectedIndex = 0;
                    m_viewmodel.PrimaryTxt = fbmodel.primarytext;
                    m_viewmodel.Headline = fbmodel.headline;
                    // m_viewmodel.CampNameTxt = Helper.imageCampaign.CampName;
                    // m_viewmodel.WebURL = Helper.imageCampaign.WebURL;
                }
            }
            if (Helper.videoCampaign != null)
            {
                if (Helper.CreateCampType.ToLower() == "video")
                {
                    ButtonTitle.SelectedIndex = 0;
                    m_viewmodel.PrimaryTxt = fbmodel.primarytext;
                    m_viewmodel.Headline = fbmodel.headline;
                    // m_viewmodel.CampNameTxt = Helper.videoCampaign.CampName;
                    // m_viewmodel.WebURL = Helper.videoCampaign.WebURL;
                }
            }
        }


        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                IsBusy = false;
                if (m_viewmodel != null)
                {
                    ButtonTitle.SelectedIndex = 0;
                    ButtonTitle.ItemSelected += OnDropdownSelected;
                    m_viewmodel.nav = this.Navigation;
                    m_viewmodel.IsTap = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void OnDropdownSelected(object sender, ItemSelectedEventArgs e)
        {
            ButtonTitle.Text = m_viewmodel.ButtonTitle[e.SelectedIndex];
        }

        private void DropdownSelected(object sender, EventArgs e)
        {
            SeButtonTitle.Focus();
        }

        private void WebURL_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WebURL.Text != null)
            {
                if (WebURL.Text.Length > 3)
                {
                    if (e.OldTextValue != null && e.NewTextValue != null)
                    {
                        if (e.NewTextValue.Length > e.OldTextValue.Length)
                        {
                            if (!WebURL.Text.ToLower().StartsWith("http"))
                            {
                                WebURL.Text = WebURL.Text.Insert(0, "https://");
                            }
                        }
                    }
                }
            }
        }
    }
}