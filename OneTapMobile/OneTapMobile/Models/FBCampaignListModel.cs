using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{
    public class FBCampaignListModel
    {

    }

    public class FBCampListRequestModel
    {
        public string user_id { get; set; }
        public string fb_access_token { get; set; }
    }


    public class Adaccounts
    {
        public List<Datum> data { get; set; }
        public Paging paging { get; set; }
    }

    public class Campaigns
    {
        public List<Datum> data { get; set; }
        public Paging paging { get; set; }
    }

    public class Cursors
    {
        public string before { get; set; }
        public string after { get; set; }
    }

    public class Datum
    {
        public Campaigns campaigns { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string budget_remaining { get; set; }
        public DateTime created_time { get; set; }
        public string status { get; set; }
    }

    public class Paging
    {
        public Cursors cursors { get; set; }
    }

    public class Response
    {
        public string id { get; set; }
        public string name { get; set; }
        public Adaccounts adaccounts { get; set; }
    }

    //public class FBCampListResponseModel
    //{
    //    public bool status { get; set; }
    //    public string message { get; set; }
    //    public Response result { get; set; }
    //}


    //new campaign model

    //public class Campaign
    //{
    //    public string id { get; set; }
    //    public string name { get; set; }
    //    public object campaign_budget { get; set; }
    //    public int budget_spend { get; set; }
    //    public string budget_percentage { get; set; }
    //    public string campaign_type { get; set; }
    //    public string CampaignProgress
    //    {
    //        get
    //        {
    //         return Convert.ToString( Convert.ToDouble( budget_percentage) / 100);
    //        }
    //    }
    //}

    //public class Result1
    //{
    //    public List<Campaign> campaigns { get; set; }
    //    public string today_optimization_score_percentage { get; set; }
    //    public int overall_optimization_score_percentage { get; set; }
    //    public int total_budget { get; set; }
    //    public int amount_spent { get; set; }
    //    public string currency { get; set; }
    //}

    //public class FBCampListResponseModel
    //{
    //    public bool status { get; set; }
    //    public string message { get; set; }
    //    public Result1 result { get; set; }
    //}


    // facebook Account campaign model

    //public class Campaign
    //{
    //    public string id { get; set; }
    //    public string name { get; set; }
    //    public string campaign_type { get; set; }
    //    public string campaign_Icon { get; set; }
    //    public int campaign_budget { get; set; }
    //    public double budget_spend { get; set; }
    //    public double budget_percentage { get; set; }
    //    public string CampaignProgress
    //    {
    //        get
    //        {
    //            return Convert.ToString(Convert.ToDouble(budget_percentage) / 100);
    //        }
    //    }
    //}

    //public class FbResult
    //{
    //    public List<Campaign> campaigns { get; set; }
    //    public int today_optimization_score_percentage { get; set; }
    //    public int overall_optimization_score_percentage { get; set; }
    //    public int total_budget { get; set; }
    //    public double amount_spent { get; set; }
    //    public string currency { get; set; }
    //}

    //public class FBCampListResponseModel
    //{
    //    public bool status { get; set; }
    //    public string message { get; set; }
    //    public FbResult result { get; set; }
    //}


    //// google account campaign model

    //public class GoCampListRequestModel
    //{
    //    public string user_id { get; set; }
    //    public string google_ad_customer_id { get; set; }
    //}



    ////public class GoogleCampaign
    ////{
    ////    public long id { get; set; }
    ////    public string name { get; set; }
    ////    public string campaign_type { get; set; }
    ////    public int campaign_budget { get; set; }
    ////    public double budget_spend { get; set; }
    ////    public double budget_percentage { get; set; }
    ////}

    //public class GResult
    //{
    //    public List<Campaign> campaigns { get; set; }
    //    public int today_optimization_score_percentage { get; set; }
    //    public int overall_optimization_score_percentage { get; set; }
    //    public int total_budget { get; set; }
    //    public double amount_spent { get; set; }
    //    public string currency { get; set; }
    //}

    //public class GoCampListResponseModel
    //{
    //    public bool status { get; set; }
    //    public string message { get; set; }
    //    public GResult result { get; set; }
    //}


    // Common Campaign List API

    public class CampListRequestModel
    {
        public bool is_google_ad { get; set; }
        public bool is_facebook_ad { get; set; }
        public string user_id { get; set; }

        //public string google_refresh_token { get; set; }
        public string google_ad_customer_id { get; set; }
        public string google_manager_id { get; set; } = "0";
        public string fb_access_token { get; set; }
        public string fb_ad_account_id { get; set; }
        public string google_refresh_token { get; set; }
    }

    public class Campaign
    {
        public string type { get; set; }
        public object id { get; set; }
        public string name { get; set; }
        public string campaign_type { get; set; }
        public string created_time { get; set; }
        public string campaign_Icon { get; set; }
        public bool instagram_publish { get; set; }
        public double campaign_budget { get; set; }
        public string status { get; set; }
        public double budget_spend { get; set; }
       // public double budget_percentage { get; set; }
        public string CurrencySymbol { get; set; }
        public int campaign_optimization_score_percentage { get; set; }
        public string CampaignProgress
        {
            get
            {
                if(campaign_optimization_score_percentage != 0)
                return Convert.ToString(Convert.ToDouble(campaign_optimization_score_percentage) / 100);
                else
                    return Convert.ToString(campaign_optimization_score_percentage);
            }
        }

        public string statuscolor
        {
            get
            {
                if (status.ToUpper() == "PAUSED")
                    return "Red";
                else
                    return "Green";
            }
        }


    }

    public class CampResult
    {
        public List<Campaign> campaigns { get; set; }
        public double today_optimization_score_percentage { get; set; }
        public double overall_optimization_score_percentage { get; set; }
        public double total_budget { get; set; }
        public double amount_spent { get; set; }
        public double facebook_spent { get; set; }
        public bool instagram_publish { get; set; }
        public double google_spent { get; set; }
        public string fb_ad_account_id { get; set; }
        public string google_ad_customer_id { get; set; }
        public string currency { get; set; }
    }

    public class CampListResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public CampResult result { get; set; }
    }


}
