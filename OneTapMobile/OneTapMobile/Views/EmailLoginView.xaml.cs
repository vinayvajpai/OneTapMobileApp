using OneTapMobile.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailLoginView : ContentPage
    {
        private EmailLoginViewModel m_viewmodel;
        public EmailLoginView()
        {
            BindingContext = m_viewmodel = new EmailLoginViewModel();
            InitializeComponent();
            if (m_viewmodel != null)
            {
                m_viewmodel.nav = this.Navigation;
            }
        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                //m_viewmodel.IsTapped = true;
                m_viewmodel.IsEnable = true;
                IsBusy = false;
                m_viewmodel.IsTap = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}