using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace OneTapMobile.Services
{
    public class FBPageCategoryService : IFBPageCategoryService
    {
        INavigation _nav;
        FBPageCategoryViewModel vm;

        #region Constructor
        public FBPageCategoryService()
        {

        }
        #endregion

        #region FBPageCategoryList Service
        public async Task FBPageCategoryList(INavigation nav, FacebookPagesModel FbPageAccessmodel, Action<CatResponseModel> response)
        {
            _nav = nav;
            UserDialogPopup popupnav;
            string url = "user/facebook-categories";
            Rest_ResponseModel rest_result = await WebService.WebService.PostData(FbPageAccessmodel, url, true);
            if (rest_result != null)
            {
                if (rest_result.status_code == 200)
                {
                    var CatResult = JsonConvert.DeserializeObject<CatResponseModel>(rest_result.response_body);

                    if (CatResult != null)
                    {
                        response.Invoke(CatResult);
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
                    vm.IsshowEmpty = true;
                    vm.IsChecking = false;
                    vm.IsBusy = false;
                    response.Invoke(null);
                }
            }
            else
            {
                popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                await PopupNavigation.Instance.PushAsync(popupnav);
                vm.IsshowEmpty = true;
                vm.IsChecking = false;
                vm.IsBusy = false;
                response.Invoke(null);
            }
        }
        #endregion

        #region FBPageUpdateCategoryList Service
        public async Task FBPageUpdateCategoryList(INavigation nav, SubmitCatRequestModel submitCatRequestModel, Action<bool> response)
        {
            _nav = nav;
            UserDialogPopup popupnav;
            string url = "user/update-facebook-categories";
            Rest_ResponseModel rest_result = await WebService.WebService.PostData(submitCatRequestModel, url, true);
            if (rest_result != null)
            {
                if (rest_result.status_code == 200)
                {
                    var CatSubmitResModel = JsonConvert.DeserializeObject<CatSubmitResponseModel>(rest_result.response_body);

                    if (CatSubmitResModel != null)
                    {
                        if (CatSubmitResModel.status)
                        {
                            response.Invoke(true);
                        }
                        else
                        {
                            response.Invoke(false);
                        }
                    }
                    else
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, CatSubmitResModel.message,"Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        vm.IsBusy = false;
                        response.Invoke(false);
                    }
                }
                else
                {
                    popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage,"Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    vm.IsBusy = false;
                    response.Invoke(false);
                }
            }
            else
            {
                popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                await PopupNavigation.Instance.PushAsync(popupnav);
                vm.IsBusy = false;
                response.Invoke(false);
            }
        }
        #endregion
    }
}

