using OneTapMobile.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordView : ContentPage
    {
        ChangePasswordViewModel m_viewmodel;
        public ChangePasswordView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new ChangePasswordViewModel();
        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                IsBusy = false;
                m_viewmodel.IsSuccess = false; 
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