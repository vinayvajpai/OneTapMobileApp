using System;
using System.Threading.Tasks;
using OneTapMobile.Models;
using Xamarin.Forms;

namespace OneTapMobile.Services
{
    public interface IForgotPasswordService
    {
       Task ForgotPassword(INavigation nav,CreateAccountRequestModel createRequestModel, Action<bool> response);
    }
}

