using MediaManager;
using MediaManager.Library;
using OneTapMobile.Global;
using OneTapMobile.Services;
using OneTapMobile.ViewModels;
using Plugin.Media;
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
    public partial class SelectVideoThumbView : ContentPage
    {
        SelectVideoThumbViewModel m_viewmodel;
        FileResult Videopicked;
        public object MediaFileType { get; private set; }
        public SelectVideoThumbView(FileResult Video)
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new SelectVideoThumbViewModel();
            try
            {
                GetPath(Video);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public  void GetPath(FileResult Video)
        {
            Videopicked = Video;
            videoView.VideoURL = Videopicked.FullPath;//await Video.GetFileChachePath();
            m_viewmodel.VideoFile = Video;
            CreateThumb();
        }

        protected  override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (m_viewmodel != null)
                {
                    IsBusy = false;
                    m_viewmodel.IsTap = false;
                    Helper.videoCampaign.Video = m_viewmodel.VideoFile;
                    m_viewmodel.nav = this.Navigation;
                    videoView.StopOnDisappear();
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
                m_viewmodel.IsTap = false;
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.Message);
            }

        }

        public async void CreateThumb()
        {
            try
            {
                var URLFile = Videopicked.FullPath;//await Videopicked.GetFileChachePath();
                m_viewmodel.Thumbnail1 = Thumbnail1.Source = DependencyService.Get<ICreateThumbnailService>().CreateThumnails(URLFile, 1);
                m_viewmodel.Thumbnail2 = Thumbnail2.Source = DependencyService.Get<ICreateThumbnailService>().CreateThumnails(URLFile, 2);
                m_viewmodel.Thumbnail3 = Thumbnail3.Source = DependencyService.Get<ICreateThumbnailService>().CreateThumnails(URLFile, 3);

                if (m_viewmodel.Thumbnail1 != null)
                {
                    m_viewmodel.SelectDefaultThumb();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }




    }
}