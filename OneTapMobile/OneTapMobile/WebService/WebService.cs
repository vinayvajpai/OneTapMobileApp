using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneTapMobile.Global;
using OneTapMobile.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace OneTapMobile.WebService
{
    public class WebService
    {
        public static string encoding = "application/json";
        //Post Data 
        public static async Task<Rest_ResponseModel> PostData(object body, string methodUrl, bool Istoken)
        {

            if (!Helper.CheckValidToken())
            {
                return null;
            }
            Rest_ResponseModel resp = new Rest_ResponseModel();
            string url = string.Empty;
            string serialized_body = "";
            string response_text = string.Empty;
            try
            {
                JObject json = null;
                HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(60) };
                HttpResponseMessage response = null;
                url = Constant.BaseUrl + methodUrl;
                StringContent content = null;
                if (body != null)
                {
                    serialized_body = JsonConvert.SerializeObject(body, Formatting.None, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    Debug.WriteLine(serialized_body);
                    content = new StringContent(serialized_body, Encoding.UTF8, encoding);
                }

                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(encoding));
                if (Istoken)
                {
                    var authHeader = new AuthenticationHeaderValue("bearer", Constant.Token);
                    httpClient.DefaultRequestHeaders.Authorization = authHeader;
                }
                //httpClient.DefaultRequestHeaders.Add("Bearer", App.Toekn);
                response = await httpClient.PostAsync(url, content);
                response_text = await response.Content.ReadAsStringAsync();
                //httpClient.Dispose();
                json = JObject.Parse(await response.Content.ReadAsStringAsync());
                #region Build-Response-Object
                if (!string.IsNullOrEmpty(response_text))
                {
                    resp.content_length = response_text.Length;
                }
                else
                {
                    resp.content_length = 0;
                }
                resp.content_type = encoding;
                resp.response_body = response_text;
                resp.status_code = (int)response.StatusCode;
                #endregion
                #region Enumerate-Response
                bool rest_enumerate = true;
                if (rest_enumerate)
                {
                    Debug.WriteLine("rest_client response status_code " + resp.status_code + ": " + resp.content_length + "B for " + "POST" + " " + url);
                    Debug.WriteLine(resp.response_body);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return resp;
        }
        public static async Task<Rest_ResponseModel> PostData(object body, string methodUrl)
        {
            if (!Helper.CheckValidToken())
            {
                return null;
            }
            Rest_ResponseModel resp = new Rest_ResponseModel();
            string url = string.Empty;
            string serialized_body = "";
            string response_text = string.Empty;
            try
            {
                JObject json = null;
                HttpClientHandler clientHandler = new HttpClientHandler();
                HttpClient httpClient = new HttpClient(clientHandler) { Timeout = TimeSpan.FromSeconds(60) };
                HttpResponseMessage response = null;
                url = Constant.BaseUrl + methodUrl;
                StringContent content = null;
                if (body != null)
                {
                    serialized_body = JsonConvert.SerializeObject(body);
                    Debug.WriteLine("rest_client request : " + serialized_body);
                    content = new StringContent(serialized_body, Encoding.UTF8, encoding);
                }
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(encoding));
                //var authHeader = new AuthenticationHeaderValue("bearer", Constant.Token);
                //httpClient.DefaultRequestHeaders.Authorization = authHeader;
                response = await httpClient.PostAsync(url, content);
                response_text = await response.Content.ReadAsStringAsync();
                httpClient.Dispose();
                json = JObject.Parse(await response.Content.ReadAsStringAsync());
                #region Build-Response-Object
                if (!string.IsNullOrEmpty(response_text))
                {
                    resp.content_length = response_text.Length;
                }
                else
                {
                    resp.content_length = 0;
                }
                resp.content_type = encoding;
                resp.response_body = response_text;
                resp.status_code = (int)response.StatusCode;
                #endregion
                #region Enumerate-Response
                bool rest_enumerate = true;
                if (rest_enumerate)
                {
                    Debug.WriteLine("rest_client response status_code " + resp.status_code + ": " + resp.content_length + "B for " + "POST" + " " + url);
                    Debug.WriteLine(resp.response_body);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return resp;
        }
        public static async Task<Rest_ResponseModel> GetData(string methodUrl, bool Istoken)
        {
            if (!Helper.CheckValidToken())
            {
                return null;
            }
            Rest_ResponseModel resp = new Rest_ResponseModel();
            string url = string.Empty;
            string response_text = string.Empty;
            try
            {
                HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(60) };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Istoken)
                {
                    var authHeader = new AuthenticationHeaderValue("bearer", Constant.Token);
                    httpClient.DefaultRequestHeaders.Authorization = authHeader;
                }
                HttpResponseMessage response = null;
                url = Constant.BaseUrl + methodUrl;
                response = await httpClient.GetAsync(url);
                response_text = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("GETURL:-" + methodUrl);
                #region Build-Response-Object
                if (!string.IsNullOrEmpty(response_text))
                {
                    resp.content_length = response_text.Length;
                }
                else
                {
                    resp.content_length = 0;
                }
                resp.content_type = encoding;
                resp.response_body = response_text;
                resp.status_code = (int)response.StatusCode;
                #endregion
                #region Enumerate-Response
                bool rest_enumerate = true;
                if (rest_enumerate)
                {
                    Debug.WriteLine("rest_client response status_code " + resp.status_code + ": " + resp.content_length + "B for " + "GET" + " " + url);
                    Debug.WriteLine(resp.response_body);
                }
                #endregion
                // httpClient.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return resp;
        }


        //Post Data
        public static async Task<Rest_ResponseModel> AIPostData(object body, string methodUrl, bool Istoken, bool IsBaseurl)
        {
            Rest_ResponseModel resp = new Rest_ResponseModel();
            string url = string.Empty;
            string response_text = string.Empty;
            try
            {
                HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(60) };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Istoken)
                {
                    var authHeader = new AuthenticationHeaderValue("Bearer", Constant.AIkey);
                    httpClient.DefaultRequestHeaders.Authorization = authHeader;
                }
                HttpResponseMessage response = null;
                
                if (IsBaseurl)
                    url = Constant.BaseUrl + methodUrl;
                else
                    url = methodUrl;
               
                response = await httpClient.PostAsync(url, GetStringContent(body));
                response_text = await response.Content.ReadAsStringAsync();

                #region Build-Response-Object
                if (!string.IsNullOrEmpty(response_text))
                {
                    resp.content_length = response_text.Length;
                }
                else
                {
                    resp.content_length = 0;
                }
                resp.content_type = encoding;
                resp.response_body = response_text;
                resp.status_code = (int)response.StatusCode;
                #endregion
                #region Enumerate-Response
                bool rest_enumerate = true;
                if (rest_enumerate)
                {
                    Debug.WriteLine("rest_client response status_code " + resp.status_code + ": " + resp.content_length + "B for " + "GET" + " " + url);
                    Debug.WriteLine(resp.response_body);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return resp;
        }

        public static StringContent GetStringContent(object postModel)
        {
            var stringcontent = new StringContent(JsonConvert.SerializeObject(postModel), Encoding.UTF8, "application/json");
            return stringcontent;
        }
    }
}
