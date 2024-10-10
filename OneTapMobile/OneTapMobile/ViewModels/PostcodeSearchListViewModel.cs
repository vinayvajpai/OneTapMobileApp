using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using OneTapMobile.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
	public class PostcodeSearchListViewModel : BaseViewModel
	{
        List<Models.FBCitiesResult> _fBCities;
        #region constructor
        public PostcodeSearchListViewModel(List<Models.FBCitiesResult> fBCities = null)
        {
            _fBCities = fBCities;
        }
        #endregion

        #region properties
        public bool ListForLoc { get; set; }

        private ObservableCollection<Models.PostCodesResult> _PostalCodeList = new ObservableCollection<PostCodesResult>();
        public ObservableCollection<PostCodesResult> PostalCodeList
        {
            get { return _PostalCodeList; }
            set { _PostalCodeList = value; OnPropertyChanged("PostalCodeList"); }
        }


        private ObservableCollection<PostCodesResult> _SelectedPostList = new ObservableCollection<PostCodesResult>();
        public ObservableCollection<PostCodesResult> SelectedPostList
        {
            get { return _SelectedPostList; }
            set { _SelectedPostList = value; OnPropertyChanged("SelectedPostList"); }
        }


        //private ObservableCollection<FBInterestResult> _InterestList = new ObservableCollection<FBInterestResult>();
        //public ObservableCollection<FBInterestResult> InterestList
        //{
        //    get { return _InterestList; }
        //    set { _InterestList = value; OnPropertyChanged("InterestList"); }
        //}


        //private ObservableCollection<FBInterestResult> _SelectedIntList = new ObservableCollection<FBInterestResult>();
        //public ObservableCollection<FBInterestResult> SelectedIntList
        //{
        //    get { return _SelectedIntList; }
        //    set { _SelectedIntList = value; OnPropertyChanged("SelectedIntList"); }
        //}




        private PostCodesResult _SelectedPostCode;
        public PostCodesResult SelectedPostCode
        {
            get
            {
                return _SelectedPostCode;
            }
            set
            {

                _SelectedPostCode = value;
                OnPropertyChanged("SelectedPostCode");
                //if (SelectedCity != null)
                //{
                //    CitiesList.ToList().Where(p => p.key == SelectedCity.key).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                //    CitiesList.ToList().Where(p => p.key != SelectedCity.key).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                //}
                try
                {
                    if (SelectedPostCode != null)
                    {
                        if (!SelectedPostList.Contains(SelectedPostCode))
                        {
                            var obj = _fBCities.Where(c=>c.key==SelectedPostCode.primary_city_id.ToString()).FirstOrDefault();
                            if (obj == null)
                            {
                                PostalCodeList.ToList().Where(p => p.key == SelectedPostCode.key).ToList().ForEach(p => { p.Tick = "RightTickGreen"; });
                                SelectedPostList.Add(SelectedPostCode);
                                SelectedPostCode = null;
                            }
                            else
                            {
                                var popupnav = new Popups.UserDialogPopup("Failed", "Please select different postal code, it should not be same to selected location postal code.", "Ok");
                                PopupNavigation.Instance.PushAsync(popupnav);
                                SelectedPostCode = null;
                            }
                        }
                        else
                        {
                            PostalCodeList.ToList().Where(p => p.key == SelectedPostCode.key).ToList().ForEach(p => { p.Tick = "RightTickGray"; });
                            if (SelectedPostList.Contains(SelectedPostCode))
                                SelectedPostList.Remove(SelectedPostCode);
                            SelectedPostCode = null;
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



        private string _SearchBoxTitle = "Search Postcode";
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
        public async Task IsListForPostcodeMethod(bool ListForLoc)
        {
            await SearchPostCode();
        }


        public async Task SearchPostCode()
        {
            try
            {

                if (SearchText.Length >= 3)
                {
                    IsBusy = true;
                    FBCitiesRequestModel fbCitiesRequestModel = new FBCitiesRequestModel
                    {
                        // fb_access_token = Helper.facebookProfile.Token,
                        fb_access_token = Global.Helper.facebookProfile.Token,
                        search_text = SearchText,
                    };
                    try
                    {
                        string url = "user/get-facebook-post-codes";
                        Rest_ResponseModel rest_result = await WebService.WebService.PostData(fbCitiesRequestModel, url, true);
                        if (rest_result != null)
                        {
                            if (rest_result.status_code == 200)
                            {
                                var fbCitiesResponseModel = JsonConvert.DeserializeObject<FBPostCodeResponseModel>(rest_result.response_body);

                                if (fbCitiesResponseModel != null)
                                {
                                    if (fbCitiesResponseModel.status)
                                    {
                                        IsBusy = false;
                                        //SearchLocationList = new List<FBCitiesResult>(fbCitiesResponseModel.result); 
                                        PostalCodeList = new ObservableCollection<PostCodesResult>(fbCitiesResponseModel.result);

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


        


        private void Cancel()
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void Done()
        {
            MessagingCenter.Send<object, ObservableCollection<PostCodesResult>>(this, "SelectedCodes", SelectedPostList);
            PopupNavigation.Instance.PopAsync();
            MessagingCenter.Unsubscribe<object, ObservableCollection<PostCodesResult>>(this, "SelectedCodes");
        }

        #endregion
    }
}

