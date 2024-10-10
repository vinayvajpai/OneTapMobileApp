using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{


    #region Create Campaign Request models
    public class GoBudgetData
    {
        public string budget_type { get; set; }
        public int bid_amount { get; set; }
        public int budget_amount { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
    }

    public class GoFields
    {
        public string objective { get; set; }
        public string campaign_type { get; set; }
        public string name { get; set; }
        public string primary_text { get; set; }
        public string button_title { get; set; }
    }

    public class GoKeywordsData
    {
        public string website_url { get; set; }
        public List<string> keywords { get; set; }
        public List<string> location_ids { get; set; }
    }

    public class GoResponsiveAdsData
    {
        public string headline_1 { get; set; }
        public string headline_2 { get; set; }
        public string headline_3 { get; set; }
        public string description_1 { get; set; }
        public string description_2 { get; set; }
    }

    public class CreateGoCampRequestModel
    {
        public string ads_type { get; set; }
        public string user_id { get; set; }
        public string google_customer_id { get; set; }
        public string google_manager_id { get; set; }
        public string google_refresh_token { get; set; }
        public GoFields fields { get; set; }
        public GoBudgetData budget_data { get; set; }
        public GoKeywordsData keywords_data { get; set; }
        public GoResponsiveAdsData responsive_ads_data { get; set; }
    }

    #endregion

    #region Facebook create video campaign request model 

    public class FBVideoAudienceData
    {
        public List<FBVideoLocation> locations { get; set; }
        public string gender { get; set; }
        public int age_min { get; set; }
        public int age_max { get; set; }
        public List<FBVideoInterest> interests { get; set; }
        public List<PostCodesResult> postcodes { get; set; }
    }

    public class FBVideoBudgetData
    {
        public string budget_type { get; set; }
        public int bid_amount { get; set; }
        public int budget_amount { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
    }

    public class FBVideoFields
    {
        public string objective { get; set; }
        public string campaign_type { get; set; }
        public string name { get; set; }
        public string primary_text { get; set; }
        public string headline { get; set; }
        public string button_title { get; set; }
        public string facebook_page_id { get; set; }
        public string video_id { get; set; }
        public string video_thumb_url { get; set; }
        public string website_url { get; set; }
    }

    public class FBVideoInterest
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class FBVideoLocation
    {
        public string key { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region { get; set; }
        public int region_id { get; set; }
    }

    public class FbVidCreateCampRequestModel
    {
        public string ads_type { get; set; }
        public string user_id { get; set; }
        public string fb_ad_account_id { get; set; }
        public string fb_access_token { get; set; }
        public FBVideoFields fields { get; set; }
        public FBVideoBudgetData budget_data { get; set; }
        public FBVideoAudienceData audience_data { get; set; }
    }

    #endregion

    #region Facebook Image Create campaign request model

    public class FBImageAudienceData
    {
        public List<FBImageLocation> locations { get; set; }
        public string gender { get; set; }
        public int age_min { get; set; }
        public int age_max { get; set; }
        public List<FBImageInterest> interests { get; set; }
        public List<PostCodesResult> postcodes { get; set; }
    }

    public class FBImageBudgetData
    {
        public string budget_type { get; set; }
        public int bid_amount { get; set; }
        public int budget_amount { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
    }

    public class FBImageFields
    {
        public string objective { get; set; }
        public string campaign_type { get; set; }
        public string name { get; set; }
        public string primary_text { get; set; }
        public string headline { get; set; }
        public string button_title { get; set; }
        public string facebook_page_id { get; set; }
        public string website_url { get; set; }
        public string image_base64_string { get; set; }
    }

    public class FBImageInterest
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class FBImageLocation
    {
        public string key { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region { get; set; }
        public int region_id { get; set; }
    }

    public class FBImgCreateCampRequestModel
    {
        public string ads_type { get; set; }
        public string user_id { get; set; }
        public string fb_ad_account_id { get; set; }
        public string fb_access_token { get; set; }
        public FBImageFields fields { get; set; }
        public FBImageBudgetData budget_data { get; set; }
        public FBImageAudienceData audience_data { get; set; }
    }

    #endregion

    #region Responses after creating campaign 
    public class CreateCampData
    {
        public string campaign_id { get; set; }
    }

    public class CreateCampResult
    {
        public bool status { get; set; }
        public string message { get; set; }
        public CreateCampData data { get; set; }
    }

    public class CreateCampResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public CreateCampResult result { get; set; }
    }
    #endregion
}
