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
    public class PopulateListViewModel : BaseViewModel
    {
        #region constructor
        public PopulateListViewModel()
        {

        }
        #endregion

        #region properties
        public bool ListForLoc { get; set; }

        private ObservableCollection<FBCitiesResult> _CitiesList = new ObservableCollection<FBCitiesResult>();
        public ObservableCollection<FBCitiesResult> CitiesList
        {
            get { return _CitiesList; }
            set { _CitiesList = value; OnPropertyChanged("CitiesList"); }
        }


        private ObservableCollection<FBCitiesResult> _SelectedCitiesList = new ObservableCollection<FBCitiesResult>();
        public ObservableCollection<FBCitiesResult> SelectedCitiesList
        {
            get { return _SelectedCitiesList; }
            set { _SelectedCitiesList = value; OnPropertyChanged("SelectedCitiesList"); }
        }


        private ObservableCollection<FBInterestResult> _InterestList = new ObservableCollection<FBInterestResult>();
        public ObservableCollection<FBInterestResult> InterestList
        {
            get { return _InterestList; }
            set { _InterestList = value; OnPropertyChanged("InterestList"); }
        }


        private ObservableCollection<FBInterestResult> _SelectedIntList = new ObservableCollection<FBInterestResult>();
        public ObservableCollection<FBInterestResult> SelectedIntList
        {
            get { return _SelectedIntList; }
            set { _SelectedIntList = value; OnPropertyChanged("SelectedIntList"); }
        }




        private FBCitiesResult _SelectedCity;
        public FBCitiesResult SelectedCity
        {
            get
            {
                return _SelectedCity;
            }
            set
            {

                _SelectedCity = value;
                OnPropertyChanged("SelectedCity");
                //if (SelectedCity != null)
                //{
                //    CitiesList.ToList().Where(p => p.key == SelectedCity.key).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                //    CitiesList.ToList().Where(p => p.key != SelectedCity.key).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                //}
                try
                {
                    if (SelectedCity != null)
                    {
                        if (!SelectedCitiesList.Contains(SelectedCity))
                        {
                            CitiesList.ToList().Where(p => p.key == SelectedCity.key).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                            SelectedCitiesList.Add(SelectedCity);
                            SelectedInt = null;
                            SelectedCity = null;
                        }
                        else
                        {
                            CitiesList.ToList().Where(p => p.key == SelectedCity.key).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                            if (SelectedCitiesList.Contains(SelectedCity))
                                SelectedCitiesList.Remove(SelectedCity);
                            SelectedInt = null;
                            SelectedCity = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }


            }
        }

        private FBInterestResult _SelectedInt;
        public FBInterestResult SelectedInt
        {
            get
            {
                return _SelectedInt;
            }
            set
            {

                _SelectedInt = value;
                OnPropertyChanged("SelectedInt");
                try
                {
                    if (SelectedInt != null)
                    {
                        if (!SelectedIntList.Contains(SelectedInt))
                        {
                            InterestList.ToList().Where(p => p.id == SelectedInt.id).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                            SelectedIntList.Add(SelectedInt);
                            SelectedInt = null;
                        }
                        else
                        {
                            InterestList.ToList().Where(p => p.id == SelectedInt.id).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                            if (SelectedIntList.Contains(SelectedInt))
                                SelectedIntList.Remove(SelectedInt);
                            SelectedInt = null;
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

        

        private string _SearchBoxTitle = "Search Box";
        public string SearchBoxTitle
        {
            get { return _SearchBoxTitle; }
            set { _SearchBoxTitle = value; OnPropertyChanged("SearchBoxTitle"); }
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

        #region Method
        public async Task IsListForLocMethod(bool ListForLoc)
        {
            if (ListForLoc)
            {
                await SearchLocations();
            }
            else
            {
                await SearchInterests();
            }
        }


        public async Task SearchLocations()
        {
            try
            {

                if (SearchText.Length >= 3)
                {
                    IsBusy = true;
                    FBCitiesRequestModel fbCitiesRequestModel = new FBCitiesRequestModel
                    {
                       // fb_access_token = Helper.facebookProfile.Token,
                       fb_access_token = Helper.facebookProfile.Token,
                        search_text = SearchText,
                    };
                    try
                    {
                        string url = "user/get-facebook-cities";
                        Rest_ResponseModel rest_result = await WebService.WebService.PostData(fbCitiesRequestModel, url, true);
                        if (rest_result != null)
                        {
                            if (rest_result.status_code == 200)
                            {
                                var fbCitiesResponseModel = JsonConvert.DeserializeObject<FBCitiesResponseModel>(rest_result.response_body);

                                if (fbCitiesResponseModel != null)
                                {
                                    if (fbCitiesResponseModel.status)
                                    {
                                        IsBusy = false;
                                        //SearchLocationList = new List<FBCitiesResult>(fbCitiesResponseModel.result); 
                                        CitiesList = new ObservableCollection<FBCitiesResult>(fbCitiesResponseModel.result);
                                        
                                    }
                                    else
                                    {
                                        //popupnav = new UserDialogPopup(Constant.PopupTitle, fbCitiesResponseModel.message, "Ok");
                                        //await PopupNavigation.Instance.PushAsync(popupnav);
                                        IsBusy = false;

                                    }
                                }
                                else
                                {
                                    //popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                                    //await PopupNavigation.Instance.PushAsync(popupnav);
                                    IsBusy = false;
                                }
                            }
                            else
                            {
                                //popupnav = new UserDialogPopup(Constant.PopupTitle,"No search result found", "Ok");
                                //await PopupNavigation.Instance.PushAsync(popupnav);
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

            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex.Message);
            }
        }


        public async Task SearchInterests()
        {
            try
            {
                if (SearchText.Length >= 3)
                {
                    IsBusy = true;
                    FBInterestRequestMdoel fbInterestRequestMdoel = new FBInterestRequestMdoel
                    {
                        fb_access_token = Helper.facebookProfile.Token,
                        //fb_access_token = "EAAIN2V0MZBMgBAFGbi90fqpFqiV4NKXqESZCqSnJQs4VRlOYxGRK46xhmZBqVEjosJMM23KXZBWM1fkTwv8IGBUYUzphZCnmZAhGLueZBaGgblMIBzZAACyFVZB8g2Lds3Gw1su7KVvzNLmuiCQDFwOogZCc1TWWmvHVJXcH4RP5QGkXqFGoUZBDJz7J8JkL935B4sZD",
                        search_text = SearchText,
                    };
                    try
                    {
                        string url = "user/get-facebook-interests";
                        Rest_ResponseModel rest_result = await WebService.WebService.PostData(fbInterestRequestMdoel, url, true);
                        if (rest_result != null)
                        {
                            if (rest_result.status_code == 200)
                            {
                                var fbInterestResponseModel = JsonConvert.DeserializeObject<FBInterestResponseModel>(rest_result.response_body);

                                if (fbInterestResponseModel != null)
                                {
                                    if (fbInterestResponseModel.status)
                                    {
                                        IsBusy = false;
                                        //SearchLocationList = new List<FBCitiesResult>(fbCitiesResponseModel.result); 
                                        InterestList = new ObservableCollection<FBInterestResult>(fbInterestResponseModel.result);
                                        
                                    }
                                    else
                                    {
                                        //var popupnav = new UserDialogPopup(Constant.PopupTitle, fbInterestResponseModel.message, "Ok");
                                        //await PopupNavigation.Instance.PushAsync(popupnav);
                                        IsBusy = false;

                                    }
                                }
                                else
                                {
                                    //var popupnav = new UserDialogPopup(Constant.PopupTitle, rest_result.ErrorMessage, "Ok");
                                    //await PopupNavigation.Instance.PushAsync(popupnav);
                                    IsBusy = false;
                                }
                            }
                            else
                            {
                                //var popupnav = new UserDialogPopup(Constant.PopupTitle, "No search result found", "Ok");
                                //await PopupNavigation.Instance.PushAsync(popupnav);
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
            if (ListForLoc)
            {
                MessagingCenter.Send<object,ObservableCollection<FBCitiesResult>>(this, "SelectedLocation", SelectedCitiesList);
            }
            else
            {
                MessagingCenter.Send<object,ObservableCollection<FBInterestResult>>(this, "SelectedInterest", SelectedIntList);
            }

            MessagingCenter.Unsubscribe<object, ObservableCollection<FBCitiesResult>>(this, "SelectedLocation");
            MessagingCenter.Unsubscribe<object, ObservableCollection<FBInterestResult>>(this, "SelectedInterest");
            PopupNavigation.Instance.PopAsync();
        }

        #endregion
    }
}
