using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{
    public class SettingsModel
    {
        public GlobalLoginDataModel SGlobalLoginDataModel { get; set; }

        public SettingsModel()
        {
            SGlobalLoginDataModel = new GlobalLoginDataModel();
        }
    }

    public class ChangeSettingsRequestModel
    {
        public string user_id { get; set; }
        public int is_push_notification { get; set; }
        public int is_newsletter { get; set; }
    }

    public class ChangeSettingsResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }

    public class DeleteUserRequestModel
    {
        public string user_id { get; set; }
    }

    public class DeleteUserResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }

}
