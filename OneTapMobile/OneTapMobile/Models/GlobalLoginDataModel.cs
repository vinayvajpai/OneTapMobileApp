using System;
using System.Collections.Generic;
using System.Text;
namespace OneTapMobile.Models
{
    public class GlobalAccessResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string image { get; set; }
        public int role { get; set; }
        public object email_verified_at { get; set; }
        public string google_id { get; set; }
        public string google_data { get; set; }
        public string facebook_id { get; set; }
        public string facebook_data { get; set; }
        public string apple_id { get; set; }
        public string apple_data { get; set; }
        public string google_refresh_token { get; set; }
        public string first_google_refresh_token { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string token { get; set; }
    }
    public class GlobalLoginDataModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public GlobalAccessResult result { get; set; }
    }
}
