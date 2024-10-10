using OneTapMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Global
{
    public class Constant
    {
        //local server development environment
        //public static string BaseUrl = "http://dev.onetap.alcyone.in/api/";

        //local server Staging environment
        // public static string BaseUrl = "http://app.onetap.alcyone.in/api/";

        //Client server Staging environment
        public static string BaseUrl = "https://staging.onetapsocial.co/api/";

        //Google Auth Url
        public static string GoogleAuthUrl = "https://staging.onetapsocial.co/google-oauth";

        public static bool IsConnected = false;
        public static string DeviceOS = string.Empty;
        public static string DeviceId = string.Empty;
        public static EmailLoginRequestModel emailLoginRequestModel;
        public static string Token;
     //  //public static string FacebookAppId = "1855604677967714";
      // //public static string FacebookAppId ="241563474859376";
      public static string FacebookAppId ="578177173616840";
       //// public static string FacebookAppId = "3309828659289335";
       // public static string FacebookAppId = "449497990571532";
       // public static string FacebookAppId = "478284210547526";
        public static int OneTapUserId;
        public static int Role;
        public static bool IsLoggedOut = true;
        public static readonly string PopupTitle = "Message";
        public static readonly string PopupMessage = "Something wrong please try again!";
        public static bool FbAdPageadded = false;
        public static bool FbRightArrowVisible = true;
        public static bool SkipNowVisisble = false;
        public static bool GoogleAdAdded = false;
        public static string DeviceToken = null;
        public static string AIkey = "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";
        public static string OrganizationId = "org-swJaUhntq1bTCU0gdU94A4uC";
        public static string openaiApi = "https://api.openai.com/v1/completions";
        //public static  string GoogleAdsRSAformattext = "Create a Google Ads in an RSA format (using multiple headlines and descriptions) for";
        //public static string GoogleAdsRSAformattext = "Create a Google Ads with 3 headlines containing characters not more then 30 each and 2 descriptions containing characters 90 each on for";
        //public static string GoogleAdsRSAformattext = "Make a Google Ads with three headlines that are no more than 30 characters each, and two descriptions that are no more than 90 characters each for";
        public static string GoogleAdsRSAformattext = "Create a Google Ads in an RSA format (using 3 headlines limit 30 characters only and short 2 descriptions limit 90 characters only) for";
       // public static string FaceBookAdsRSAformat = "Create a Facebook Ads in an RSA format (using headline and primary text limit 30 characters only and short description limit 90 characters only) for";
        public static string FaceBookAdsRSAformat = "Create a Facebook Ads in an RSA format (using 1 headline limit 40 characters only and 1 primary text limit 125 characters only) for";
    }
}
