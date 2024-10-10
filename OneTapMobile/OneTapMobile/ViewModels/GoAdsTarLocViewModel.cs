using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class GoAdsTarLocViewModel : BaseViewModel
    {
        #region properties

        private ObservableCollection<GoAdsTarLocResult> _GoAdMasterLocList = new ObservableCollection<GoAdsTarLocResult>();
        public ObservableCollection<GoAdsTarLocResult> GoAdMasterLocList
        {
            get { return _GoAdMasterLocList; }
            set { _GoAdMasterLocList = value; OnPropertyChanged("GoAdMasterLocList"); }
        }


        private ObservableCollection<GoAdsTarLocResult> _GoAdLocationList = new ObservableCollection<GoAdsTarLocResult>();
        public ObservableCollection<GoAdsTarLocResult> GoAdLocationList
        {
            get { return _GoAdLocationList; }
            set { _GoAdLocationList = value; OnPropertyChanged("GoAdLocationList"); }
        }

        private ObservableCollection<GoAdsTarLocResult> _GoAdSelectedLocList = new ObservableCollection<GoAdsTarLocResult>();
        public ObservableCollection<GoAdsTarLocResult> GoAdSelectedLocList
        {
            get { return _GoAdSelectedLocList; }
            set { _GoAdSelectedLocList = value; OnPropertyChanged("GoAdSelectedLocList"); }
        }


        private GoAdsTarLocResult _SelectedLoc;
        public GoAdsTarLocResult SelectedLoc
        {
            get
            {
                return _SelectedLoc;
            }
            set
            {

                _SelectedLoc = value;
                OnPropertyChanged("SelectedLoc");
                //if (SelectedLoc != null)
                //{
                //    GoAdLocationList.ToList().Where(p => p.id == SelectedLoc.id).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                //    GoAdLocationList.ToList().Where(p => p.id != SelectedLoc.id).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                //}
                try
                {
                    if (SelectedLoc != null)
                    {
                        if (!GoAdSelectedLocList.Contains(SelectedLoc))
                        {
                            GoAdLocationList.ToList().Where(p => p.id == SelectedLoc.id).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                            GoAdSelectedLocList.Add(SelectedLoc);
                            SelectedLoc = null;
                        }
                        else
                        {
                            GoAdLocationList.ToList().Where(p => p.id == SelectedLoc.id).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                            if (GoAdSelectedLocList.Contains(SelectedLoc))
                                GoAdSelectedLocList.Remove(SelectedLoc);
                            SelectedLoc = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }


            }
        }

        private string _Tick = "GrayRightTick";
        public string Tick
        {
            get { return _Tick; }
            set { _Tick = value; OnPropertyChanged("Tick"); }
        }


        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set { _SearchText = value; OnPropertyChanged("SearchText"); }
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
        public async Task SearchLocations()
        {
            try
            {
                    IsBusy = true;
                    GoAdsTarLocRequestModel goAdsTarLocRequestModel = new GoAdsTarLocRequestModel
                    {
                        search_text = "",
                    };
                    try
                    {
                        string url = "user/google-ads-locations";
                        Rest_ResponseModel rest_result = await WebService.WebService.PostData(goAdsTarLocRequestModel, url, true);
                        if (rest_result != null)
                        {
                            if (rest_result.status_code == 200)
                            {
                                var goAdsTarLocResponseModel = JsonConvert.DeserializeObject<GoAdsTarLocResponseModel>(rest_result.response_body);

                                if (goAdsTarLocResponseModel != null)
                                {
                                    if (goAdsTarLocResponseModel.status)
                                    {
                                        GoAdMasterLocList = new ObservableCollection<GoAdsTarLocResult>(goAdsTarLocResponseModel.result);
                                        IsBusy = false;
                                    }
                                    else
                                    {
                                        var popupnav = new UserDialogPopup(Constant.PopupTitle, goAdsTarLocResponseModel.message, "Ok");
                                        await PopupNavigation.Instance.PushAsync(popupnav);
                                        IsBusy = false;

                                    }
                                }
                                else
                                {
                                    var popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                                    await PopupNavigation.Instance.PushAsync(popupnav);
                                    IsBusy = false;
                                }
                            }
                            else
                            {
                                var popupnav = new UserDialogPopup(Constant.PopupTitle,"No search result found", "Ok");
                                await PopupNavigation.Instance.PushAsync(popupnav);
                                IsBusy = false;
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        IsBusy = false;
                    }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex.Message);
            }
        }


        private void Cancel()
        {
            PopupNavigation.Instance.PopAsync();
        }


        private void Done()
        {
            MessagingCenter.Send<object,ObservableCollection<GoAdsTarLocResult>>(this, "ChoosedLocations", GoAdSelectedLocList);
            PopupNavigation.Instance.PopAsync();
        }

        public async Task SearchLocationsFromMaster()
        {
            try
            {
                if (SearchText.Count() >= 2)
                {
                    if (GoAdMasterLocList != null)
                    {
                        if (SearchText != null)
                        {
                            var GoAdLocList = GoAdMasterLocList.Where(p => p.name.ToLower().StartsWith(SearchText.ToLower())).ToList();

                            if (GoAdLocationList.Count > 0)
                            {
                                _GoAdLocationList.Clear();
                            }
                            foreach (var i in GoAdLocList)
                            {
                                _GoAdLocationList.Add(i);
                            }

                            GoAdLocationList = _GoAdLocationList;
                        }
                        else
                        {
                            var popupnav = new UserDialogPopup(Constant.PopupTitle, "Please Enter Text", "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                        }
                    }
                    else
                    {
                        var popupnav = new UserDialogPopup(Constant.PopupTitle, "No search result found", "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                    }
                }
                else if(SearchText.Length == 0){
                    _GoAdLocationList.Clear();
                }
              
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
