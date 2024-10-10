using Acr.UserDialogs;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using OneTapMobile.Views.ErrorPage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadImageView : ContentPage
    {
        UploadImageViewModel m_viewmodel;
        public UploadImageView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new UploadImageViewModel();
        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (m_viewmodel != null)
                {
                    m_viewmodel.nav = this.Navigation;
                    m_viewmodel.IsTap = false;
                }

                MessagingCenter.Subscribe<object, bool>(this, "ChangeIsTapToFalse", (sender, arg) =>
                {
                    if (!arg)
                    {
                        m_viewmodel.IsTap = arg;
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}