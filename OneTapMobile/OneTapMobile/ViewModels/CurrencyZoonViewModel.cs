using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using OneTapMobile.Global;
using OneTapMobile.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class CurrencyZoonViewModel : BaseViewModel
    {
        public CurrencyZoonViewModel(bool IsCurrency=false)
        {
            if (IsCurrency)
                CurrencyList = new ObservableCollection<CurrencyModel>(Helper.GetCurrencyList());
            else
                TimeZoneList = new ObservableCollection<string>(Helper.GetTimeZoneList());
        }

        private ObservableCollection<string> _TimeZoneList = new ObservableCollection<string>();
        public ObservableCollection<string> TimeZoneList
        {
            get { return _TimeZoneList; }
            set { _TimeZoneList = value; OnPropertyChanged("TimeZoneList"); }
        }

        private ObservableCollection<CurrencyModel> _CurrencyList = new ObservableCollection<CurrencyModel>();
        public ObservableCollection<CurrencyModel> CurrencyList
        {
            get { return _CurrencyList; }
            set { _CurrencyList = value; OnPropertyChanged("CurrencyList"); }
        }

        private string _Selectedvalue;
        public string Selectedvalue
        {
            get
            {
                return _Selectedvalue;
            }
            set
            {

                _Selectedvalue = value;
                OnPropertyChanged("Selectedvalue");
               
            }
        }

        private Command cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new Command(Cancel);
                }

                return cancelCommand;
            }
        }



        private Command doneCommand;

        public ICommand DoneCommand
        {
            get
            {
                if (doneCommand == null)
                {
                    doneCommand = new Command(Done);
                }

                return doneCommand;
            }
        }


        private void Cancel()
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void Done()
        {
            try
            {
                //list.SelectedItem = null;   
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            PopupNavigation.Instance.PopAsync();
        }
    }
}

