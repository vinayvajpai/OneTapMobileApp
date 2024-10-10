using OneTapMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OneTapMobile.LocalDataBases.DataBaseModel
{
    public class LoginUserDataDBModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string LoginUserData { get; set; }
    }
    
    public class ProfileDBModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string ProfileModelData { get; set; }
    }
    
    public class facebookProfileDBModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string facebookProfileData { get; set; }
    }

    public class GoRefreshTokenDBModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string GoRefreshTokenData { get; set; }
    }
    
    public class FBcustomerAccListDBModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string FBcustomerAccListData { get; set; }
    }

    public class GoogleAdsCustomersDBModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string GoogleAdsCustomersData { get; set; }
    }
    
    public class FacebookPageIdDBModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string FacebookPageIdData { get; set; }
    }
    
    public class SelectedGoAdCustDetailDBModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }

        // public string SelectedGoAdCustDetailData { get; set; }
        public string customer_id { get; set; }
        public string manager_id { get; set; }
        public string descriptive_name { get; set; }
        public string currency_code { get; set; }
        public string time_zone { get; set; }
     //   public List<ChildAccount> child_accounts { get; set; }
        public bool is_manager { get; set; }
        public string Tick {get; set;}
        public string ExpandArrow { get; set; }
        public bool IsSubAcc { get; set; }
    }

    public class TokenExpireTimeDBModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string TokenExpireTimeData { get; set; }
    }


    public class ImageCampaignModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string Objective { get; set; }
        public string facebook_page_id { get; set; }
        public string facebook_page_name { get; set; }
        public string Image { get; set; }
        public string CroppedImage { get; set; }
        public string CampName { get; set; }
        public string PrimaryText { get; set; }
        public string Headline { get; set; }
        public string WebURL { get; set; }
        public string ButtonTitle { get; set; }
        public string InstaId { get; set; }
        public string TargetLocation { get; set; }
        public string DemoInterest { get; set; }
        public int StartAgeRange { get; set; }
        public int EndAgeRange { get; set; }
        public string Gender { get; set; }
        public string Budget { get; set; }
        public string BudgetType { get; set; }
        public string PaymentMethod { get; set; }
        public string StartCampDate { get; set; }
        public string EndCampDate { get; set; }
        public string PostCodes { get; set; }
    }
    
    
    public class VideoCampaignModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string Objective { get; set; }
        public string facebook_page_id { get; set; }
        public string facebook_page_name { get; set; }
        public string Video { get; set; }
        public string SelectedThumb { get; set; }
        public string CampName { get; set; }
        public string PrimaryText { get; set; }
        public string Headline { get; set; }
        public string WebURL { get; set; }
        public string ButtonTitle { get; set; }
        public string InstaId { get; set; }
        public string TargetLocation { get; set; }
        public string DemoInterest { get; set; }
        public int StartAgeRange { get; set; }
        public int EndAgeRange { get; set; }
        public string Gender { get; set; }
        public string Budget { get; set; }
        public string BudgetType { get; set; }
        public string PaymentMethod { get; set; }
        public string StartCampDate { get; set; }
        public string EndCampDate { get; set; }
        public string video_id { get; set; }
        public string Thumbname { get; set; }
        public string video_url { get; set; }
        public string thumb_url { get; set; }
        public string PostCodes { get; set; }
    }

    public class KeywordCampaignModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string Objective { get; set; }
        public string CampName { get; set; }
        public string WebsiteURL { get; set; }
        public string Headline1 { get; set; }
        public string Headline2 { get; set; }
        public string Headline3 { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Industry { get; set; }
        public string Keywords { get; set; }
        public string KeywordTheme { get; set; }
        public string TargetLocation { get; set; }
        public string Budget { get; set; }
        public string BudgetType { get; set; }
        public string PaymentMethod { get; set; }
        public string StartCampDate { get; set; }
        public string EndCampDate { get; set; }

    }

}
