using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.LocalDataBase;
using OneTapMobile.Popups;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region shared properties

        bool isBusy = false;
        public bool IsBusy
        {
            get {
                return isBusy; 
            }
            set {
                SetProperty(ref isBusy, value);
            }
        }

        bool isChecking = false;
        public bool IsChecking
        {
            get { return isChecking; }
            set { SetProperty(ref isChecking, value); }
        }

        //bool isTapped = true;
        //public bool IsTapped
        //{
        //    get { return isTapped; }
        //    set { SetProperty(ref isTapped, value); }
        //}

        bool isTap = false;
        public bool IsTap
        {
            get { return isTap; }
            set { SetProperty(ref isTap, value); }
        }

        bool isshowEmpty = false;
        public bool IsshowEmpty
        {
            get { return isshowEmpty; }
            set { SetProperty(ref isshowEmpty, value); }
        }

        public Thickness SafeAreaSpacing
        {
            get { return DependencyService.Get<INotchService>().HasNotch() ? new Thickness(10, 50,10,20) : new Thickness(10, 20); }
        }


        bool isEnable = true;
        public bool IsEnable
        {
            get { return isEnable; }
            set { SetProperty(ref isEnable, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        string _StartColor = "#FFF5DD";
        public string StartColor
        {
            get { return _StartColor; }
            set { SetProperty(ref _StartColor, value); }
        }

        string _EndColor = "#FFE3A5";
        public string EndColor
        {
            get { return _EndColor; }
            set { SetProperty(ref _EndColor, value); }
        }



        private bool _IsEnabledBtn = false;
        public bool IsEnabledBtn
        {
            get
            {
                return _IsEnabledBtn;
            }
            set
            {
                _IsEnabledBtn = value;
                OnPropertyChanged("IsEnabledBtn");
            }
        }

        private double _opacityBtn = 0.5;
        public double OpacityBtn
        {
            get
            {
                return _opacityBtn;
            }
            set
            {
                _opacityBtn = value;
                OnPropertyChanged("OpacityBtn");
            }
        }

        #endregion

        #region generic properties setting
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;
            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserDialogPopup popupnav = new UserDialogPopup();
        #endregion

        #region check network connection

        public BaseViewModel()
        {
            CheckWifiOnStart();
            //CheckWifiContinuously();
        }



        private bool _conn;
        public bool Conn
        {
            get => _conn;
            set
            {
                _conn = value;
                OnPropertyChanged("Conn");

            }
        }

        public void CheckWifiOnStart()
        {
            Conn = CrossConnectivity.Current.IsConnected ? true : false;
            if (Conn == false)
            {
                popupnav = new UserDialogPopup(Constant.PopupTitle, "Internet not available", "Ok");
                PopupNavigation.Instance.PushAsync(popupnav);
                IsChecking = false;
            }
        }

        public void CheckWifiContinuously()
        {
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                Conn = args.IsConnected ? true : false;
                if (Conn == false)
                {
                    if (PopupNavigation.Instance.PopupStack.Count >= 1)
                    {
                        PopupNavigation.Instance.PopAllAsync();

                        if (PopupNavigation.Instance.PopupStack.Count == 0)
                        {
                            popupnav = new UserDialogPopup(Constant.PopupTitle, "Internet not available", "Ok");
                            PopupNavigation.Instance.PushAsync(popupnav);
                            IsChecking = false;
                        }
                    }
                    else
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, "Internet not available", "Ok");
                        PopupNavigation.Instance.PushAsync(popupnav);
                        IsChecking = false;
                    }
                }
            };
        }
        #endregion

    }
    #region SwipeLeftToBackNavigation'
    
    
    public class CustomContentPage: ContentPage
    {

        ContentPage contentPage;
        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);

            var leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            leftSwipeGesture.Swiped += OnSwiped;
            contentPage.Content.GestureRecognizers.Add(leftSwipeGesture);

        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            Navigation.PopAsync();
        }
    }

    #endregion

    public class PropertyChangedModel : INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;
            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }


}
