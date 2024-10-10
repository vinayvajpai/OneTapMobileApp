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
    public partial class AddKeywordCampWebSiteView : ContentPage
    {
        AddKeywordCampWebSiteViewModel m_viewmodel;
        public AddKeywordCampWebSiteView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new AddKeywordCampWebSiteViewModel();
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

        private void WebURLTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WebURLTxt.Text != null)
            {
                if (WebURLTxt.Text.Length > 3)
                {
                    if (e.NewTextValue.Length > e.OldTextValue.Length)
                    {
                        if (!WebURLTxt.Text.ToLower().StartsWith("http"))
                        {
                                WebURLTxt.Text = WebURLTxt.Text.Insert(0, "https://");
                        }
                    }
                }
            }
        }
    }
}