using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.LocalDataBase;
using OneTapMobile.LocalDataBases.DataBaseModel;
using OneTapMobile.Models;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class ListPopupViewModel : BaseViewModel
    {
        #region properties

        GoCustomerListResult SelectedTempAccount;
        private ObservableCollection<GoCustomerListResult> _AdAccountsList = new ObservableCollection<GoCustomerListResult>();
        public ObservableCollection<GoCustomerListResult> AdAccountsList
        {
            get { return _AdAccountsList; }
            set { _AdAccountsList = value; OnPropertyChanged("AdAccountsList"); }
        }

        private bool _FromNogoogleaccountview = false;
        public bool FromNogoogleaccountview
        {
            get { return _FromNogoogleaccountview; }
            set { _FromNogoogleaccountview = value; OnPropertyChanged("FromNogoogleaccountview"); }
        }
        private GoCustomerListResult _SelectedAccount;
        public GoCustomerListResult SelectedAccount
        {
            get
            {
                return _SelectedAccount;
            }
            set
            {
                try
                {
                    _SelectedAccount = value;
                    OnPropertyChanged("SelectedAccount");
                    if (SelectedAccount != null)
                    {
                        if (SelectedAccount.is_manager)
                        {
                            if (SelectedAccount.child_accounts != null)
                            {
                                if (SelectedAccount.child_accounts.Count > 0)
                                {
                                    SelectedAccount.child_accounts.ToList().Where(p => p.customer_id == SelectedAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                                    SelectedAccount.child_accounts.ToList().Where(p => p.customer_id != SelectedAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                                    AdAccountsList.ToList().Where(p => p.customer_id != SelectedAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGray"; p.IsSubAcc = false; });

                                    SelectedAccount.IsSubAcc = !SelectedAccount.IsSubAcc;
                                    SelectedAccount.ExpandArrow = SelectedAccount.IsSubAcc == true ? "RightDArrow" : "RightArrow";
                                    var tempList = AdAccountsList.ToList();
                                    AdAccountsList.Clear();
                                    AdAccountsList = new ObservableCollection<GoCustomerListResult>(tempList);
                                    OnPropertyChanged("AdAccountsList");
                                    if (SelectedAccount.IsSubAcc)
                                        SelectedTempAccount = SelectedAccount;
                                    else
                                    {
                                        SelectedTempAccount = null;
                                    }
                                    SelectedAccount = null;
                                }
                            }
                            else
                            {
                                AdAccountsList.ToList().Where(p => p.customer_id == SelectedAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                                AdAccountsList.ToList().Where(p => p.customer_id != SelectedAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGray"; p.IsSubAcc = false; });
                                var tempList = AdAccountsList.ToList();
                                AdAccountsList.Clear();
                                AdAccountsList = new ObservableCollection<GoCustomerListResult>(tempList);
                            }
                        }
                        else
                        {
                            AdAccountsList.ToList().Where(p => p.customer_id == SelectedAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                            AdAccountsList.ToList().Where(p => p.customer_id != SelectedAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGray"; p.IsSubAcc = false; });
                            var tempList = AdAccountsList.ToList();
                            AdAccountsList.Clear();
                            AdAccountsList = new ObservableCollection<GoCustomerListResult>(tempList);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }



            }
        }

        private ChildAccount _SelectedChildAccount;
        public ChildAccount SelectedChildAccount
        {
            get
            {
                return _SelectedChildAccount;
            }
            set
            {
                try
                {
                    _SelectedChildAccount = value;
                    OnPropertyChanged("SelectedChildAccount");
                    if (SelectedChildAccount != null)
                    {

                        SelectedTempAccount.child_accounts.ToList().Where(p => p.customer_id == SelectedChildAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                        SelectedTempAccount.child_accounts.ToList().Where(p => p.customer_id != SelectedChildAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                        AdAccountsList.ToList().Where(p => p.customer_id != SelectedTempAccount.customer_id).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
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

        #region methods
        private void Cancel()
        {
            try
            {
                MessagingCenter.Send<object, bool>(this, "ChangeIsTapToFalse", false);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void Done()
        {
            try
            {
                MessagingCenter.Send<object, bool>(this, "ChangeIsTapToFalse", false);

                var A = SelectedTempAccount;
                if (SelectedAccount != null)
                {
                    Helper.profileModel.google_ad_customer_id = SelectedAccount.customer_id;
                    Helper.profileModel.google_ad_manager_id = "0";
                    Helper.SelectedGoAdCustDetail = SelectedAccount;

                    Helper.SavePropertyData("SelectedGoAdCustDetailTick", SelectedAccount.Tick);
                    Helper.SavePropertyData("SelectedGoAdCustDetailcustomerid", SelectedAccount.customer_id);
                    Helper.SavePropertyData("SelectedGoAdCustDetailmanagerid", SelectedAccount.manager_id);
                    Helper.SavePropertyData("SelectedGoAdCustDetailIsSubAcc", SelectedAccount.IsSubAcc);
                    Helper.SavePropertyData("SelectedGoAdCustDetailExpandArrow", SelectedAccount.ExpandArrow);
                    Helper.SavePropertyData("SelectedGoAdCustDetailtimezone", SelectedAccount.time_zone);
                    Helper.SavePropertyData("SelectedGoAdCustDetailismanager", SelectedAccount.is_manager);
                    Helper.SavePropertyData("SelectedGoAdCustDetaildescriptivename", SelectedAccount.descriptive_name);
                    Helper.SavePropertyData("SelectedGoAdCustDetailcurrencycode", SelectedAccount.currency_code);
                    Helper.SavePropertyData("SelectedGoAdCustDetail", true);
                    Helper.SavePropertyData("google_ad_customer_id", SelectedAccount.customer_id);
                    Helper.SavePropertyData("profileModel", JsonConvert.SerializeObject(Helper.profileModel));
                }
                else if (SelectedTempAccount != null && SelectedChildAccount != null)
                {
                    AdAccountsList.Where(x => x.customer_id == SelectedTempAccount.customer_id).ToList().ForEach(x => x.IsSubAcc = false);
                    Helper.profileModel.google_ad_customer_id = SelectedChildAccount.customer_id;
                    Helper.profileModel.google_ad_manager_id = SelectedChildAccount.manager_id;
                    Helper.SavePropertyData("profileModel", JsonConvert.SerializeObject(Helper.profileModel));
                }

                Constant.GoogleAdAdded = true;
                Constant.SkipNowVisisble = true;
                PopupNavigation.Instance.PopAsync();

                if (!FromNogoogleaccountview)
                {
                    if (Helper.IsFacebookAdLogin)
                    {
                        App.Current.MainPage = new NavigationPage(new DashBoard());
                    }
                    else
                    {
                        MessagingCenter.Send<object, bool>(this, "MakeGoogleChecked", true);
                        App.Current.MainPage = new NavigationPage(new DashBoard());
                    }
                }
                else
                {
                    App.Current.MainPage = new NavigationPage(new DashBoard());
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
