using MediaManager;
using MediaManager.Playback;
using OneTapMobile.Global;
using OneTapMobile.ViewModels;
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
    public partial class AdReviewView : ContentPage
    {
        AdReviewViewModel m_viewmodel;
        public AdReviewView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new AdReviewViewModel();
        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                IsBusy = false;
                if (m_viewmodel != null)
                {
                    videoView.StopOnDisappear();
                    _ = m_viewmodel.GetAdReviewData();
                    m_viewmodel.IsTap = false;
                    m_viewmodel.nav = this.Navigation;

                    if (Helper.CreateCampType == "video")
                    {
                        m_viewmodel.UploadedImage = Helper.videoCampaign.SelectedThumb;
                        videoView.VideoURL = Helper.videoCampaign.Video.FullPath;

                    }
                    if (Helper.CreateCampType== "image")
                        m_viewmodel.UploadedImage = Helper.imageCampaign.Image;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnDisappearing()
        {
            try
            {
                base.OnDisappearing();
                videoView.StopOnDisappear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

    }

}