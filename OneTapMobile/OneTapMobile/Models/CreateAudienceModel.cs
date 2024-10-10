using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace OneTapMobile.Models
{

    #region Facebook Cities API for Audience data
    public class FBCitiesRequestModel
    {
        public string fb_access_token { get; set; }
        public string search_text { get; set; }
    }

    public class PostCodesResult : PropertyChangedModel
    {
        public string key { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region { get; set; }
        public int region_id { get; set; }
        public string primary_city { get; set; }
        public int primary_city_id { get; set; }
        public bool supports_region { get; set; }
        public bool supports_city { get; set; }

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
        public string ModifiedName => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(region) ? name + ", " + region + ", " : name + region;
    }

    public class PostCodesRequest
    {
        public string key { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region { get; set; }
        public int region_id { get; set; }
        public string primary_city { get; set; }
        public int primary_city_id { get; set; }
        public bool supports_region { get; set; }
        public bool supports_city { get; set; }
    }

    public class FBCitiesResult : PropertyChangedModel
    {
        public string key { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region { get; set; }
        public int region_id { get; set; }
        public bool supports_region { get; set; }
        public bool supports_city { get; set; }
        public string geo_hierarchy_level { get; set; }
        public string geo_hierarchy_name { get; set; }

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
        public string ModifiedName => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(region) ? name + ", " + region + ", " : name + region;
 
    }
    public class FBCitiesResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<FBCitiesResult> result { get; set; }

    }

    public class FBPostCodeResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<PostCodesResult> result { get; set; }

    }

    #endregion

    #region Fb Interest API for audience data
    public class FBInterestRequestMdoel
    {
        public string fb_access_token { get; set; }
        public string search_text { get; set; }
    }

    public class FBInterestResult : BaseViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public object type { get; set; }
        public object description { get; set; }
        public object source { get; set; }
        public object partner { get; set; }
        public int audience_size { get; set; }
        public object country { get; set; }
        public object country_access { get; set; }
        public object topic { get; set; }

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

        public string ModifiedName => name;
    }

    public class FBInterestResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<FBInterestResult> result { get; set; }
    }
    #endregion

}
