using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
namespace OneTapMobile.Models
{
    public class ImageCampaign
    {
        public string Objective { get; set; }
        public string facebook_page_id { get; set; }
        public string facebook_page_name { get; set; }
        public ImageSource Image { get; set; }
        public ImageSource CroppedImage { get; set; }
        public string CampName { get; set; }
        public string PrimaryText { get; set; }
        public string Headline { get; set; }
        public string WebURL { get; set; }
        public string ButtonTitle { get; set; }
        public string InstaId { get; set; }
        public List<FBCitiesResult> TargetLocation { get; set; }
        public List<FBInterestResult> DemoInterest { get; set; }
        public List<PostCodesResult> postcodes { get; set; }
        public int StartAgeRange { get; set; }
        public int EndAgeRange { get; set; }
        public string Gender { get; set; }
        public string Budget { get; set; }  
        public string BudgetType { get; set; }
        public string PaymentMethod { get; set; }
        public string StartCampDate { get; set; }
        public string EndCampDate { get; set; }
    }
    public class VideoCampaign
    {
        public string Objective { get; set; }
        public string facebook_page_id { get; set; }
        public string facebook_page_name { get; set; }
        public FileResult Video { get; set; }
        public ImageSource SelectedThumb { get; set; }
        public string CampName { get; set; }
        public string PrimaryText { get; set; }
        public string Headline { get; set; }
        public string WebURL { get; set; }
        public string ButtonTitle { get; set; }
        public string InstaId { get; set; }
        public List<FBCitiesResult> TargetLocation { get; set; }
        public List<FBInterestResult> DemoInterest { get; set; }
        public List<PostCodesResult> postcodes { get; set; }
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


    }
    public class KeywordCampaign
    {
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
        public List<string> KeywordTheme { get; set; }
        public List<GoAdsTarLocResult> TargetLocation { get; set; }
        public string Budget { get; set; }
        public string BudgetType { get; set; }
        public string PaymentMethod { get; set; }
        public string StartCampDate { get; set; }
        public string EndCampDate { get; set; }

    }

    public class Interest
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class DemoInterestForCreateCamp
{
        public List<Interest> interests { get; set; }
    }

}
