using System;
using System.Threading.Tasks;
using OneTapMobile.Models;
using Xamarin.Forms;

namespace OneTapMobile.Services
{
    public interface ICreateAccountService
    {
        Task CreateAccount(INavigation nav, CreateAccountRequestModel createRequestModel, Action<bool> response);
    }
}

