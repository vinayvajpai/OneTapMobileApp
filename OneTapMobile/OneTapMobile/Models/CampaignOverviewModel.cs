using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{
    public class OtherData
    {
        public int optimization_score_per { get; set; }
        public string total_spent { get; set; }
        public string today_spend { get; set; }
        public string status { get; set; }
        public string status_text { get; set; }
        public string created_time { get; set; }
        public string campaign_type { get; set; }
        public string thumbnail_url { get; set; }
        public string bg_image_url { get; set; }
    }

    public class QuestionDatum
    {
        public string question { get; set; }
        public string answer { get; set; }
        public string icon_type { get; set; }
    }

    public class ReportDatum
    {
        public string clicks { get; set; }
        public DateTime label { get; set; }
    }

    public class CampOverviewResult
    {
        public List<ReportDatum> report_data { get; set; }
        public List<QuestionDatum> question_data { get; set; }
        public OtherData other_data { get; set; }
    }

    public class OverviewResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public CampOverviewResult result { get; set; }
    }

    public class FbOverviewRequestModel
    {
        public string user_id { get; set; }
        public string ads_type { get; set; }
        public string fb_access_token { get; set; }
        public string campaign_id { get; set; }
        public string report_type { get; set; }
    }

    public class GoOverviewRequestModel
    {
        public string user_id { get; set; }
        public string ads_type { get; set; }
        public string google_refresh_token { get; set; }
        public string google_ad_customer_id { get; set; }
        public string google_ad_Manager_id { get; set; }
        public string campaign_id { get; set; }
        public string report_type { get; set; }
    }

    // campaign status update model 

    public class FBCamapStatusRequestModel
    {
        public string user_id { get; set; }
        public string ads_type { get; set; }
        public string fb_access_token { get; set; }
        public string campaign_id { get; set; }
        public int status { get; set; }
    }



    public class GoCamapStatusRequestModel
    {
        public string user_id { get; set; }
        public string ads_type { get; set; }
        public string google_refresh_token { get; set; }
        public string google_ad_customer_id { get; set; }
        public string google_ad_manager_id { get; set; }
        public string campaign_id { get; set; }
        public int status { get; set; }
    }


    public class CampStatusResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }

}
