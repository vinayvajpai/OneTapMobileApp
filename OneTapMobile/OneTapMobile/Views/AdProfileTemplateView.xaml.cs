using System;
using System.Collections.Generic;
using System.Diagnostics;
using OneTapMobile.Global;
using OneTapMobile.ViewModels;
using Xamarin.Forms;

namespace OneTapMobile.Views
{	
	public partial class AdProfileTemplateView : ContentPage
	{
        AdProfileTemplateViewModel m_viewmodel;
        public AdProfileTemplateView (ImageSource adImage)
		{
			InitializeComponent ();
			BindingContext = m_viewmodel = new AdProfileTemplateViewModel(adImage);
        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                IsBusy = false;
                if (m_viewmodel != null)
                {
                    m_viewmodel.IsTap = false;
                    m_viewmodel.nav = this.Navigation;
                    if (Helper.CreateCampType == "image")
                    {
                        m_viewmodel.FBPageName = Helper.imageCampaign.facebook_page_name;
                    }
                    if (Helper.CreateCampType == "video")
                    {
                        m_viewmodel.FBPageName = Helper.videoCampaign.facebook_page_name;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}

