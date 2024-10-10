using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class TryAnotherGoogleAdAccViewModel : BaseViewModel
    {
        #region Constructor
        public TryAnotherGoogleAdAccViewModel()
        {
            NextbtnCmd = new Command(async () => await NextbtnCmdMethod());
        }

        #endregion

        #region Properties

        public INavigation nav;

        private string _AccountName;
        public string AccountName
        {
            get
            {
                return _AccountName;
            }
            set
            {
                _AccountName = value;
                OnPropertyChanged("_AccountName");
            }
        }
        private string _TimeZone;
        public string TimeZoneVal
        {
            get
            {
                return _TimeZone;
            }
            set
            {
                _TimeZone = value;
                OnPropertyChanged("TimeZoneVal");
            }
        }
        private string _Currency;
        public string CurrencyVal
        {
            get
            {
                return _Currency;
            }
            set
            {
                _Currency = value;
                OnPropertyChanged("CurrencyVal");
            }
        }
        #endregion

        #region Commands
        public Command NextbtnCmd { get; set; }

        private Command backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new Command(Back);
                }

                return backCommand;
            }
        }

        #endregion

        #region Next Button Clicked (which will Ad a new google ad account)
        private async Task NextbtnCmdMethod()
        {
            try
            { 
               await CreateGoogldAdAcc();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsBusy = false;
                IsChecking = false;

            }
        }


        private async Task CreateGoogldAdAcc()
        {
            try
            {
                var objGoogleAcc = Helper.GocustomerAccList.Where(g => g.is_manager == true).FirstOrDefault();
                var createAddAccModelReq = new CreateAddAccModelReq()
                {
                    user_id = Convert.ToString(Constant.OneTapUserId),
                    manager_id = objGoogleAcc.customer_id,
                    google_refresh_token = Helper.GoRefreshToken,
                    name = AccountName,
                    currency = CurrencyVal,
                    time_zone = TimeZoneVal
                };
                IsBusy = true;
                await Task.Delay(10);
                string url = "user/create-google-ads-account";
                Rest_ResponseModel rest_result = await WebService.WebService.PostData(createAddAccModelReq, url, true);
                if (rest_result != null)
                {
                    if (rest_result.status_code == 200)
                    {
                        var createAdResponseResult = JsonConvert.DeserializeObject<CreateAddAccResponse>(rest_result.response_body);
                        if (createAdResponseResult.status)
                        {
                            await GoogleAdAccountList(Helper.GoRefreshToken);
                        }
                        else
                        {
                            UserDialogPopup popupnav = new UserDialogPopup("Failed", createAdResponseResult.message, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            IsBusy = false;
                        }
                    }
                    else
                    {
                        IsBusy = false;
                        UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to create Google Ads account", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }
                   
                }
                else
                {
                    IsBusy = false;
                    UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to create Google Ads account", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex.Message);
                UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to create Google Ads account", "Ok");
                await PopupNavigation.Instance.PushAsync(popupnav);
            }
        }


        public async Task GoogleAdAccountList(string refreshToken)
        {
            try
            {
                if (refreshToken != null)
                {
                    IsBusy = true;
                    await Task.Delay(10);
                    GoogleAdRequestModel googleAdRequestModel = new GoogleAdRequestModel
                    {
                        user_id = Convert.ToString(Constant.OneTapUserId),
                        google_refresh_token = refreshToken,
                    };
                    string url = "user/google-ads-customers";
                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(googleAdRequestModel, url, true);
                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var googleAdResponseResult = JsonConvert.DeserializeObject<GoCustomerListResponse>(rest_result.response_body);
                            IsBusy = false;
                            if (googleAdResponseResult.status == true)
                            {
                                //UserDialogPopup popupnav = new UserDialogPopup("Success", "Created Google Ads account.", "Ok");
                                //await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                                IsChecking = true;
                                await Task.Delay(1000);
                                await nav.PopAsync();
                                IsChecking = false;
                            }
                            else
                            {

                                UserDialogPopup popupnav = new UserDialogPopup("Failed", googleAdResponseResult.message, "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                            }
                            //App.Current.MainPage = new NavigationPage(new DashBoard());
                            // need to add popup or a screen to select specific account from logged in Ad Account.

                        }
                        else
                        {
                            IsBusy = false;
                            UserDialogPopup popupnav = new UserDialogPopup("Failed", rest_result.ErrorMessage, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);

                        }
                    }
                    else
                    {
                        IsBusy = false;
                        UserDialogPopup popupnav = new UserDialogPopup("Failed", "Unable to login.", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }

                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex.Message);
            }

        }

        private void Popupnav_eventOK(object sender, EventArgs e)
        {
            this.nav.PopAsync();
        }


        #endregion

        #region  methods

        private void Back()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                nav.PopAsync();
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

    }
}
