using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{   
    //public class AdResult
    //{
    //    public string id { get; set; }
    //    public string name { get; set; }
    //}

    //public class GoogleAdResponseModel
    //{
    //    public bool status { get; set; }
    //    public string message { get; set; }
    //    public List<AdResult> result { get; set; }
    //}

    public class GoogleAdRequestModel
    {
        public string user_id { get; set; }
        public string google_refresh_token { get; set; }
    }

    public class ChildAccount:BaseViewModel
    {
        public string customer_id { get; set; }
        public string manager_id { get; set; }
        public string descriptive_name { get; set; }
        public string currency_code { get; set; }
        public string time_zone { get; set; }
        public List<object> child_accounts { get; set; }
        public bool is_manager { get; set; }
        private string _Tick = "RightTickGray";
        public string Tick
        {
            get
            {
                return _Tick;
            }
            set
            {
                _Tick = value;
                OnPropertyChanged("Tick");
            }
        }

    }

    public class GoCustomerListResult: BaseViewModel
    {
        public string customer_id { get; set; }
        public string manager_id { get; set; }
        public string descriptive_name { get; set; }
        public string currency_code { get; set; }
        public string time_zone { get; set; }
        public List<ChildAccount> child_accounts { get; set; }
        public bool is_manager { get; set; }

        private string _Tick = "RightTickGray";
        public string Tick
        {
            get
            {
                return _Tick;
            }
            set
            {
                _Tick = value;
                OnPropertyChanged("Tick");
            }
        }

        public string ModifiedName => string.IsNullOrWhiteSpace(descriptive_name) ? "Google Ads Account" : descriptive_name;
        public string ManagerAccount => is_manager ? "Manager" :"";
       

        private string _ExpandArrow = "RightArrow";
        public string ExpandArrow
        {
            get
            {
                return _ExpandArrow;
            }
            set
            {
                _ExpandArrow = value;
                OnPropertyChanged("ExpandArrow");
            }
        }

        private bool _IsSubAcc = false;
        public bool IsSubAcc
        {
            get
            {
                return _IsSubAcc;
            }
            set
            {
                _IsSubAcc = value;
                OnPropertyChanged("IsSubAcc");
            }
        }

        public bool HideCheck
        {
            get
            {
                return child_accounts == null ? false : (child_accounts.Count == 0 ? true : false);
            }
        }


    }

    public class GoCustomerListResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<GoCustomerListResult> result { get; set; }
    }

    public class CurrencyModel
    {
        public string cc { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
    }

    public class TimeZoneModel
    {
        public string value { get; set; }
        public string abbr { get; set; }
        public int offset { get; set; }
        public bool isdst { get; set; }
        public string text { get; set; }
        public List<string> utc { get; set; }
    }

}
