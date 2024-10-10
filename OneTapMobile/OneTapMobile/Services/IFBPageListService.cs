using System;
using System.Threading.Tasks;
using OneTapMobile.Models;
using Xamarin.Forms;

namespace OneTapMobile.Services
{
    public interface IFBPageListService
    {
        Task FBPageList(INavigation nav, FacebookPagesModel FbPageAccessmodel, Action<FBPageResponseModel> response);
    }
}

