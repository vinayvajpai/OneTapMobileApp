namespace OneTapMobile.Models
{
    public class Rest_ResponseModel
    {
        public string content_type { get; set; }
        public long content_length { get; set; }
        public int status_code { get; set; }
        public string response_body { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class SubResponseModel<T>
    {
        public string success { get; set; }
        public string message { get; set; }
        public T result { get; set; }
    }
}
