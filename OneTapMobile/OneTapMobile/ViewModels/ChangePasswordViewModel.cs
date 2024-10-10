using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        #region Constructor
        public ChangePasswordViewModel()
        {
        }
        #endregion

        #region Properties
        public INavigation nav;
       // public IChangePasswordService changePasswordService;
        private bool _IsSuccess = false;
        public bool IsSuccess
        {
            get
            {
                return _IsSuccess;
            }
            set
            {
                _IsSuccess = value;
                OnPropertyChanged("IsSuccess");
            }
        }
        public string _OldPassword = String.Empty;
        public string OldPassword
        {
            get
            {
                return _OldPassword;
            }
            set
            {
                _OldPassword = value;
                OnPropertyChanged("OldPassword");
            }
        }
        private bool _HideOldPassWord = false;
        public bool HideOldPassWord
        {
            get
            {
                return _HideOldPassWord;
            }
            set
            {
                _HideOldPassWord = value;
                OnPropertyChanged("HideOldPassWord");
            }
        }
        private bool _ViewOldPassWord = true;
        public bool ViewOldPassWord
        {
            get
            {
                return _ViewOldPassWord;
            }
            set
            {
                _ViewOldPassWord = value;
                OnPropertyChanged("ViewOldPassWord");
            }
        }
        private bool _IsOldPassword = true;
        public bool IsOldPassword
        {
            get
            {
                return _IsOldPassword;
            }
            set
            {
                _IsOldPassword = value;
                OnPropertyChanged("IsOldPassword");
            }
        }
        public string _NewPassword = String.Empty;
        public string NewPassword
        {
            get
            {
                return _NewPassword;
            }
            set
            {
                _NewPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }
        private bool _HideNewPassWord = false;
        public bool HideNewPassWord
        {
            get
            {
                return _HideNewPassWord;
            }
            set
            {
                _HideNewPassWord = value;
                OnPropertyChanged("HideNewPassWord");
            }
        }
        private bool _ViewNewPassWord = true;
        public bool ViewNewPassWord
        {
            get
            {
                return _ViewNewPassWord;
            }
            set
            {
                _ViewNewPassWord = value;
                OnPropertyChanged("ViewNewPassWord");
            }
        }
        private bool _IsNewPassword = true;
        public bool IsNewPassword
        {
            get
            {
                return _IsNewPassword;
            }
            set
            {
                _IsNewPassword = value;
                OnPropertyChanged("IsNewPassword");
            }
        }
        public string _ConfirmNewPassword = String.Empty;
        public string ConfirmNewPassword
        {
            get
            {
                return _ConfirmNewPassword;
            }
            set
            {
                _ConfirmNewPassword = value;
                OnPropertyChanged("ConfirmNewPassword");
            }
        }
        private bool _ViewConNewPassWord = true;
        public bool ViewConNewPassWord
        {
            get
            {
                return _ViewConNewPassWord;
            }
            set
            {
                _ViewConNewPassWord = value;
                OnPropertyChanged("ViewConNewPassWord");
            }
        }
        private bool _HideConNewPassWord = false;
        public bool HideConNewPassWord
        {
            get
            {
                return _HideConNewPassWord;
            }
            set
            {
                _HideConNewPassWord = value;
                OnPropertyChanged("HideConNewPassWord");
            }
        }
        private bool _IsConfirmNewPassword = true;
        public bool IsConfirmNewPassword
        {
            get
            {
                return _IsConfirmNewPassword;
            }
            set
            {
                _IsConfirmNewPassword = value;
                OnPropertyChanged("IsConfirmNewPassword");
            }
        }
        #endregion

        #region  Commands
        private Command viewOldPassWordCmd;
        public ICommand ViewOldPassWordCmd
        {
            get
            {
                if (viewOldPassWordCmd == null)
                {
                    viewOldPassWordCmd = new Command(PerformViewOldPassWordCmd);
                }
                return viewOldPassWordCmd;
            }
        }
        private Command hideOldPassWordCmd;
        public ICommand HideOldPassWordCmd
        {
            get
            {
                if (hideOldPassWordCmd == null)
                {
                    hideOldPassWordCmd = new Command(PerformHideOldPassWordCmd);
                }
                return hideOldPassWordCmd;
            }
        }
        private Command viewNewPassWordCmd;
        public ICommand ViewNewPassWordCmd
        {
            get
            {
                if (viewNewPassWordCmd == null)
                {
                    viewNewPassWordCmd = new Command(PerformViewNewPassWordCmd);
                }
                return viewNewPassWordCmd;
            }
        }
        private Command hideNewPassWordCmd;
        public ICommand HideNewPassWordCmd
        {
            get
            {
                if (hideNewPassWordCmd == null)
                {
                    hideNewPassWordCmd = new Command(PerformHideNewPassWordCmd);
                }
                return hideNewPassWordCmd;
            }
        }
        private Command viewCofirmPassCmd;
        public ICommand ViewCofirmPassCmd
        {
            get
            {
                if (viewCofirmPassCmd == null)
                {
                    viewCofirmPassCmd = new Command(PerformViewCofirmPassCmd);
                }
                return viewCofirmPassCmd;
            }
        }
        private Command hideCofirmPassCmd;
        public ICommand HideCofirmPassCmd
        {
            get
            {
                if (hideCofirmPassCmd == null)
                {
                    hideCofirmPassCmd = new Command(PerformHideCofirmPassCmd);
                }
                return hideCofirmPassCmd;
            }
        }
        private Command changePasswordCmd;
        public ICommand ChangePasswordCmd
        {
            get
            {
                if (changePasswordCmd == null)
                {
                    changePasswordCmd = new Command(PerformChangePasswordCmd);
                }
                return changePasswordCmd;
            }
        }
        private Command backBtn;
        public Command BackBtn
        {
            get
            {
                if (backBtn == null)
                {
                    backBtn = new Command(PerformBackBtn);
                }
                return backBtn;
            }
        }
        #endregion

        #region Show and Hide Entry Field Text
        private void PerformViewOldPassWordCmd()
        {
            HideOldPassWord = true;
            ViewOldPassWord = false;
            IsOldPassword = false;
        }
        private void PerformViewNewPassWordCmd()
        {
            HideNewPassWord = true;
            ViewNewPassWord = false;
            IsNewPassword = false;
        }
        private void PerformViewCofirmPassCmd()
        {
            HideConNewPassWord = true;
            ViewConNewPassWord = false;
            IsConfirmNewPassword = false;
        }
        private void PerformHideOldPassWordCmd()
        {
            HideOldPassWord = false;
            ViewOldPassWord = true;
            IsOldPassword = true;
        }
        private void PerformHideNewPassWordCmd()
        {
            HideNewPassWord = false;
            ViewNewPassWord = true;
            IsNewPassword = true;
        }
        private void PerformHideCofirmPassCmd()
        {
            HideConNewPassWord = false;
            ViewConNewPassWord = true;
            IsConfirmNewPassword = true;
        }
        #endregion

        #region Method for changing the password
        private async void PerformChangePasswordCmd()
        {
            if (!Conn)
            {
                return;
            }
            else
            {
                try
                {
                    if (IsTap)
                        return;
                    IsTap = true;

                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                    if (string.IsNullOrWhiteSpace(OldPassword) || string.IsNullOrWhiteSpace(ConfirmNewPassword) || string.IsNullOrWhiteSpace(NewPassword))
                    {
                        IsTap = false;
                        popupnav = new UserDialogPopup(Constant.PopupTitle, "All Fields Are Required", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        return;
                    }

                    if(OldPassword.Length < 8 || ConfirmNewPassword.Length < 8  || NewPassword.Length < 8)
                    {
                        IsTap = false;
                        popupnav = new UserDialogPopup(Constant.PopupTitle, "Password can't be less then 8 charecters", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        return;
                    }

                    else if (NewPassword != ConfirmNewPassword)
                    {
                        IsTap = false;
                        popupnav = new UserDialogPopup(Constant.PopupTitle, "password does not match with confirm password", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }
                    else if (OldPassword == ConfirmNewPassword || OldPassword == NewPassword)
                    {
                        IsTap = false;
                        popupnav = new UserDialogPopup(Constant.PopupTitle, "old password can't be same as new password", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }
                    else
                    {
                        ChangePasswordRequestModel changePasswordRequestModel = new ChangePasswordRequestModel
                        {
                            user_id = Constant.OneTapUserId,
                            old_password = OldPassword,
                            new_password = NewPassword,
                            confirm_password = ConfirmNewPassword,
                        };

                        IsBusy = true;
                        await DependencyService.Get<IChangePasswordService>().ChangePassword(changePasswordRequestModel, async (res) =>
                        {
                            if (res)
                            {
                                IsBusy = false;
                                IsSuccess = true;
                                popupnav = new UserDialogPopup(Constant.PopupTitle, "Password Changed Successfully", "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                await nav.PopAsync();
                            }
                            else
                            {
                                IsBusy = false;
                                IsTap = false;
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    IsTap = false;
                    Debug.WriteLine(ex.Message);
                }
            }
        }
        #endregion

        #region back button method
        public void PerformBackBtn()
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
