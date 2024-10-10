using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.CustomControl
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomVideoPlayerView : ContentView
    {
        public LibVLC _libVLC;
        public CustomVideoPlayerView()
        {
            InitializeComponent();
            Device.BeginInvokeOnMainThread(() =>
            {
                PlayPauseBtnContainer.IsVisible = true;
                playPauseButton.IsVisible = true;
                VideoLoader.IsVisible = false;
                VideoLoader.IsRunning = false;
            });

            LibVLCSharp.Shared.Core.Initialize();
           _libVLC = new LibVLC();
            VideoView.MediaPlayer = new MediaPlayer(_libVLC);
            VideoView.MediaPlayer.Opening += MediaPlayer_Opening;
            VideoView.MediaPlayer.Buffering += MediaPlayer_Buffering;
            VideoView.MediaPlayer.Playing += MediaPlayer_Playing;
            VideoView.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
        }



        public static readonly BindableProperty VideoURLProperty = BindableProperty.Create(nameof(VideoURL), typeof(string), typeof(CustomVideoPlayerView), string.Empty, propertyChanged: VideoURLPropertyChanged);

        //Gets or sets IsCurvedCornersEnabled value  
        public string VideoURL
        {
            get => (string)GetValue(VideoURLProperty);
            set => SetValue(VideoURLProperty, value);
        }

        public static readonly BindableProperty IsPause1Property = BindableProperty.Create(nameof(IsPause1), typeof(bool), typeof(CustomVideoPlayerView), false, propertyChanged: IsPauseLPropertyChanged);

        //Gets or sets IsCurvedCornersEnabled value  
        public bool IsPause1
        {
            get => (bool)GetValue(VideoURLProperty);
            set => SetValue(VideoURLProperty, value);
        }

        private static void VideoURLPropertyChanged(BindableObject bindable, object oldVal, object newVal)
        {
            var tagview = bindable as CustomVideoPlayerView;
            LibVLCSharp.Shared.Core.Initialize();
            /*var _libVLC = new LibVLC()*/;
            tagview.VideoView.MediaPlayer.Media = new Media(tagview._libVLC, new Uri(Convert.ToString(newVal)));
            //tagview.VideoView.MediaPlayer = new MediaPlayer(_libVLC)
            //{
            //    Media = new Media(_libVLC, new Uri(Convert.ToString(newVal)))
            //};
            tagview.VideoView.MediaPlayer.Play();
            tagview.PlayPauseBtnContainer.IsVisible = true;
            tagview.playPauseButton.IsVisible = true;
        }

        private void MediaPlayer_Opening(object sender, EventArgs e)
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    PlayPauseBtnContainer.IsVisible = true;
            //});
        }

        private void MediaPlayer_Buffering(object sender, MediaPlayerBufferingEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (!PlayPauseBtnContainer.IsVisible)
                {
                    VideoLoader.IsVisible = true;
                    VideoLoader.IsRunning = true;
                }
                else
                {
                    VideoLoader.IsVisible = false;
                    VideoLoader.IsRunning = false;
                }
            });
        }

        private void MediaPlayer_Playing(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                VideoLoader.IsVisible = false;
                VideoLoader.IsRunning = false;
            });
        }

        private void MediaPlayer_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                VideoLoader.IsVisible = false;
                VideoLoader.IsRunning = false;
            });
        }


        private static void IsPauseLPropertyChanged(BindableObject bindable, object oldVal, object newVal)
        {
            try
            {

                var tagview = bindable as CustomVideoPlayerView;
                var result = Convert.ToBoolean(newVal);
                if (tagview.VideoView.MediaPlayer != null)
                {
                    if (result)
                    {
                        tagview.VideoView.MediaPlayer.Stop();
                        tagview.playPauseButton.Text = "Play";
                    }
                    else
                    {
                        tagview.VideoView.MediaPlayer.Play();
                        tagview.playPauseButton.Text = "Pause";
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        //private void mediaPlayer_MediaOpened(object sender, EventArgs e)
        //{
        //    //loader.IsVisible = true;
        //}

        //private void mediaPlayer_MediaEnded(object sender, EventArgs e)
        //{
        //    //loader.IsVisible = false;
        //}

        //private void mediaPlayer_MediaFailed(object sender, EventArgs e)
        //{

        //    //loader.IsVisible = false;
        //}


        private void Button_Clicked(object sender, EventArgs e)
        {
            if (playPauseButton.IsVisible)
            {
                if (playPauseButton.Text == "Play")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        playPauseButton.Text = "Pause";
                        PlayPauseBtnContainer.IsVisible = false;
                        if (VideoView.MediaPlayer != null)
                            VideoView.MediaPlayer.Play();
                    });

                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        playPauseButton.Text = "Play";
                        PlayPauseBtnContainer.IsVisible = true;
                        VideoLoader.IsVisible = false;
                        VideoLoader.IsRunning = false;

                        if (VideoView.MediaPlayer != null)
                            VideoView.MediaPlayer.Pause();
                    });
                }
            }
        }

        public void StopOnDisappear()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                playPauseButton.Text = "Play";
                //PlayPauseBtnContainer.IsVisible = false;
                if(VideoView.MediaPlayer!=null)
                    VideoView.MediaPlayer.Stop();
            });
        }


        private void ShowHidePlayButton(object sender, EventArgs e)
        {
            if (!PlayPauseBtnContainer.IsVisible)
            {
                PlayPauseBtnContainer.IsVisible = true;
                Device.BeginInvokeOnMainThread(() =>
                {
                    VideoLoader.IsVisible = false;
                    VideoLoader.IsRunning = false;
                });
            }
            else
                PlayPauseBtnContainer.IsVisible = false;
        }



    }
}