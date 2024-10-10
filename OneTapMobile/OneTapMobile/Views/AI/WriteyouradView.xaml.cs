using OneTapMobile.Global;
using OneTapMobile.ViewModels;
using OneTapMobile.ViewModels.AI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views.AI
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WriteyouradView : ContentPage
    {
        WriteyouradViewModel m_viewmodel;

        public WriteyouradView (Models.GoAdResponse model)
		{
			InitializeComponent ();
            BindingContext = m_viewmodel = new WriteyouradViewModel(model);
            
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
            Headline4Txt.Focus();
        }

        private void Heading4_Completed(object sender, EventArgs e)
        {
            CampaignName.Focus();

        }
    }
}