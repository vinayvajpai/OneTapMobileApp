using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace OneTapMobile.Services
{
    public class CreateAccountService : ICreateAccountService
    {
        INavigation _nav;

        #region Constructor
        public CreateAccountService()
        { 
        }
        #endregion

        #region CreateAccount Service
        public async Task CreateAccount(INavigation nav, CreateAccountRequestModel createRequestModel, Action<bool> response)
        {
            _nav = nav;
            UserDialogPopup popupnav;
            string url = "register";
            Rest_ResponseModel rest_result = await WebService.WebService.PostData(createRequestModel, url, false);
            if (rest_result != null)
            {
                if (rest_result.status_code == 200)
                {
                    CreateAccountResponseModel CreateAccountResponseModel = JsonConvert.DeserializeObject<CreateAccountResponseModel>(rest_result.response_body);
                    if (CreateAccountResponseModel != null)
                    {
                        if (CreateAccountResponseModel.status)
                        {
                            response.Invoke(true);
                        }
                        else
                        {
                            popupnav = new UserDialogPopup(Constant.PopupTitle, CreateAccountResponseModel.message, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            response.Invoke(false);
                        }
                    }
                    else
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        response.Invoke(false);
                    }
                }
                else
                {
                    popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    response.Invoke(false);
                }
            }
            else
            {
                popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                await PopupNavigation.Instance.PushAsync(popupnav);
                response.Invoke(false);
            }
        }
        #endregion
    }
}

