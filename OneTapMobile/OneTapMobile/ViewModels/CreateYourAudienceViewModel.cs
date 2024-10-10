using Newtonsoft.Json;
using OneTapMobile.CustomControl;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using OneTapMobile.Views.ErrorPage;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class CreateYourAudienceViewModel : BaseViewModel
    {
        #region Constructor

        PopulateListView populateListView = new PopulateListView(true);
        public CreateYourAudienceViewModel()
        {
            DropDownAgeList();
        }
        public CreateYourAudienceViewModel(FlexLayout flexLayout)
        {
            var Flex = flexLayout;

        }

        #endregion

        #region Properties

        public INavigation nav;

        public string targetlocTxt;
        public string TargetlocTxt
        {
            get
            {
                return targetlocTxt;
            }
            set
            {
                targetlocTxt = value;
                OnPropertyChanged("TargetlocTxt");
            }
        }

        public bool _searchedItemList = false;
        public bool SearchedItemList
        {
            get
            {
                return _searchedItemList;
            }
            set
            {
                _searchedItemList = value;
                OnPropertyChanged("SearchedItemList");
            }
        }

        public string demoInterestsTxt;
        public string DemoInterestsTxt
        {
            get
            {
                return demoInterestsTxt;
            }
            set
            {
                demoInterestsTxt = value;
                OnPropertyChanged("DemoInterestsTxt");
            }
        }

        public string postCodeTxt;
        public string PostCodeTxt
        {
            get
            {
                return postCodeTxt;
            }
            set
            {
                postCodeTxt = value;
                OnPropertyChanged("PostCodeTxt");
            }
        }

        //private List<FBCitiesResult> searchLocationList = new List<FBCitiesResult>();
        //public List<FBCitiesResult> SearchLocationList
        //{
        //    get => searchLocationList; set => SetProperty(ref searchLocationList, value);
        //}

        private ObservableCollection<FBCitiesResult> _SearchLocationList = new ObservableCollection<FBCitiesResult>();
        public ObservableCollection<FBCitiesResult> SearchLocationList
        {
            get { return _SearchLocationList; }
            set { _SearchLocationList = value; OnPropertyChanged("SearchLocationList"); }
        }


        private List<string> _MinAgeRangeitems = new List<string>();
        public List<string> MinAgeRangeitems
        {
            get => _MinAgeRangeitems; set => SetProperty(ref _MinAgeRangeitems, value);
        }
         
        
        private List<string> _MaxAgeRangeitems = new List<string>();
        public List<string> MaxAgeRangeitems
        {
            get => _MaxAgeRangeitems; set => SetProperty(ref _MaxAgeRangeitems, value);
        }


        private ObservableCollection<FBInterestResult> demoInterestsList = new ObservableCollection<FBInterestResult>();
        public ObservableCollection<FBInterestResult> DemoInterestsList
        {
            get
            {
                return demoInterestsList;
            }
            set
            {
                demoInterestsList = value;
                OnPropertyChanged("DemoInterestsList");
            }
        }

        private ObservableCollection<PostCodesResult> _PostCodesResultList = new ObservableCollection<PostCodesResult>();
        public ObservableCollection<PostCodesResult> PostCodesResultList
        {
            get
            {
                return _PostCodesResultList;
            }
            set
            {
                _PostCodesResultList = value;
                OnPropertyChanged("PostCodesResultList");
            }
        }

        private ObservableCollection<FBCitiesResult> targetLocList = new ObservableCollection<FBCitiesResult>();
        public ObservableCollection<FBCitiesResult> TargetLocList
        {
            get
            {
                return targetLocList;
            }
            set
            {
                targetLocList = value;
                OnPropertyChanged("TargetLocList");
            }
        }

        private ObservableCollection<PostCodesResult> targetPostList = new ObservableCollection<PostCodesResult>();
        public ObservableCollection<PostCodesResult> TargetPostList
        {
            get
            {
                return targetPostList;
            }
            set
            {
                targetPostList = value;
                OnPropertyChanged("TargetPostList");
            }
        }

        private FBInterestResult _FBInterestResultChanged;
        public FBInterestResult FBInterestResultChanged { get => _FBInterestResultChanged; set => SetProperty(ref _FBInterestResultChanged, value); }
        
        
        private FBCitiesResult _FBCitiesResultChanged;
        public FBCitiesResult FBCitiesResultChanged { get => _FBCitiesResultChanged; set => SetProperty(ref _FBCitiesResultChanged, value); }

        private PostCodesResult _PostCodeResultChanged;
        public PostCodesResult PostCodeResultChanged { get => _PostCodeResultChanged; set => SetProperty(ref _PostCodeResultChanged, value); }


        private FBCitiesResult _SelectedLocation;
        public FBCitiesResult SelectedLocation { get => _SelectedLocation; set => SetProperty(ref _SelectedLocation, value); }


        private string selectedMinAgeRange;
        public string SelectedMinAgeRange { get => selectedMinAgeRange; set => SetProperty(ref selectedMinAgeRange, value); }
        
        private string selectedMaxAgeRange;
        public string SelectedMaxAgeRange { get => selectedMaxAgeRange; set => SetProperty(ref selectedMaxAgeRange, value); }


        private string selectedGender = null;
        public string SelectedGender { get => selectedGender; set => SetProperty(ref selectedGender, value); }

        #endregion

        #region Commands


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
                    helpCommand = new Command(HelpCmd);
                }

                return helpCommand;
            }
        }


        //private Command _CityTextEntered;
        //public ICommand CityTextEntered
        //{
        //    get
        //    {
        //        if (_CityTextEntered == null)
        //        {
        //            _CityTextEntered = new Command(CityTextEnteredCmd);
        //        }

        //        return _CityTextEntered;
        //    }
        //}

        private Command saveAudienceCommand;
        public ICommand SaveAudienceCommand
        {
            get
            {
                if (saveAudienceCommand == null)
                {
                    saveAudienceCommand = new Command(SaveAudience);
                }

                return saveAudienceCommand;
            }
        }

        #endregion

        #region method
        public void DropDownAgeList()
        {
            MinAgeRangeitems = new List<string>();
            //ageRangeitems.Add("Select Demographic age Range");

            for(int i = 18; i<= 65;i++)
            {
                _MinAgeRangeitems.Add(i.ToString());
                _MaxAgeRangeitems.Add(i.ToString());
            }

            MinAgeRangeitems = _MinAgeRangeitems;
            MaxAgeRangeitems = _MaxAgeRangeitems;
        }

        private void SaveAudience()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                // added tagviewmodel "name" to a temp list.

                if (TargetLocList != null)
                {
                    List<FBCitiesResult> TargetLocations = new List<FBCitiesResult>();
                    foreach (var item in TargetLocList)
                    {
                        TargetLocations.Add(item);
                    }
                    Helper.imageCampaign.TargetLocation = Helper.videoCampaign.TargetLocation = TargetLocations;
                }

                if (TargetPostList != null)
                {
                    List<PostCodesResult> TargetPostcode = new List<PostCodesResult>();
                    foreach (var item in TargetPostList)
                    {
                        TargetPostcode.Add(item);
                    }
                    Helper.imageCampaign.postcodes = Helper.videoCampaign.postcodes = TargetPostcode;
                }

                if (demoInterestsList != null)
                {
                    List<FBInterestResult> demoIntList = new List<FBInterestResult>();
                    foreach (var item in demoInterestsList)
                    {
                        demoIntList.Add(item);
                    }
                    Helper.imageCampaign.DemoInterest = Helper.videoCampaign.DemoInterest = demoIntList;
                }

                SetAge(Convert.ToInt32(SelectedMinAgeRange), Convert.ToInt32(SelectedMaxAgeRange));

                Helper.imageCampaign.Gender = Helper.videoCampaign.Gender = SelectedGender;

                if (!string.IsNullOrWhiteSpace(SelectedGender) && Helper.imageCampaign.ButtonTitle != "Select Demographic age Range" && (Helper.imageCampaign.TargetLocation.Count >= 1||Helper.imageCampaign.postcodes.Count>=1) && Helper.imageCampaign.DemoInterest.Count >= 3)
                {
                    nav.PushAsync(new AdReviewView());
                }
                else
                {
                    if (DemoInterestsList.Count < 3)
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, "please Add minimum 3 Tags in demographic interests or behaviours", "Ok");
                        PopupNavigation.Instance.PushAsync(popupnav);
                        IsTap = false;
                        return;
                    }
                    //else if (TargetLocList.Count == 0)
                    //{
                    //    popupnav = new UserDialogPopup(Constant.PopupTitle, "please Add Target locations", "Ok");
                    //    PopupNavigation.Instance.PushAsync(popupnav);
                    //    IsTap = false;
                    //    return;
                    //}
                    //popupnav = new UserDialogPopup(Constant.PopupTitle, "please Select Button Title!", "Ok");
                    //PopupNavigation.Instance.PushAsync(popupnav);
                    IsTap = false;
                    //return;
                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

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


        private void HelpCmd()
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

        public void SetAge(int IndexOfMinAgeList , int IndexOfMaxAgeList)
        {

            // Adding 18 for getting value from index and drop down list is starting from 18.

            Helper.imageCampaign.StartAgeRange = Helper.videoCampaign.StartAgeRange =Convert.ToInt16(IndexOfMinAgeList) + 18;
            Helper.imageCampaign.EndAgeRange = Helper.videoCampaign.EndAgeRange =Convert.ToInt16(IndexOfMaxAgeList) + 18;

            #region old drop down selection

            //if (IndexOfList == 0)
            //{
            //    Helper.imageCampaign.StartAgeRange = Helper.videoCampaign.StartAgeRange = 18;
            //    Helper.imageCampaign.EndAgeRange = Helper.videoCampaign.EndAgeRange = 24;
            //}
            //else if (IndexOfList == 1)
            //{
            //    Helper.imageCampaign.StartAgeRange = Helper.videoCampaign.StartAgeRange = 24;
            //    Helper.imageCampaign.EndAgeRange = Helper.videoCampaign.EndAgeRange = 30;
            //}
            //else if (IndexOfList == 2)
            //{
            //    Helper.imageCampaign.StartAgeRange = Helper.videoCampaign.StartAgeRange = 30;
            //    Helper.imageCampaign.EndAgeRange = Helper.videoCampaign.EndAgeRange = 36;
            //}
            //else if (IndexOfList == 3)
            //{
            //    Helper.imageCampaign.StartAgeRange = Helper.videoCampaign.StartAgeRange = 36;
            //    Helper.imageCampaign.EndAgeRange = Helper.videoCampaign.EndAgeRange = 42;
            //}
            //else if (IndexOfList == 4)
            //{
            //    Helper.imageCampaign.StartAgeRange = Helper.videoCampaign.StartAgeRange = 42;
            //    Helper.imageCampaign.EndAgeRange = Helper.videoCampaign.EndAgeRange = 48;
            //}
            //else if (IndexOfList == 5)
            //{
            //    Helper.imageCampaign.StartAgeRange = Helper.videoCampaign.StartAgeRange = 48;
            //    Helper.imageCampaign.EndAgeRange = Helper.videoCampaign.EndAgeRange = 54;
            //}
            //else
            //{
            //    Helper.imageCampaign.StartAgeRange = Helper.videoCampaign.StartAgeRange = 18;
            //    Helper.imageCampaign.EndAgeRange = Helper.videoCampaign.EndAgeRange = 24;
            //}
            #endregion
        }
        #endregion
    }
    #region local tag model
    public class TagTitleModel
    {
        public string DisplayName { get; set; }
        public string TagIndex { get; set; }
    }

    #endregion
}

