using Acr.UserDialogs;
using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using OneTapMobile.Popups;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using OneTapMobile.Services;
using OneTapMobile.Interface;

namespace OneTapMobile.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        #region properties
        public INavigation nav;
        private readonly IForgotPasswordService _ForgotPasswordService;

        public ForgotPasswordViewModel()
        {
            _ForgotPasswordService = DependencyService.Get<IForgotPasswordService>();
        }

        public string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged("Email");
            }
        }


        public bool _Safearea = true;
        public bool Safearea
        {
            get
            {
                return _Safearea;
            }
            set
            {
                _Safearea = value;
                OnPropertyChanged("Safearea");

            }
        }

        public string _LoaderText;
        public string LoaderText
        {
            get
            {
                return _LoaderText;
            }
            set
            {
                _LoaderText = value;
                OnPropertyChanged("LoaderText");
            }
        }

        #endregion

        #region commands
        private Command goBackBtn;
        public ICommand GoBackBtn
        {
            get
            {
                if (goBackBtn == null)
                {
                    goBackBtn = new Command(PerformGoBackBtn);
                }
                return goBackBtn;
            }
        }

        private Command submitCmd;

        public ICommand SubmitCmd
        {
            get
            {
                if (submitCmd == null)
                {
                    submitCmd = new Command(PerformSubmitCmd);
                }

                return submitCmd;
            }
        }
        #endregion

        #region Back button pressed method
        private void PerformGoBackBtn()
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

        #region forgot password
        private async void PerformSubmitCmd()
        {
            IsEnable = false;

            if (!Conn)
            {
                return;
            }
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                Safearea = false;
                if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Email))
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "Please insert email address", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    IsEnable = true;
                    return;
                }
                else if (!Helper.ValidateEmail(Email))
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "Please insert a valid email address", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    IsEnable = true;
                    return;
                }
                else
                {
                    IsBusy = true;
                    LoaderText = "Requesting for new password.";
                    CreateAccountRequestModel createRequestModel = new CreateAccountRequestModel
                    {
                        email = Email,
                    };

                    await _ForgotPasswordService.ForgotPassword(nav, createRequestModel, async (res) =>
                     {
                         if (res)
                         {
                             IsBusy = false;
                             Safearea = true;
                         }
                         else
                         {
                             IsTap = false;
                             popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                             await PopupNavigation.Instance.PushAsync(popupnav);
                             return;
                         }
                     });
                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                IsBusy = false;
                Safearea = true;
                IsEnable = true;
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
                Safearea = true;
                IsEnable = true;
            }

        }
        #endregion

        #region Popup Ok button clicked (for navigation to new page)
        private async void  Popupnav_eventDoneAsync(object sender, EventArgs e)
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
              await nav.PopAsync();
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
