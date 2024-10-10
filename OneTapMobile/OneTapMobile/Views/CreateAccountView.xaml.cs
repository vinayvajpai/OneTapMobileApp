using OneTapMobile.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAccountView : ContentPage
    {
        private CreateAccountViewModel m_viewmodel;
        public CreateAccountView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new CreateAccountViewModel();
        }
        protected override void OnAppearing()
        {
            try
            {
                IsBusy = false;
                base.OnAppearing();
                if (m_viewmodel != null)
                {
                    m_viewmodel.nav = this.Navigation;
                    m_viewmodel.IsEnable = true;
                    m_viewmodel.IsTap = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}