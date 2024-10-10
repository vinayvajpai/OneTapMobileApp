using MediaManager;
using MediaManager.Library;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Models;
using OneTapMobile.ViewModels;
using OneTapMobile.Views.ErrorPage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadVideoView : ContentPage
    {
        public UploadVideoViewModel m_viewmodel;
        public UploadVideoView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new UploadVideoViewModel();
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