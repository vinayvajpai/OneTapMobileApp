using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{
    //public class ChangePasswordModel
    //{       


    //}
    public class ChangePasswordRequestModel
    {
        public int user_id { get; set; }
        public string old_password { get; set; }
        public string new_password { get; set; }
        public string confirm_password { get; set; }
    }

    public class ChangePasswordResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
}
