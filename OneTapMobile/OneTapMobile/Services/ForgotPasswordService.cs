using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace OneTapMobile.Services
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        INavigation _nav;

        #region Constructor
        public ForgotPasswordService()
        {
        }
        #endregion

        #region ForgotPassword Service
        public async Task ForgotPassword(INavigation nav, CreateAccountRequestModel createRequestModel, Action<bool> response)
        {
            _nav = nav;
            string url = "forget";
            UserDialogPopup popupnav;
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
                            popupnav = new UserDialogPopup(Constant.PopupTitle, CreateAccountResponseModel.message, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            popupnav.eventCancel += Popupnav_eventDoneAsync;
                        }
                        else
                        {
                            popupnav = new UserDialogPopup(Constant.PopupTitle, CreateAccountResponseModel.message, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            return;
                        }
                    }
                    else
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, CreateAccountResponseModel.message, "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        return;
                    }
                }
                else
                {
                    popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    return;
                }
            }
            else
            {
                popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok"); ;
                await PopupNavigation.Instance.PushAsync(popupnav);
                return;
            }
        }
        #endregion

        public async void Popupnav_eventDoneAsync(object sender, EventArgs e)
        {
            await _nav.PopAsync();
        }
    }

}

