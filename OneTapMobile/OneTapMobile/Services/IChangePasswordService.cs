using OneTapMobile.Models;
using System;
using System.Threading.Tasks;

namespace OneTapMobile.Services
{
    public interface IChangePasswordService
    {
        Task ChangePassword(ChangePasswordRequestModel changePasswordRequestModel, Action<bool> response);
    }
}
