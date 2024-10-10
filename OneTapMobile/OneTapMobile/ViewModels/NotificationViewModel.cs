using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
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
    public class NotificationViewModel : BaseViewModel
    {
        #region properties
        public INavigation nav;

        private NotificationListResResult _SelectedNotification;
        public NotificationListResResult SelectedNotification
        {
            get
            {
                return _SelectedNotification;
            }
            set
            {
                _SelectedNotification = value;
                OnPropertyChanged("SelectedNotification");
            }
        }


        private ObservableCollection<NotificationListResResult> _notificationList;
        public ObservableCollection<NotificationListResResult> NotificationList
        {
            get { return _notificationList; }
            set
            {
                _notificationList = value;
                OnPropertyChanged("NotificationList");
            }
        }

        #endregion

        #region commands

        private Command backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new Command(Back);
                }

                return backCommand;
            }
        }

        private Command helpCommand;
        public ICommand HelpCommand
        {
            get
            {
                if (helpCommand == null)
                {
                    helpCommand = new Command(Help);
                }

                return helpCommand;
            }
        }

        private Command _SelectedNotificationCmd;
        public ICommand SelectedNotificationCmd
        {
            get
            {
                if (_SelectedNotificationCmd == null)
                {
                    _SelectedNotificationCmd = new Command(TappedNotificationCmd);
                }

                return _SelectedNotificationCmd;
            }
        }

        #endregion

        #region methods
        private void Back()
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

        private void Help()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                nav.PushAsync(new HelpGuideView());
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        public async void AddNotification()
        {
            try
            {
                _notificationList = new ObservableCollection<NotificationListResResult>();

                if (Constant.OneTapUserId != 0)
                {
                    NotificationListReqModel notificationListReqModel = new NotificationListReqModel()
                    {
                        user_id = Convert.ToString(Constant.OneTapUserId),
                    };


                    string url = "user/user-notifications";
                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(notificationListReqModel, url, true);
                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var notificationListResModel = JsonConvert.DeserializeObject<NotificationListResModel>(rest_result.response_body);
                            if(notificationListResModel != null)
                            {
                                if(notificationListResModel.status)
                                {
                                    _notificationList = new ObservableCollection<NotificationListResResult>(notificationListResModel.result);
                                    NotificationList = _notificationList;
                                }
                                else
                                {
                                    popupnav = new UserDialogPopup(Constant.PopupTitle, notificationListResModel.message, "Ok");
                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                    IsBusy = false;
                                }
                            }
                            else
                            {
                                popupnav = new UserDialogPopup(Constant.PopupTitle, notificationListResModel.message, "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                            }
                        }
                        else
                        {
                            popupnav = new UserDialogPopup(Constant.PopupTitle,"Couldn't connect to server try again later", "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            IsBusy = false;
                        }
                    }
                    else
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, "Couldn't connect to server try again later", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                    }
                }
                else
                {
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "User Id not found please login and try again!", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
           
        }

        private async void TappedNotificationCmd(object obj)
        {
            try
            {
                if (Constant.OneTapUserId != 0 && SelectedNotification.status != 1)
                {
                    // Here status = 1 means it is read.
                    NotificationReadUpdateReqmodel notificationReadUpdateReqmodel = new NotificationReadUpdateReqmodel()
                    {
                        user_id = Convert.ToString(Constant.OneTapUserId),
                        notification_id = SelectedNotification.id,
                        status = 1,
                    };

                    string url = "user/read-notification";
                    Rest_ResponseModel rest_result = await WebService.WebService.PostData(notificationReadUpdateReqmodel, url, true);
                    if (rest_result != null)
                    {
                        if (rest_result.status_code == 200)
                        {
                            var notificationReadUpdateResmodel = JsonConvert.DeserializeObject<NotificationReadUpdateResmodel>(rest_result.response_body);
                            if (notificationReadUpdateResmodel != null)
                            {
                                if (notificationReadUpdateResmodel.status)
                                {
                                    AddNotification();
                                    Debug.WriteLine("Read Status Updated successfully");
                                }
                                else
                                {
                                    popupnav = new UserDialogPopup(Constant.PopupTitle, notificationReadUpdateResmodel.message, "Ok");
                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                    IsBusy = false;
                                }
                            }
                            else
                            {
                                popupnav = new UserDialogPopup(Constant.PopupTitle, notificationReadUpdateResmodel.message, "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                            }
                        }
                        else
                        {
                            popupnav = new UserDialogPopup(Constant.PopupTitle, "Couldn't connect to server try again later", "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            IsBusy = false;

                        }
                    }
                    else
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, "Couldn't connect to server try again later", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                    }
                }
                else
                {
                    if (Constant.OneTapUserId == 0)
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, "User Id not found please login and try again!", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                    }
                    else
                    {
                        // NOTHING TO DO HERE
                    }
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
    }
}
