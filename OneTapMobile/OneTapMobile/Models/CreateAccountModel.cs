using System;
using System.Collections.Generic;
using System.Text;
namespace OneTapMobile.Models
{
    public class CreateAccountRequestModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string password_confirm { get; set; }
    }
    public class CreateAccountResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Result data { get; set; }
    }
}
