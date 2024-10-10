using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class FBAccIdListPopupViewModel : BaseViewModel
    {
        #region properties
        public NavigationPage nav;

        private ObservableCollection<FBAccIdListResult> _AdAccountsList = new ObservableCollection<FBAccIdListResult>();
        public ObservableCollection<FBAccIdListResult> AdAccountsList
        {
            get { return _AdAccountsList; }
            set { _AdAccountsList = value; OnPropertyChanged("AdAccountsList"); }
        }

        //private bool _FromConnectAccountView;
        //public bool FromConnectAccountView
        //{
        //    get 
        //    {
        //        return _FromConnectAccountView;
        //    }
        //    set 
        //    { 
        //        _FromConnectAccountView = value; 
        //        OnPropertyChanged("FromConnectAccountView"); 
        //    }
        //}



        private FBAccIdListResult _SelectedAccount;
        public FBAccIdListResult SelectedAccount
        {
            get
            {
                return _SelectedAccount;
            }
            set
            {

                _SelectedAccount = value;
                OnPropertyChanged("SelectedAccount");
                if (SelectedAccount != null)
                {
                    AdAccountsList.ToList().Where(p => p.account_id == SelectedAccount.account_id).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                    AdAccountsList.ToList().Where(p => p.account_id != SelectedAccount.account_id).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                }
            }
        }

        #endregion

        #region commands
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

        #endregion

        #region Methods
        private void Cancel()
        {
            MessagingCenter.Send<object, bool>(this, "ChangeIsTapToFalse", false);
            PopupNavigation.Instance.PopAsync();
        }

        private void Done()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                MessagingCenter.Send<object, bool>(this, "ChangeIsTapToFalse", false);

                if (SelectedAccount != null)
                {
                    Helper.profileModel.fb_ad_account_id = SelectedAccount.account_id;
                    Helper.profileModel.currency = SelectedAccount.currency;
                    Helper.SavePropertyData("profileModel", JsonConvert.SerializeObject(Helper.profileModel));
                     //eventOK.Invoke(sender, e);
                     PopupNavigation.Instance.PopAsync();
                    //if (!FromConnectAccountView)
                    //{
                    //    App.Current.MainPage = new NavigationPage(new DashBoard());
                    //}
                    //else
                    //{
                    //    App.Current.MainPage.Navigation.PushAsync(new FBPageListView());
                    //}

                    App.Current.MainPage.Navigation.PushAsync(new FBPageListView());

                }
                //list.SelectedItem = null;   
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
            }

            PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}
