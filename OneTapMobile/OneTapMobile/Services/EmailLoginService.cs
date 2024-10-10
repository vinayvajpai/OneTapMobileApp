using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Rg.Plugins.Popup.Services;
namespace OneTapMobile.Services
{
    public class EmailLoginService : IEmailLoginService
    {
        IGoogleClientManager _emailloginService = CrossGoogleClient.Current;
        EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate;
        #region constuctor
        public EmailLoginService()
        { }
        #endregion

        #region Email Login service
        public async Task EmailLogin(EmailLoginRequestModel emailLoginRequestModel, Action<bool> response)
        {
            string url = "login";
            UserDialogPopup popupnav;
            Rest_ResponseModel rest_result = await WebService.WebService.PostData(emailLoginRequestModel, url, false);
            if (rest_result != null)
            {
                if (rest_result.status_code == 200)
                {
                    var loginResponseModel = JsonConvert.DeserializeObject<EmailLoginResponseModel>(rest_result.response_body);
                    Helper.SetLoginUserData(rest_result.response_body);
                    if (loginResponseModel != null)
                    {
                        if (loginResponseModel.status)
                        {
                            Constant.Token = loginResponseModel.result.token;
                            Constant.OneTapUserId = loginResponseModel.result.id;
                            response.Invoke(true);
                        }
                        else
                        {
                            popupnav = new UserDialogPopup(Constant.PopupTitle, loginResponseModel.message, "Ok");
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
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "Password and email do not match? Please fill the correct username and password?", "Ok");
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
        #endregion
    }
}
