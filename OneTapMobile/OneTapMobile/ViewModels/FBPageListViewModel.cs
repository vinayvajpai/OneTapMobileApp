using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Services;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
namespace OneTapMobile.ViewModels
{
    public class FBPageListViewModel : BaseViewModel
    {
        #region Constructor
        public FBPageListViewModel()
        {
            _FBPageListService = DependencyService.Get<IFBPageListService>();
        }
        #endregion

        #region properties
        public INavigation nav;
        private readonly IFBPageListService _FBPageListService;
        private ObservableCollection<FBResult> _PageList = new ObservableCollection<FBResult>();
        public ObservableCollection<FBResult> PageList
        {
            get
            {
                return _PageList;
            }
            set
            {
                _PageList = value;
                OnPropertyChanged("PageList");
            }
        }

        private FBResult _SelectedPage;
        public FBResult SelectedPage
        {
            get
            {
                return _SelectedPage;
            }
            set
            {
                _SelectedPage = value;
                OnPropertyChanged("SelectedPage");
                if (SelectedPage != null)
                {
                    PageList.ToList().Where(p => p.page_id == SelectedPage.page_id).ToList().ForEach(p => { p.NotChecked = false; p.PageChecked = true; p.SelectionColor = Color.FromHex("#AC47ED"); });
                    PageList.ToList().Where(p => p.page_id != SelectedPage.page_id).ToList().ForEach(p => { p.NotChecked = true; p.PageChecked = false; p.SelectionColor = Color.Transparent; });
                    IsEnabledBtn = true;
                    OpacityBtn = 1;

                }
                else
                {
                    IsEnabledBtn = false;
                    OpacityBtn = 0.5;
                }
                CheckIsSelectedCat();
            }
        }

        #endregion

        #region Commands

        private Command _NextBtnClicked;
        public Command NextBtnClicked
        {
            get { return _NextBtnClicked ?? (_NextBtnClicked = new Command(() => NextBtnMethod())); }
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

        #endregion

        #region back button pressed method
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

        #region next button pressed method
        private void NextBtnMethod()
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

                Helper.imageCampaign.facebook_page_id = Helper.videoCampaign.facebook_page_id = SelectedPage.page_id;
                Helper.imageCampaign.facebook_page_name = Helper.videoCampaign.facebook_page_name = SelectedPage.page_name;
                Helper.SavePropertyData("facebook_page_id", SelectedPage.page_id);
                Helper.SavePropertyData("facebook_page_name", SelectedPage.page_name);

                nav.PushAsync(new FBPageCategoryView());
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }

        #endregion

        #region get facebook pages method
        public async Task GetPageData()
        {
            
            if (!Conn)
            {
                return;
            }
            try
            {
                IsChecking = true;
                IsBusy = true;
                FacebookPagesModel FbPageAccessmodel = new FacebookPagesModel
                {
                    fb_access_token = Helper.facebookProfile.Token,
                    user_id = Constant.OneTapUserId
                };
                await _FBPageListService.FBPageList(nav, FbPageAccessmodel, async (res) =>
                {
                    if (res != null)
                    {
                        if (res.result != null)
                        {
                            PageList = new ObservableCollection<FBResult>(res.result);
                            if (PageList == null || PageList.Count == 0)
                            {
                                IsshowEmpty = true;
                            }
                            else
                            {
                                IsshowEmpty = false;
                            }
                        }
                        else
                        {
                            IsshowEmpty = true;
                        }
                    }
                    else
                    {
                        IsshowEmpty = false;
                    }
                    IsBusy = false;
                });
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsshowEmpty = true;
                IsBusy = false;

            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region check selected page method
        void CheckIsSelectedCat()
        {
            var objCat = PageList.Where(c => c.PageChecked == true).ToList();
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
