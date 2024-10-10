using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Views;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using OneTapMobile.Popups;
using Rg.Plugins.Popup.Services;
using OneTapMobile.Services;
using OneTapMobile.Interface;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OneTapMobile.ViewModels
{
    public class EmailLoginViewModel : BaseViewModel
    {
        #region properties
        public INavigation nav;

        private readonly IEmailLoginService _EmailLoginService;

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
        private bool _IsPassword = true;
        public bool IsPassword
        {
            get
            {
                return _IsPassword;
            }
            set
            {
                _IsPassword = value;
                OnPropertyChanged("IsPassword");
            }
        }
        private bool _HidePassWord = false;
        public bool HidePassWord
        {
            get
            {
                return _HidePassWord;
            }
            set
            {
                _HidePassWord = value;
                OnPropertyChanged("HidePassWord");
            }
        }
        private bool _ViewPassWord = true;
        public bool ViewPassWord
        {
            get
            {
                return _ViewPassWord;
            }
            set
            {
                _ViewPassWord = value;
                OnPropertyChanged("ViewPassWord");
            }
        }
        public string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                OnPropertyChanged("Password");
            }
        }
        #endregion

        #region commands
        public Command ForgotPasswordCmd { get; set; }
        public Command SignUpCmd { get; set; }
        private Command _LoginCmd;
        public Command LoginCmd
        {
            get { return _LoginCmd ?? (_LoginCmd = new Command(() => LoginMethod())); }
        }
        private Command _ViewPassWordCmd;
        public Command ViewPassWordCmd
        {
            get { return _ViewPassWordCmd ?? (_ViewPassWordCmd = new Command(() => ViewPassWordMethod())); }
        }
        private Command _HidePassWordCmd;
        public Command HidePassWordCmd
        {
            get { return _HidePassWordCmd ?? (_HidePassWordCmd = new Command(() => HidePassWordMethod())); }
        }
        private Command backBtn;
        public ICommand BackBtn
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

        #region Hide Password Method
        public void HidePassWordMethod()
        {
            HidePassWord = false;
            ViewPassWord = true;
            IsPassword = true;
        }
        #endregion

        #region Show Password Method
        public void ViewPassWordMethod()
        {
            HidePassWord = true;
            ViewPassWord = false;
            IsPassword = false;
        }
        #endregion

        #region constructor
        public EmailLoginViewModel()
        {
            _EmailLoginService = DependencyService.Get<IEmailLoginService>();
            ForgotPasswordCmd = new Command(() => ForgotPasswordmethod());
            SignUpCmd = new Command(() => SignUpMethod());
        }
        #endregion

        #region Login Method
        private async void LoginMethod()
        {
            if (!Conn)
            {
                IsTap = false;
                return;
            }
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                DependencyService.Get<IKeyboardHelper>().HideKeyboard();

                Email = Email.ToLower();

                if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Email))
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "Please insert email address", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    return;
                }
                else if (!Helper.ValidateEmail(Email))
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "Please insert a valid email address", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(Password))
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "Please insert Password", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    return;
                }
                else
                {
                    IsBusy = true;
                    EmailLoginRequestModel emailLoginRequestModel = new EmailLoginRequestModel
                    {
                        email = Email,
                        password = Password,
                    };
                    await _EmailLoginService.EmailLogin(emailLoginRequestModel, async (res) =>
                    {
                        if (res)
                        {
                            Constant.IsLoggedOut = false;
                            Helper.UserEmail = Email;
                            Helper.profileModel.UserName = "NA";
                            await RegisterUserToken();
                            App.Current.MainPage = new NavigationPage(new ConnectAccountView());
                            IsBusy = false;
                        }
                        else
                        {
                            IsTap = false;
                            IsBusy = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                IsBusy = false;
                //IsTapped = false;
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region sign Up method
        private void SignUpMethod()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                nav.PushAsync(new CreateAccountView());
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region forgot password button method
        private void ForgotPasswordmethod()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                nav.PushAsync(new ForgotPasswordView());
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region back button method
        private void PerformBackBtn()
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

        #region Register for user token method
        async Task RegisterUserToken()
        {
            try
            {
                if (Constant.DeviceToken != null && Constant.OneTapUserId != 0)
                {
                    NotificationRequestModel notificationRequestModel = new NotificationRequestModel
                    {
                        user_id = Convert.ToString(Constant.OneTapUserId),
                        device_token = Constant.DeviceToken
                    };


                    string url = "user/add-device-token";
                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(notificationRequestModel, url, true);
                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var NotificationResponseModel = JsonConvert.DeserializeObject<NotificationResponseModel>(rest_result.response_body);

                            if (NotificationResponseModel != null)
                            {
                                Debug.WriteLine("Token Registered for notification");
                            }
                            else
                            {
                                Debug.WriteLine("not Registered for notification");
                            }
                        }
                    }
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
