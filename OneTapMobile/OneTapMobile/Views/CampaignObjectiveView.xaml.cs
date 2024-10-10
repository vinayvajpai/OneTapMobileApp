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
    public partial class CampaignObjectiveView : ContentPage
    {
        CampaignObjectiveViewModel m_viewmodel;
        public CampaignObjectiveView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new CampaignObjectiveViewModel();
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
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}