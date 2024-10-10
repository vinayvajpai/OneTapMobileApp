using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullScreenVideoView : ContentPage
    {
        public FullScreenVideoView()
        {
            InitializeComponent();
            // CrossMediaManager.Current.Play("https://sec.ch9.ms/ch9/e68c/690eebb1-797a-40ef-a841-c63dded4e68c/Cognitive-Services-Emotion_high.mp4");
            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body>  <iframe style='position: absolute; top: 0; left: 0; width: 100%; height: 100%;'  src='https://sec.ch9.ms/ch9/e68c/690eebb1-797a-40ef-a841-c63dded4e68c/Cognitive-Services-Emotion_high.mp4' frameborder='0' poster='OneTapLogo.png' allowfullscreen =''></iframe></body></html>";
            browser.Source = htmlSource;
            videoPlayer.Source = htmlSource;
        }
        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, "allowLandScapePortrait");
            videoPlayer.WidthRequest = this.Width;
            videoPlayer.HeightRequest = this.Height;
            base.OnAppearing();
        }



        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, "preventLandScape");
            base.OnDisappearing();
        }

        private void MiniMize_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}