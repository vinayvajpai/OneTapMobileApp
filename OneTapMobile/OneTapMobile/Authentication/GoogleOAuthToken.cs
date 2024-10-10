namespace OneTapMobile.Authentication
{
    public class GoogleOAuthToken
    {
        public string TokenType { get; set; }
        public string AccessToken { get; set; }       
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string id_token { get; set; }
    }
}
