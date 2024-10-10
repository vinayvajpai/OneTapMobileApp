using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentPage
    {
        SettingsViewModel m_viewmodel;

        bool IsTap = false;
        public SettingsView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new SettingsViewModel();
        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (m_viewmodel != null)
                {
                    m_viewmodel.nav = this.Navigation;
                    m_viewmodel.IsTap = false;
                    MessagingCenter.Unsubscribe<object, bool>(this, "ChangeIsTapToFalse");
                    MessagingCenter.Subscribe<object, bool>(this, "ChangeIsTapToFalse", (sender, arg) =>
                    {
                        if (!arg)
                        {
                            m_viewmodel.IsTap = arg;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void SignOut_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                btnSignOut.IsEnabled = false;
                var popupnav = new UserDialogPopup("Confirm", "Are you sure you want to logout?", true, true, "OK", "Cancel");
                popupnav.eventOK += Popupnav_eventOK;
                popupnav.eventCancel += Popupnav_eventCancel;
                PopupNavigation.Instance.PushAsync(popupnav);
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private void Popupnav_eventCancel(object sender, EventArgs e)
        {
            IsTap = false;
            btnSignOut.IsEnabled = true;
        }

        private void Popupnav_eventOK(object sender, EventArgs e)
        {
            Helper.ResetLoginData();
            App.Current.MainPage = new NavigationPage(new LoginView());
            IsTap = false;
            btnSignOut.IsEnabled = true;
        }

        private async void PushSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            try
            {

                if (Constant.OneTapUserId != 0)
                {
                    ChangeSettingsRequestModel changeSettingsRequestModel = new ChangeSettingsRequestModel
                    {
                        user_id = Convert.ToString(Constant.OneTapUserId),
                      //  is_newsletter = NewsLetterSwitch.IsToggled ? 1 : 0,      // feature removed
                        is_newsletter = 0,
                        is_push_notification = PushButtonSwitch.IsToggled ? 1 : 0,

                    };
                    string url = "user/change-user-settings";
                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(changeSettingsRequestModel, url, true);
                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var changeSettingsResponseModel = JsonConvert.DeserializeObject<ChangeSettingsResponseModel>(rest_result.response_body);
                            Debug.WriteLine(changeSettingsResponseModel.message);
                        }
                    }
                    else
                    {
                       var popupnav = new UserDialogPopup(Constant.PopupTitle,"Can't communicate server now!! please try again later", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }
                }
                else
                {
                    var popupnav = new UserDialogPopup(Constant.PopupTitle, "User Id not found , please login and try again", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}