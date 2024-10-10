using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{
    public class UserProfileModel
    {
        public double amount_spent { get; set; }
        public double facebook_spent { get; set; }
        public double google_spent { get; set; }
        public string fb_ad_account_id { get; set; }
        public string currency { get; set; }
        public string google_ad_customer_id { get; set; }
        public string google_ad_manager_id { get; set; }
        public string ProfileImage { get; set;}
        public string UserName { get; set;}
        public string Location   { get; set; }
    }

    public class CreateAddAccModelReq
    {
        public string user_id { get; set; }
        public string google_refresh_token { get; set; }
        public string manager_id { get; set; }
        public string name { get; set; }
        public string currency { get; set; }
        public string time_zone { get; set; }
    }

    public class CreateAddAccResponseResult
    {
        public string customer_id { get; set; }
        public string manager_id { get; set; }
    }

    public class CreateAddAccResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public CreateAddAccResponseResult result { get; set; }
    }
}
