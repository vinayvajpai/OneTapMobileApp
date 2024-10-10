﻿using System.Threading.Tasks;

namespace OneTapMobile.Interface
{
    public interface IAppleSignInService
    {
        bool IsAvailable { get; }
        Task<AppleSignInCredentialState> GetCredentialStateAsync(string userId);
        Task<AppleAccount> SignInAsync();
    }
    public class AppleAccount
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string RealUserStatus { get; set; }
        public string UserId { get; set; }
    }
    public enum AppleSignInCredentialState
    {
        Authorized,
        Revoked,
        NotFound,
        Unknown
    }
    public interface INotchService
    {
        bool HasNotch();
    }
    public interface IClearCookies
    {
        void ClearAllCookies();
    }
}
