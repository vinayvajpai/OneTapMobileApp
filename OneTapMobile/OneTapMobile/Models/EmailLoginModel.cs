using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
namespace OneTapMobile.Models
{
    public class EmailLoginRequestModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public object email_verified_at { get; set; }
        public object otp_token { get; set; }
        public object otp_expire_time { get; set; }
        public string is_otp_required { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string token { get; set; }
    }
    public class EmailLoginResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Result result { get; set; }
    }
    public class FacebookProfile : BaseViewModel
    {
        public string user_id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string fb_access_token { get; set; }
        public UriImageSource Picture { get; set; }
    }
    public class ProviderData
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Id { get; set; }
        public string Picture { get; set; }
        public string Token { get; set; }
    }
    public class ProviderRoot
    {
        public ProviderData provider_data { get; set; }
    }

    public class AuthNetwork
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Background { get; set; }
        public string Foreground { get; set; }
    }
    public class NetworkAuthData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Picture { get; set; }
        public string Background { get; set; }
        public string Foreground { get; set; }
        public string Email { get; set; }
    }
    public class GoogleProviderData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
    }
    public class GoogleProviderRoot
    {
        public GoogleProviderData provider_data { get; set; }
    }

    public class AppleProviderData
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RealUserStatus { get; set; }
        public string Token { get; set; }
    }

    public class AppleProviderRoot
    {
        public AppleProviderData provider_data { get; set; }
    }



}
