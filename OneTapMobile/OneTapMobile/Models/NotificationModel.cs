using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{
    #region model for user notification sent from API
    public class NotificationRequestModel
    {
        public string user_id { get; set; }
        public string device_token { get; set; }

    }
    public class NotificationResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }

    #endregion

    #region Models for Notification list
    public class NotificationListResResult
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string notification_title { get; set; }
        public string notification_body { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string IsShowReadicon => status == 0 ? "Unread" : ""; 
    }

    public class NotificationListResModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<NotificationListResResult> result { get; set; }
    }
    public class NotificationListReqModel
    {
        public string user_id { get; set; }

    }
    #endregion

    public class NotificationReadUpdateReqmodel
    {
        public string user_id { get; set; }
        public int notification_id { get; set; }
        public int status { get; set; }
    }
    public class NotificationReadUpdateResmodel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }

}

