using Newtonsoft.Json;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OneTapMobile.Global
{
    public class Helper : BaseViewModel
    {
        public static ProviderData facebookProfile;
        public static GoogleProviderData GoogleProfile;
        public static GoCustomerListResult SelectedGoAdCustDetail;
        public static UserProfileModel profileModel = new UserProfileModel();
        public static byte[] CampimageByte;
        public static ImageSource UserImage;
        public static string UserImagePath;
        public static Image CroppedImage;
        public static TimeSpan videoseentime;
        public static string ThumbnailAddress;
        public static byte[] BitmapData;
        public static string GoRefreshToken;
        public static string PageName = "NoGoogleAdAccView";
        public static bool IsFacebookAdLogin = false;
        public static string UserEmail;

        // CreateCampType shows which type of campaign is going to be create.
        // It can be ("image", "Video", "Keywords")
        //ByDefault setting as video but it will change during camapaign creation.
        public static string CreateCampType = "Video";
        public static string thumbfolder = "";
        //   public static string facebook_page_id = "0";
        public static ImageCampaign imageCampaign = new ImageCampaign();
        public static VideoCampaign videoCampaign = new VideoCampaign();
        public static KeywordCampaign keywordCampaign = new KeywordCampaign();
        public static List<GoCustomerListResult> GocustomerAccList = new List<GoCustomerListResult>();
        public static List<FBAccIdListResult> FBcustomerAccList = new List<FBAccIdListResult>();
        public static List<GoAdsTarLocResult> GoAdLocationList = new List<GoAdsTarLocResult>();
        public static string NoOfMessages;

        //public static double TotalSpentAmount;
        //public static double GSpentAmount;
        //public static double FbSpentAmount;
        //public static string FbAdAccountId;
        //public static string GoAdCustomerId;

        public static bool ValidateEmail(string email)
        {
            try
            {
                return Regex.IsMatch(email.ToLower(), @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                 @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            }
            catch
            {
                return false;
            }
        }

        //public static bool ValidateURL(string URL)
        //{
        //    try 
        //    {  
        //        string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
        //        Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //        return Rgx.IsMatch(URL);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public static void ResetLoginData()
        {
            Constant.IsLoggedOut = true;
            Constant.FbAdPageadded = false;
            Constant.GoogleAdAdded = false;
            Constant.FbRightArrowVisible = true;
            Constant.SkipNowVisisble = false;
            Helper.facebookProfile = new ProviderData();
            Helper.SelectedGoAdCustDetail = new GoCustomerListResult();
            Helper.GoogleProfile = new GoogleProviderData();
            Helper.CampimageByte = null;
            Helper.UserImage = null;
            Helper.CroppedImage = null;
            Helper.videoseentime = TimeSpan.Zero;
            Helper.imageCampaign = new ImageCampaign();
            Helper.videoCampaign = new VideoCampaign();
            Helper.keywordCampaign = new KeywordCampaign();
            Helper.profileModel = new UserProfileModel();
            Helper.GocustomerAccList = new List<GoCustomerListResult>();
            Helper.FBcustomerAccList = new List<FBAccIdListResult>();
            Helper.UserEmail = null;
            Helper.GoRefreshToken = null;
            Helper.IsFacebookAdLogin = false;
            PageName = "NoGoogleAdAccView";
            Application.Current.Properties.Clear();
            Application.Current.SavePropertiesAsync();

        }

        public static void Goback(INavigation Navigation)
        {
            Navigation.PopAsync();
        }

        public static List<CurrencyModel> GetCurrencyList()
        {
            string jsonFileName = "Currency.json";
            List<CurrencyModel> ObjCurrenctyList = new List<CurrencyModel>();
            try
            {
                var assembly = typeof(Helper).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
                using (var reader = new System.IO.StreamReader(stream))
                {
                    var jsonString = reader.ReadToEnd();

                    //Converting JSON Array Objects into generic list    
                    ObjCurrenctyList = JsonConvert.DeserializeObject<List<CurrencyModel>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return ObjCurrenctyList;
        }

        internal static double GetExtraYAxisValue(double maxvalue)
        {
            return maxvalue / 5;
        }

        public static List<string> GetTimeZoneList()
        {
            string jsonFileName = "TimeZone.json";
            List<string> ObjTimeZoneList = new List<string>();
            try
            {
                var assembly = typeof(Helper).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
                using (var reader = new System.IO.StreamReader(stream))
                {
                    var jsonString = reader.ReadToEnd();

                    //Converting JSON Array Objects into generic list    
                    ObjTimeZoneList = JsonConvert.DeserializeObject<List<string>>(jsonString);
                }
            }
            catch (Exception ex)
            {

            }
            return ObjTimeZoneList;
        }

        public static string ReadHtmlFileContent(string FileName)
        {
            string jsonFileName = FileName;
            string GetPageCode = null;
            try
            {
                var assembly = typeof(Helper).GetTypeInfo().Assembly;
                var FPath = assembly.GetName().Name+".Resources";
                Stream stream = assembly.GetManifestResourceStream($"{FPath}.{jsonFileName}");

                using (var reader = new System.IO.StreamReader(stream))
                {
                    GetPageCode = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return GetPageCode;
        }

        public static class FormUpload
        {
            private static readonly Encoding encoding = Encoding.UTF8;
            public static HttpWebResponse MultipartFormPost(string postUrl, string userAgent, Dictionary<string, object> postParameters, string headerkey, string headervalue)
            {
                string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
                string contentType = "multipart/form-data; boundary=" + formDataBoundary;

                byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

                return PostForm(postUrl, userAgent, contentType, formData, headerkey, headervalue);
            }
            private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData, string headerkey, string headervalue)
            {
                HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }

                // Set up the request properties.
                request.Method = "POST";
                request.ContentType = contentType;
                request.UserAgent = userAgent;
                request.CookieContainer = new CookieContainer();
                request.ContentLength = formData.Length;

                // You could add authentication here as well if needed:
                // request.PreAuthenticate = true;
                // request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;

                //Add header if needed
                request.Headers.Add(headerkey, headervalue);

                // Send the form data to the request.
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                }

                return request.GetResponse() as HttpWebResponse;
            }

            private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
            {
                Stream formDataStream = new System.IO.MemoryStream();
                bool needsCLRF = false;

                foreach (var param in postParameters)
                {

                    if (needsCLRF)
                        formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                    needsCLRF = true;

                    if (param.Value is FileParameter) // to check if parameter if of file type
                    {
                        FileParameter fileToUpload = (FileParameter)param.Value;

                        // Add just the first part of this param, since we will write the file data directly to the Stream
                        string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                            boundary,
                            param.Key,
                            fileToUpload.FileName ?? param.Key,
                            fileToUpload.ContentType ?? "application/octet-stream");

                        formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                        // Write the file data directly to the Stream, rather than serializing it to a string.
                        formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                    }
                    else
                    {
                        string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                            boundary,
                            param.Key,
                            param.Value);
                        formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                    }
                }

                // Add the end of the request.  Start with a newline
                string footer = "\r\n--" + boundary + "--\r\n";
                formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

                // Dump the Stream into a byte[]
                formDataStream.Position = 0;
                byte[] formData = new byte[formDataStream.Length];
                formDataStream.Read(formData, 0, formData.Length);
                formDataStream.Close();

                return formData;
            }

            public class FileParameter
            {
                public byte[] File { get; set; }
                public string FileName { get; set; }
                public string ContentType { get; set; }
                public FileParameter(byte[] file) : this(file, null) { }
                public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
                public FileParameter(byte[] file, string filename, string contenttype)
                {
                    File = file;
                    FileName = filename;
                    ContentType = contenttype;
                }
            }

        }

        public static void SavePropertyData(string key,object value)
        {
            if (Application.Current.Properties.ContainsKey(key)) {
                Application.Current.Properties.Remove(key);
            }
            Application.Current.Properties[key] = value;
            Application.Current.SavePropertiesAsync();
        }
        public static void SetLoginUserData(string userJsonData)
        {
            Application.Current.Properties["LoginUserData"] = userJsonData;
            Application.Current.Properties["TokenExpireTime"] = DateTime.UtcNow;
            Application.Current.SavePropertiesAsync();
        }
        public static GlobalLoginDataModel GetLoginUserData()
        {
            if (Application.Current.Properties.ContainsKey("LoginUserData"))
            {
                var jsonUserData = Application.Current.Properties["LoginUserData"].ToString();
                return JsonConvert.DeserializeObject<GlobalLoginDataModel>(jsonUserData);
            }
            return null;
        }
        public static bool CheckValidToken()
        {
            try
            {

                if (Application.Current.Properties.ContainsKey("TokenExpireTime"))
                {
                    var TokenExpireTime = (DateTime)Application.Current.Properties["TokenExpireTime"];
                    if (TokenExpireTime.Year < DateTime.UtcNow.Year)
                    {
                        var page = App.Current.MainPage.Navigation.NavigationStack.LastOrDefault() as Page;
                        if (page != null)
                        {
                            var baseVM = page.BindingContext as BaseViewModel;
                            baseVM.IsBusy = false;
                        }
                        if (PopupNavigation.Instance.PopupStack.Any())
                            PopupNavigation.Instance.PopAllAsync();

                        var popupnav = new UserDialogPopup("Token Expired", "Your login token has been expired, please login again!", true);
                        popupnav.eventOK += Popupnav_eventOK;
                        PopupNavigation.Instance.PushAsync(popupnav);
                        return false;
                    }
                }

            }
            catch (Exception)
            {
            }
            return true;
        }
        private static void Popupnav_eventOK(object sender, EventArgs e)
        {
            Helper.ResetLoginData();
            App.Current.MainPage = new NavigationPage(new LoginView());
        }

        async public static Task<bool> CheckSession()
        {
            if (Application.Current.Properties.ContainsKey("IsLoggedInGoogleAds"))
            {
                var dt = Application.Current.Properties["IsLoggedInGoogleAds"].ToString();
                var logginDate = Convert.ToDateTime(dt);
                var days = (DateTime.Now - logginDate).Days;
                if(days >= 180)
                {
                    
                    var popupnav = new UserDialogPopup("Session Expired", "Your OneTap session has expired. Please login again.",true);
                    popupnav.eventOK += Popupnav_eventOK1;
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    return true;
                }
            }
            return false;
        }

        private static void Popupnav_eventOK1(object sender, EventArgs e)
        {
            Helper.ResetLoginData();
            App.Current.MainPage = new NavigationPage(new LoginView());
        }

        public static async Task PopToPage<T>(INavigation navigation)
        {
            //First, we get the navigation stack as a list
            var pages = navigation.NavigationStack.ToList();

            //Then we invert it because it's from first to last and we need in the inverse order
            pages.Reverse();

            //Then we discard the current page
            pages.RemoveAt(0);

            var toRemove = new List<Page>();

            var tipoPagina = typeof(T);

            foreach (var page in pages)
            {
                if (page.GetType() == tipoPagina)
                    break;

                toRemove.Add(page);
            }

            foreach (var rvPage in toRemove)
            {
                navigation.RemovePage(rvPage);
            }

            await navigation.PopAsync();
        }
    }

   public static class Extention
    {

        public async static Task<string> GetFileChachePath(this FileResult fileResult)
        {
            var newFile = Path.Combine(FileSystem.CacheDirectory, fileResult.FileName);
            using (var stream = await fileResult.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            return newFile;
        }
    }

    public static class StringExtension
    {
        public static string CapitalizeFirst(this string s)
        {

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string titleCase = textInfo.ToTitleCase(s.ToLower());
            return titleCase;

            //bool IsNewSentense = true;
            //var result = new StringBuilder(s.Length);
            //for (int i = 0; i < s.Length; i++)
            //{
            //    if (IsNewSentense && char.IsLetter(s[i]))
            //    {
            //        result.Append(char.ToUpper(s[i]));
            //        IsNewSentense = false;
            //    }
            //    else
            //        result.Append(s[i].ToString().ToLower());

            //    if (s[i] == '!' || s[i] == '?' || s[i] == '.')
            //    {
            //        IsNewSentense = true;
            //    }
            //}

            //return result.ToString();
        }
    }
}
