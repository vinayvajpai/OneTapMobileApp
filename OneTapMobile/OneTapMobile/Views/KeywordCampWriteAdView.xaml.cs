using OneTapMobile.Global;
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
    public partial class KeywordCampWriteAdView : ContentPage
    {
        KeywordCampWriteAdViewModel m_viewmodel;
        public KeywordCampWriteAdView(Models.GoAdResponse model)
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new KeywordCampWriteAdViewModel();
            //if(Helper.keywordCampaign != null)
            //{
            //    m_viewmodel.Headline1Txt = Helper.keywordCampaign.Headline1;
            //    m_viewmodel.Headline2Txt = Helper.keywordCampaign.Headline2;
            //    m_viewmodel.Headline3Txt = Helper.keywordCampaign.Headline3;
            //    m_viewmodel.Desc1Txt = Helper.keywordCampaign.Description1;
            //    m_viewmodel.Desc2Txt = Helper.keywordCampaign.Description2;
            //}

            if (model != null)
            {
                m_viewmodel.Headline1Txt = model.headline_1;
                m_viewmodel.Headline2Txt = model.headline_2;
                m_viewmodel.Headline3Txt = model.headline_3;
                m_viewmodel.Desc1Txt = model.description_1;
                m_viewmodel.Desc2Txt = model.description_2;
            }
        }


        public KeywordCampWriteAdView(Models.GoAdResponse model,List<string> listInputs)
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new KeywordCampWriteAdViewModel(listInputs);
            
            //if(Helper.keywordCampaign != null)
            //{
            //    m_viewmodel.Headline1Txt = Helper.keywordCampaign.Headline1;
            //    m_viewmodel.Headline2Txt = Helper.keywordCampaign.Headline2;
            //    m_viewmodel.Headline3Txt = Helper.keywordCampaign.Headline3;
            //    m_viewmodel.Desc1Txt = Helper.keywordCampaign.Description1;
            //    m_viewmodel.Desc2Txt = Helper.keywordCampaign.Description2;
            //}

            if (model != null)
            {
                m_viewmodel.Headline1Txt = model.headline_1;
                m_viewmodel.Headline2Txt = model.headline_2;
                m_viewmodel.Headline3Txt = model.headline_3;
                m_viewmodel.Desc1Txt = model.description_1;
                m_viewmodel.Desc2Txt = model.description_2;
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
                    m_viewmodel.nav = this.Navigation;
                    m_viewmodel.IsTap = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void Heading1_Completed(object sender, EventArgs e)
        {
            Headline2Txt.Focus();
        }

        private void Heading2_Completed(object sender, EventArgs e)
        {
            Headline3Txt.Focus();
        }

        private void Heading3_Completed(object sender, EventArgs e)
        {
            Desc1Txt.Focus();
        }
    }
}