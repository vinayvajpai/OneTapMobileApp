using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace OneTapMobile.Services
{
    public class FBCampListService : IFBCampListService
    {
        DashBoardViewModel vm;

        public FBCampListService()
        {
        }

        public async Task FBCampList(INavigation nav, FBCampListRequestModel fBCampListRequestModel, Action<List<Campaign>> response)
        {
            try
            {
                UserDialogPopup popupnav;
                string url = "user/campaigns";
                Rest_ResponseModel rest_result = await WebService.WebService.PostData(fBCampListRequestModel, url, true);
                if (rest_result != null)
                {
                    
                    if (rest_result.status_code == 200)
                    {
                        var CampListResponseModel = JsonConvert.DeserializeObject<CampListResponseModel>(rest_result.response_body);

                        if (CampListResponseModel != null)
                        {
                          
                            if (CampListResponseModel.status)
                            {
                                List<Campaign> list = new List<Campaign>();
                                list = CampListResponseModel.result.campaigns;
                                response.Invoke(list);
                            }
                            else
                            {
                                response.Invoke(null);
                            }
                        }
                        else
                        {
                            popupnav = new UserDialogPopup(Constant.PopupTitle, CampListResponseModel.message, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            vm.IsBusy = false;
                            response.Invoke(null);
                        }
                    }
                    else
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        vm.IsBusy = false;
                        response.Invoke(null);
                    }
                }
                else
                {
                    popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    vm.IsBusy = false;
                    response.Invoke(null);
                }

            }
            catch (Exception ex)
            {
                vm.IsBusy = false;
                vm.IsChecking = false;
                Debug.WriteLine(ex.Message);

            }
            finally
            {
                vm.IsBusy = false;
                vm.IsChecking = false;
            }
        }
    }
}
