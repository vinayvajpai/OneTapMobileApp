using System;
using System.Diagnostics;
using OneTapMobile.ViewModels;
using Xamarin.Forms;
namespace OneTapMobile.Views
{
    public partial class FBPageCategoryView : ContentPage
    {
        FBPageCategoryViewModel m_viewmodel;
        public FBPageCategoryView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new FBPageCategoryViewModel();
            m_viewmodel.nav = this.Navigation;
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
                    await m_viewmodel.GetCatData();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
