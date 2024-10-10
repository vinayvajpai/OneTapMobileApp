using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{
    public class GoAdTempResponseResult
    {
        public int id { get; set; }
        public string industry { get; set; }
        public string ad_profile { get; set; }
        public string headline_1 { get; set; }
        public string headline_2 { get; set; }
        public string headline_3 { get; set; }
        public string description_1 { get; set; }
        public string description_2 { get; set; }
        public string keywords { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }
        public object updated_at { get; set; }
    }

    public class GoAdTempResponsemodel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<GoAdTempResponseResult> result { get; set; }
    }
    public class GoAdTempRequestModel
    {
        public string user_id { get; set; }
    }



}
