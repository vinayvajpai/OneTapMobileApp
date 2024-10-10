using LibVLCSharp.Shared;
using MediaManager;
using Newtonsoft.Json.Linq;
using OneTapMobile.Global;
using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartCampaignView : ContentPage
    {
        StartCampaignViewModel m_viewmodel;
        bool istap = false;
        LibVLC _libVLC;
        MediaPlayer _mediaPlayer;

        public StartCampaignView()
        {
            try
            {
                BindingContext = m_viewmodel = new StartCampaignViewModel();
                InitializeComponent();
                if (m_viewmodel != null)
                {
                    m_viewmodel.nav = this.Navigation;
                }
                videoView.VideoURL = "https://sec.ch9.ms/ch9/e68c/690eebb1-797a-40ef-a841-c63dded4e68c/Cognitive-Services-Emotion_high.mp4";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }

        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (m_viewmodel != null)
                {
                    //m_viewmodel.IsTapped = true;
                    m_viewmodel.IsEnable = true;
                    IsBusy = false;
                    m_viewmodel.IsTap = false;
                }
                if(videoView != null)
                    videoView.StopOnDisappear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        protected override async void OnDisappearing()
        {
            try
            {
                base.OnDisappearing();
                if (videoView != null)
                    videoView.StopOnDisappear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            
        }


        private void Maximize_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FullScreenVideoView());
        }

        private void Nextbutton(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CampaignObjectiveView());
        }
    }
}