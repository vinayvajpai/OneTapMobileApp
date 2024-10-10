using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OneTapMobile.Services
{
    public class GoogleService
    {
        public async Task<string> GetEmailAsync(string tokenType, string accessToken)
        {
            try
            {

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var json = await httpClient.GetStringAsync("https://www.googleapis.com/userinfo/email?alt=json");
                if (json != null)
                {
                    var email = JsonConvert.DeserializeObject<GoogleEmail>(json);
                    return email.Data.Email;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return string.Empty;
        }
    }
}
