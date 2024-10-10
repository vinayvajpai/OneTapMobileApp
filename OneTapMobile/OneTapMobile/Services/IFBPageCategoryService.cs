using System;
using System.Threading.Tasks;
using OneTapMobile.Models;
using Xamarin.Forms;

namespace OneTapMobile.Services
{
    public interface IFBPageCategoryService
    {
        Task FBPageCategoryList(INavigation nav, FacebookPagesModel FbPageAccessmodel, Action<CatResponseModel> response);
        Task FBPageUpdateCategoryList(INavigation nav, SubmitCatRequestModel submitCatRequestModel, Action<bool> response);
    }
}

