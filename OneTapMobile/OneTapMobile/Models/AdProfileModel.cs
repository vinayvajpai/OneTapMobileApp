using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OneTapMobile.Models
{
	public class AdProfileModel
	{
		public string Industry { get; set; }
		public string AdProfile { get; set; }
		public string HeadLine1 { get; set; }
		public string HeadLine2 { get; set; }
		public string HeadLine3 { get; set; }
		public string Description1 { get; set; }
		public string Description2 { get; set; }
		public string Keywords { get; set; }
		public ImageSource  AdImageVideo { get; set; }
		
	}
    public class AdProfileResponseResult
    {
        public int id { get; set; }
        public string industry { get; set; }
        public string ad_profile { get; set; }
        public string primary_text { get; set; }
        public string button_title { get; set; }
        public string campaign_name { get; set; }
        public string headline { get; set; }
        public string fbPageName { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }
        public object updated_at { get; set; }
        public ImageSource AdImageVideo { get; set; }
    }

    public class AdProfileResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<AdProfileResponseResult> result { get; set; }
    }

    public class AdProfileRequestModel
    {
        public string user_id { get; set; }
    }


}

