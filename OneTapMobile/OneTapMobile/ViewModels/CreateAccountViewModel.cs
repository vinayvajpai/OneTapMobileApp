using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Services;
using OneTapMobile.Views.ErrorPage;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace OneTapMobile.ViewModels
{
    public class CreateAccountViewModel : BaseViewModel
    {
        #region properties
        public INavigation nav;
        private readonly ICreateAccountService _ICreateAccountService;
        public string _ConfirmPassword = String.Empty;
        public string ConfirmPassword
        {
            get
            {
                return _ConfirmPassword;
            }
            set
            {
                _ConfirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }
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
        public string _Email = String.Empty;
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
        public string _Password = String.Empty;
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
        private bool _PasswordErrorText = true;
        public bool PasswordErrorText
        {
            get
            {
                return _PasswordErrorText;
            }
            set
            {
                _PasswordErrorText = value;
                OnPropertyChanged("PasswordErrorText");
            }
        }
        private bool _ViewConPassWord = true;
        public bool ViewConPassWord
        {
            get
            {
                return _ViewConPassWord;
            }
            set
            {
                _ViewConPassWord = value;
                OnPropertyChanged("ViewConPassWord");
            }
        }
        private bool _HideConPassWord = false;
        public bool HideConPassWord
        {
            get
            {
                return _HideConPassWord;
            }
            set
            {
                _HideConPassWord = value;
                OnPropertyChanged("HideConPassWord");
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
        private bool _IsConfirmPassword = true;
        public bool IsConfirmPassword
        {
            get
            {
                return _IsConfirmPassword;
            }
            set
            {
                _IsConfirmPassword = value;
                OnPropertyChanged("IsConfirmPassword");
            }
        }
        #endregion

        #region commands
        public Command SignUpCmd { get; set; }
        public Command PrivacyPolicyCmd { get; set; }
        public Command TermofServiceCmd { get; set; }
        private Command _HidePassWordCmd;
        public Command HidePassWordCmd
        {
            get { return _HidePassWordCmd ?? (_HidePassWordCmd = new Command(() => HidePassWordMethod())); }
        }
        private Command _ViewPassWordCmd;
        public Command ViewPassWordCmd
        {
            get { return _ViewPassWordCmd ?? (_ViewPassWordCmd = new Command(() => ViewPassWordMethod())); }
        }
        private Command _HideCofirmPassCmd;
        public Command HideCofirmPassCmd
        {
            get { return _HideCofirmPassCmd ?? (_HideCofirmPassCmd = new Command(() => HideCofirmPassMethod())); }
        }
        private Command _ViewCofirmPassCmd;
        public Command ViewCofirmPassCmd
        {
            get { return _ViewCofirmPassCmd ?? (_ViewCofirmPassCmd = new Command(() => ViewCofirmPassMethod())); }
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

        #region constructor
        public CreateAccountViewModel()
        {
            PrivacyPolicyCmd = new Command(() => PrivacyPolicymethod());
            SignUpCmd = new Command(() => SignUpMethod());
            TermofServiceCmd = new Command(() => TermAndCondMethod());
            _ICreateAccountService = DependencyService.Get<ICreateAccountService>();
        }
        #endregion

        #region hide Password Method
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

        #region Hide confirm Password Method
        private void HideCofirmPassMethod()
        {
            HideConPassWord = false;
            ViewConPassWord = true;
            IsConfirmPassword = true;
        }
        #endregion

        #region Show confirm Password Method
        private void ViewCofirmPassMethod()
        {
            HideConPassWord = true;
            ViewConPassWord = false;
            IsConfirmPassword = false;
        }
        #endregion

        #region Sign Up account Method
        private async void SignUpMethod()
        {
            if (IsTap)
                return;
            IsTap = true;

            if(!Conn)
            {
                IsTap = false;
                return;
            }
            try
            {
                DependencyService.Get<IKeyboardHelper>().HideKeyboard();
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
                else if (string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Password))
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "Please insert Password", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    return;
                }
                else if (ConfirmPassword != Password)
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "password does not match with confirm password", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                }
                else
                {
                    IsBusy = true;
                    CreateAccountRequestModel createRequestModel = new CreateAccountRequestModel
                    {
                        email = Email,
                        password = Password,
                        password_confirm = ConfirmPassword
                    };

                    await _ICreateAccountService.CreateAccount(nav, createRequestModel, async (res) =>
                    {
                        if (res)
                        {
                            IsBusy = false;
                            IsSuccess = true;
                            await Task.Delay(2000);
                            await nav.PopAsync();
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
                Debug.WriteLine(ex.Message);

            }
        }
        #endregion

        #region Open privacy policy method
        private void PrivacyPolicymethod()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                WebViewPopup popupnavigate;
                string HTMLPageContent;
                HTMLPageContent = Helper.ReadHtmlFileContent("PrivacyNotice.HTML");
                popupnavigate = new WebViewPopup(HTMLPageContent);
                PopupNavigation.Instance.PushAsync(popupnavigate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
            }
        }
        #endregion

        #region Open Term And Condition Method
        private void TermAndCondMethod()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                WebViewPopup popupnavigate;
                string HTMLPageContent;
                HTMLPageContent = Helper.ReadHtmlFileContent("TermsAndConditions.HTML");
                popupnavigate = new WebViewPopup(HTMLPageContent);
                PopupNavigation.Instance.PushAsync(popupnavigate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
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
                Debug.WriteLine(ex.Message);
                IsTap=false;
            }
        }
        #endregion
    }
}
