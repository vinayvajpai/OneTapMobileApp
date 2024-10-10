using System;
using System.Threading.Tasks;
using OneTapMobile.Models;
namespace OneTapMobile.Services
{
    public interface IEmailLoginService
    {
        Task EmailLogin(EmailLoginRequestModel emailLoginRequestModel, Action<bool> response);
    }
}
