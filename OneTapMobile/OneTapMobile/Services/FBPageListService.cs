using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace OneTapMobile.Services
{
    public class FBPageListService : IFBPageListService
    {
        INavigation _nav;

        #region Constructor
        public FBPageListService()
        {
        }
        #endregion

        #region FBPageList Service
        public async Task FBPageList(INavigation nav, FacebookPagesModel FbPageAccessmodel, Action<FBPageResponseModel> response)
        {
            _nav = nav;
            string url = "user/facebook-pages";
            UserDialogPopup popupnav;
            Rest_ResponseModel rest_result = await WebService.WebService.PostData(FbPageAccessmodel, url, true);
            if (rest_result != null)
            {
                if (rest_result.status_code == 200)
                {
                   var PageResultModel = JsonConvert.DeserializeObject<FBPageResponseModel>(rest_result.response_body);
                    if (PageResultModel != null)
                    {
                        response.Invoke(PageResultModel);
                    }
                    else
                    {
                        response.Invoke(null);
                    }
                }
                else
                {
                    popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage,"Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    response.Invoke(null);
                }
            }
                else
                {
                    popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    response.Invoke(null);
                }
        }
        #endregion
    }
}

