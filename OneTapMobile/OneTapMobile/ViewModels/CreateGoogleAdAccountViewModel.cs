using OneTapMobile.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class CreateGoogleAdAccountViewModel :BaseViewModel
    {
        #region Properties

        public INavigation nav;

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

        private string _AccountNameEntry;
        public string AccountNameEntry
        {
            get
            {
                return _AccountNameEntry;
            }
            set
            {
                _AccountNameEntry = value;
                OnPropertyChanged("AccountNameEntry");
            }
        }
        private string _TimeZoneEnty;
        public string TimeZoneEnty
        {
            get
            {
                return _TimeZoneEnty;
            }
            set
            {
                _TimeZoneEnty = value;
                OnPropertyChanged("TimeZoneEnty");
            }
        }
        private string _CurrencyEntry;
        public string CurrencyEntry
        {
            get
            {
                return _CurrencyEntry;
            }
            set
            {
                _CurrencyEntry = value;
                OnPropertyChanged("CurrencyEntry");
            }
        }
        #endregion

        #region Commands
        public Command NextbtnCmd { get; set; }

        #endregion

        #region Constructor
        public CreateGoogleAdAccountViewModel()
        {
            NextbtnCmd = new Command(async () => await NextbtnCmdMethod());
        }

        #endregion


        #region Next Button Clicked (which will Ad a new google ad account)
        private async Task NextbtnCmdMethod()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(5000);
                IsBusy = false;
                IsChecking = true;
                await Task.Delay(5000);
                await nav.PushAsync(new DashBoard());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsBusy = false;
                IsChecking = false;

            }
        }
        #endregion

        #region back button command
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


    }
}
