using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Properties

        public INavigation nav;

        public SettingsViewModel()
        {

        }

        private string _EmailId = "Na";
        public string EmailId
        {
            get
            {
                return _EmailId;
            }
            set
            {
                _EmailId = value;

                if (!string.IsNullOrWhiteSpace(Helper.UserEmail))
                {
                    _EmailId = Helper.UserEmail;
                }
                OnPropertyChanged("EmailId");
            }
        }

        #endregion

        #region Commands

        private ICommand backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new Command(PerformBackBtn);
                }
                return backCommand;
            }
        }

        private ICommand _AboutUsCommand;
        public ICommand AboutUsCommand
        {
            get
            {
                _AboutUsCommand = new Command<string>((cp) => OpenAboutUsTopic(cp));
                return _AboutUsCommand;
            }
        }

        private Command changePasswordTapped;

        public ICommand ChangePasswordTapped
        {
            get
            {
                if (changePasswordTapped == null)
                {
                    changePasswordTapped = new Command(PerformChangePasswordTapped);
                }

                return changePasswordTapped;
            }
        }
        
        private Command deleteUserTapped;

        public ICommand DeleteUserTapped
        {
            get
            {
                if (deleteUserTapped == null)
                {
                    deleteUserTapped = new Command(PerformDeleteUserTapped);
                }

                return deleteUserTapped;
            }
        }

        #endregion

        #region back button pressed command

        private void OpenAboutUsTopic(string CP)
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                WebViewPopup popupnavigate;
                string HTMLPageContent;
                switch (CP)
                {
                    case "0":
                        HTMLPageContent = Helper.ReadHtmlFileContent("TermsAndConditions.HTML");
                        popupnavigate = new WebViewPopup(HTMLPageContent);
                        PopupNavigation.Instance.PushAsync(popupnavigate);
                        break;

                    case "1":
                        HTMLPageContent = Helper.ReadHtmlFileContent("Disclaimer.HTML");
                        popupnavigate = new WebViewPopup(HTMLPageContent);
                        PopupNavigation.Instance.PushAsync(popupnavigate);
                        break;

                    case "2":
                        HTMLPageContent = Helper.ReadHtmlFileContent("PrivacyNotice.HTML");
                        popupnavigate = new WebViewPopup(HTMLPageContent);
                        PopupNavigation.Instance.PushAsync(popupnavigate);
                        break;

                    case "3":
                        HTMLPageContent = Helper.ReadHtmlFileContent("EULA.HTML");
                        popupnavigate = new WebViewPopup(HTMLPageContent);
                        PopupNavigation.Instance.PushAsync(popupnavigate);
                        break;

                    default:
                        IsTap = false;
                        UserDialogPopup popupnav = new UserDialogPopup("Failed", "Can't connect to server please try again.", "Ok");
                        PopupNavigation.Instance.PushAsync(popupnav);
                        break;
                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }



        private void PerformBackBtn(object obj)
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

        #region Open Change Password Screen method

        private void PerformChangePasswordTapped()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                nav.PushAsync(new ChangePasswordView());
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }


        #endregion

        #region delete user method
        private async void PerformDeleteUserTapped(object obj)
        {
            try
            {

                if (IsTap)
                    return;
                IsTap = true;

                popupnav = new UserDialogPopup("Alert", "are you sure you want to delete your account permanently?", true,true, "Ok","Cancel");
                popupnav.eventCancel += Popupnav_eventCancel;
                popupnav.eventOK += Popupnav_eventOK; ;
                await PopupNavigation.Instance.PushAsync(popupnav);
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
            }
        }


        private void Popupnav_eventCancel(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
            IsTap = false;
        }

        private async void Popupnav_eventOK(object sender, EventArgs e)
        {
            try
            {

                if (Constant.OneTapUserId != 0)
                {
                    DeleteUserRequestModel deleteUserRequestModel = new DeleteUserRequestModel
                    {
                        user_id = Convert.ToString(Constant.OneTapUserId)
                    };
                    string url = "user/delete-user";
                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(deleteUserRequestModel, url, true);
                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var deleteUserResponse = JsonConvert.DeserializeObject<DeleteUserResponseModel>(rest_result.response_body);
                            if(deleteUserResponse != null)
                            {
                                if(deleteUserResponse.status)
                                {
                                    Helper.ResetLoginData();
                                    App.Current.MainPage = new NavigationPage(new LoginView());
                                }

                                popupnav = new UserDialogPopup(Constant.PopupTitle, deleteUserResponse.message, "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsTap = false;
                            }

                        }
                    }
                    else
                    {
                        var popupnav = new UserDialogPopup(Constant.PopupTitle, "Can't communicate server now!! please try again later", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsTap = false;
                    }
                }
                else
                {
                    var popupnav = new UserDialogPopup(Constant.PopupTitle, "User Id not found, please login and try again", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    IsTap = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
