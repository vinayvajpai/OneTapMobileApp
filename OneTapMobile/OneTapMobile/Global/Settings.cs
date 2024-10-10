using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace OneTapMobile.Global
{
    public class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }


        public static string facebook_page_id
        {
            get
            {
                return AppSettings.GetValueOrDefault("SFacebook_Page_Id" , "0");
            }
            set
            {
                AppSettings.AddOrUpdateValue("SFacebook_Page_Id", value);
            }
        }

        public static string GoRefreshToken
        {
            get
            {
                return AppSettings.GetValueOrDefault("SGoRefreshToken", null);
            }
            set
            {
                AppSettings.AddOrUpdateValue("SGoRefreshToken", value);
            }
        }

        public static string UserLoginDataJson
        {
            get
            {
                return AppSettings.GetValueOrDefault("UserLoginDataJson", null);
            }
            set
            {
                AppSettings.AddOrUpdateValue("UserLoginDataJson", value);
            }
        }

    }
}
