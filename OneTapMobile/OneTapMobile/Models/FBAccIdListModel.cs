using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{
    public class FBAccIdListRequest
    {
        public string user_id { get; set; }
        public string fb_access_token { get; set; }
    }

    public class FBAccIdListResult : BaseViewModel
    {
        public string account_id { get; set; }
        public string name { get; set; }
        public int account_status { get; set; }
        public string amount_spent { get; set; }
        public string currency { get; set; }
        public string id { get; set; }

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

        public static implicit operator List<object>(FBAccIdListResult v)
        {
            throw new NotImplementedException();
        }
    }

    public class FBAccIdListResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<FBAccIdListResult> result { get; set; }
    }


}
