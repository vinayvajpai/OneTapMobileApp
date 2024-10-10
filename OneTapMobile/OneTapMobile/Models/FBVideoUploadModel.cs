using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OneTapMobile.Models
{
    public class FBVideoUploadRequest
    {
        public string ads_video { get; set; }
        public ImageSource video_thumb { get; set; }
        public string facebook_ad_account_id { get; set; }
        public string fb_access_token { get; set; }
    }

    public class FBVideoUploadResult
    {
        public string video_id { get; set; }
        public string video_url { get; set; }
        public string thumb_url { get; set; }


       
}

    public class FBVideoUploadResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public FBVideoUploadResult result { get; set; }
    }
}
