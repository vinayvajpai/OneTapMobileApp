using System;
using System.Threading.Tasks;
using OneTapMobile.Interface;
using OneTapMobile.Models;
using Plugin.FacebookClient;
namespace OneTapMobile.Services
{
    public interface ILoginService
    {
        Task FaceBookLogin(Action<FacebookResponse<bool>> response);
        Task GoogleLogin(Action<NetworkAuthData> response);
        Task AppleLogin(Action<AppleAccount> response);
    }
}
