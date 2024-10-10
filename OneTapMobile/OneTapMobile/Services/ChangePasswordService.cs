using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OneTapMobile.Services
{
    public class ChangePasswordService :IChangePasswordService
    {
        #region Constructor
        public ChangePasswordService()
        {
        }
        #endregion

        #region Change Password Service
        public async Task ChangePassword(ChangePasswordRequestModel changePasswordRequestModel, Action<bool> response)
        {
            UserDialogPopup popupnav;
            string url = "user/change-password";
            Rest_ResponseModel rest_result = await WebService.WebService.PostData(changePasswordRequestModel, url, true);
            if (rest_result != null)
            {
                if (rest_result.status_code == 200)
                {
                    ChangePasswordResponseModel changePasswordResponseModel = JsonConvert.DeserializeObject<ChangePasswordResponseModel>(rest_result.response_body);
                    if (changePasswordResponseModel != null)
                    {
                        if (changePasswordResponseModel.status)
                        {
                            response.Invoke(true);
                        }
                        else
                        {
                            popupnav = new UserDialogPopup(Constant.PopupTitle, changePasswordResponseModel.message, "Ok");
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
