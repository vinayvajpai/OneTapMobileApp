using OneTapMobile.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FBPageListView : ContentPage
    {
        FBPageListViewModel m_viewmodel;
        public FBPageListView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new FBPageListViewModel();
        }
        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (m_viewmodel != null)
                {
                    m_viewmodel.nav = this.Navigation;
                    m_viewmodel.IsTap = false;
                    await m_viewmodel.GetPageData();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new LoginView());
        }
    }
}