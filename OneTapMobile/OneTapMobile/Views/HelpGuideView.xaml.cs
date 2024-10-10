using Microsoft;
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
    public partial class HelpGuideView : ContentPage
    {
        HelpGuideViewModel m_viewmodel;
        public HelpGuideView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new HelpGuideViewModel();
            //string HTMLPageContent = Helper.ReadHtmlFileContent("HelpGuide.HTML");
            var browser = new WebView();
            //var htmlSource = new HtmlWebViewSource();
            //htmlSource.Html = HTMLPageContent;
            WebPageView.Source = "https://onetap-knowledge-base.groovehq.com/help";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                m_viewmodel.nav = this.Navigation;
                m_viewmodel.IsTap = false;
                activity_indicator.IsVisible = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            
        }
        public void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            activity_indicator.IsVisible = false;
        }
    }
}