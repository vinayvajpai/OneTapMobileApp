using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Services;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class FBPageCategoryViewModel : BaseViewModel
    {
        #region constructor
        public FBPageCategoryViewModel()
        {
            _FBPageCategoryService = DependencyService.Get<IFBPageCategoryService>();
            _FBPageUpdateCategoryService = DependencyService.Get<IFBPageCategoryService>();
        }
        #endregion

        #region properties
        public INavigation nav;
        private readonly IFBPageCategoryService _FBPageCategoryService;
        private readonly IFBPageCategoryService _FBPageUpdateCategoryService;
        private ObservableCollection<AllCategory> _CategoryList = new ObservableCollection<AllCategory>();
        public ObservableCollection<AllCategory> CategoryList
        {
            get { return _CategoryList; }
            set { _CategoryList = value; OnPropertyChanged("CategoryList"); }
        }

        private string _OtherCategory = string.Empty;
        public string OtherCategory
        {
            get { return _OtherCategory; }
            set { _OtherCategory = value; OnPropertyChanged("OtherCategory"); }
        }

        private bool _IsOtherCategory = false;
        public bool IsOtherCategory
        {
            get { return _IsOtherCategory; }
            set
            {
                _IsOtherCategory = value; OnPropertyChanged("" + "IsOtherCategory");
            }
        }


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

        private ICommand nextCommand;
        public ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                {
                    nextCommand = new Command(NextCommandExecute);
                }

                return nextCommand;
            }
        }

        private AllCategory _SelectedCat;
        public AllCategory SelectedCat
        {
            get
            {
                return _SelectedCat;
            }
            set
            {

                _SelectedCat = value;
                OnPropertyChanged("SelectedCat");
                if (SelectedCat != null)
                {
                    CategoryList.ToList().Where(p => p.id == SelectedCat.id).ToList().ForEach(p => { p.NotChecked = false; p.PageChecked = true; p.SelectionColor = Color.FromHex("#AC47ED"); });
                    CategoryList.ToList().Where(p => p.id != SelectedCat.id).ToList().ForEach(p => { p.NotChecked = true; p.PageChecked = false; p.SelectionColor = Color.Transparent; });

                    if (SelectedCat.id == "0")
                    {
                        if (SelectedCat.PageChecked)
                        {
                            var popupdefault = new AddCategoryPopup("Add Category", "", "Ok", "Cancel");
                            popupdefault.eventOK += Popupdefault_eventOK;
                            popupdefault.eventCancel += Popupdefault_eventCancel;
                            PopupNavigation.Instance.PushAsync(popupdefault);
                            return;
                        }
                        else
                        {
                            OtherCategory = string.Empty;
                            IsOtherCategory = false;
                        }
                    }

                    SelectedCat = null;
                }
                CheckIsSelectedCat();

            }
        }

        #endregion

        #region PopUp Cancel Button Pressed Method 
        private void Popupdefault_eventCancel(object sender, EventArgs e)
        {
            OtherCategory = string.Empty;
            IsOtherCategory = false;
            if (SelectedCat != null)
            {
                SelectedCat.NotChecked = !SelectedCat.NotChecked;
                SelectedCat.PageChecked = !SelectedCat.PageChecked;
                SelectedCat.SelectionColor = SelectedCat.PageChecked ? Color.FromHex("#AC47ED") : Color.Transparent;
                CheckIsSelectedCat();
                SelectedCat = null;
            }
        }
        #endregion

        #region PopUp Ok Button Pressed Method
        private void Popupdefault_eventOK(object sender, string e)
        {
            OtherCategory = e;
            IsOtherCategory = true;
            if (SelectedCat != null)
            {


                //_CategoryList.Where(c => c.id == "0").ToList().ForEach(c => c.name = "Other(" + OtherCategory + ")");

                //CategoryList = _CategoryList;                

                var newcatname = string.Empty;
                ObservableCollection<AllCategory> Tempdata = new ObservableCollection<AllCategory>();
                foreach (var cat in CategoryList)
                {
                    if (cat.id == "0")
                    {
                        newcatname = "Other (" + OtherCategory + ")";
                    }
                    else
                    {
                        newcatname = cat.name;
                    }

                    Tempdata.Add(new AllCategory
                    {
                        id = cat.id,
                        name = newcatname,
                        NotChecked = cat.NotChecked,
                        PageChecked = cat.PageChecked,
                        SelectionColor = cat.SelectionColor,

                    });
                }

                if (CategoryList != null)
                {
                    CategoryList.Clear();
                    CategoryList = Tempdata;
                }

                   CheckIsSelectedCat();
                SelectedCat = null;                
            }
        }
        #endregion

        #region Back Button Method
        private void PerformBackBtn()
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

        #region Next Button Pressed Method
        private async void NextCommandExecute()
        {
            if (!Conn)
            {
                return;
            }
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                if (IsOtherCategory && string.IsNullOrEmpty(OtherCategory))
                {
                    IsTap = false;
                    popupnav = new UserDialogPopup(Constant.PopupTitle, "Please enter other category.", "Ok");
                    await PopupNavigation.Instance.PushAsync(popupnav);
                    return;
                }
                else
                {
                    IsBusy = true;
                    var selectCat = CategoryList.Where(c => c.PageChecked == true).ToList().Select(c => c.id).ToList();
                    SubmitCatRequestModel submitCatRequestModel = new SubmitCatRequestModel
                    {
                        user_id = Convert.ToString(Constant.OneTapUserId),
                        selected_categories = new List<string>(selectCat),
                        is_other = IsOtherCategory,
                        new_category = OtherCategory
                    };
                    await _FBPageUpdateCategoryService.FBPageUpdateCategoryList(nav, submitCatRequestModel, async (res) =>
                    {
                        if (res)
                        {
                            if (!DependencyService.Get<IHandleNotification>().registeredForNotifications())
                            {
                                await nav.PushAsync(new EnableNotificationView());
                            }
                            else
                            {
                                await nav.PushAsync(new CongratsView());
                                IsBusy = false;
                                IsTap = false;
                            }
                        }
                        else
                        {
                            popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                            await PopupNavigation.Instance.PushAsync(popupnav);
                            IsBusy = false;
                            IsTap = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                IsBusy = false;
                IsChecking = false;
                Debug.WriteLine(ex.Message);

            }
            finally
            {
                IsBusy = false;
                IsChecking = false;
            }

        }
        #endregion

        #region Get Category Data Method
        public async Task GetCatData()
        {
            if (!Conn)
            {
                return;
            }
            IsChecking = true;
            try
            {
                IsBusy = true;
                FacebookPagesModel FbPageAccessmodel = new FacebookPagesModel
                {
                    user_id = Constant.OneTapUserId
                };
                await _FBPageCategoryService.FBPageCategoryList(nav, FbPageAccessmodel, async (res) =>
                {
                    if (res != null)
                    {
                        CategoryList = new ObservableCollection<AllCategory>(res.result.all_categories);
                        if (CategoryList == null || CategoryList.Count == 0)
                        {
                            IsshowEmpty = true;
                        }
                        else
                        {
                            IsshowEmpty = false;
                            foreach (var selcat in res.result.user_selected_categories)
                            {
                                var catObj = CategoryList.Where(c => c.id == selcat).FirstOrDefault();
                                if (catObj != null)
                                {
                                    catObj.NotChecked = false;
                                    catObj.PageChecked = true;
                                    catObj.SelectionColor = Color.FromHex("#AC47ED");
                                }
                            }
                            CategoryList.Add(new AllCategory()
                            {
                                id = "0",
                                name = "Other",
                            });

                            CheckIsSelectedCat();
                        }
                    }
                    else
                    {
                        popupnav = new UserDialogPopup(Constant.PopupTitle, Constant.PopupMessage, "Ok");
                        await PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                        IsTap = false;
                    }
                });
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsshowEmpty = true;
                IsTap = false;
                IsBusy = false;
                IsChecking = false;

            }
            finally
            {
                IsBusy = false;
                IsChecking = false;
            }
        }
        #endregion

        #region Check Selected Category Method 
        void CheckIsSelectedCat()
        {
            var objCat = CategoryList.Where(c => c.PageChecked == true).ToList();
            if (objCat.Count > 0)
            {
                IsEnabledBtn = true;
                OpacityBtn = 1;
            }
            else
            {
                IsEnabledBtn = false;
                OpacityBtn = 0.5;
            }
        }
        #endregion
    }
}

