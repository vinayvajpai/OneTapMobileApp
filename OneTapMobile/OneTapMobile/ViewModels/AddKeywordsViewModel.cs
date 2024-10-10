using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Views;
using OneTapMobile.Views.ErrorPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
namespace OneTapMobile.ViewModels
{
    public class AddKeywordsViewModel : BaseViewModel
    {

        #region Properties

        public INavigation nav;
        public class TagsNameModel
        {
            public int TagIndex { get; set; }
            public string DisplayName { get; set; }

        }
        private ObservableCollection<TagsNameModel> keywordThemeList = new ObservableCollection<TagsNameModel>();
        public ObservableCollection<TagsNameModel> KeywordThemeList
        {
            get
            {
                return keywordThemeList;
            }
            set
            {
                keywordThemeList = value;
                OnPropertyChanged("KeywordThemeList");
            }
        }
        private ObservableCollection<GoAdsTarLocResult> targetLocList = new ObservableCollection<GoAdsTarLocResult>();
        public ObservableCollection<GoAdsTarLocResult> TargetLocList
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

        private TagsNameModel _KeywordThemeChanged;
        public TagsNameModel KeywordThemeChanged { get => _KeywordThemeChanged; set => SetProperty(ref _KeywordThemeChanged, value); }


        private GoAdsTarLocResult _TargetLocChanged;
        public GoAdsTarLocResult TargetLocChanged { get => _TargetLocChanged; set => SetProperty(ref _TargetLocChanged, value); }

        private string keywordsthemesTxt;
        public string KeywordsthemesTxt
        {
            get
            {
                return keywordsthemesTxt;
            }
            set
            {
                keywordsthemesTxt = value;
                OnPropertyChanged("keywordsthemesTxt");
            }
        }
        private string targetLocTxt;
        public string TargetLocTxt
        {
            get
            {
                return targetLocTxt;
            }
            set
            {
                targetLocTxt = value;
                OnPropertyChanged("TargetLocTxt");
            }
        }

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
        private Command continueBtnCommand;
        public ICommand ContinueBtnCommand
        {
            get
            {
                if (continueBtnCommand == null)
                {
                    continueBtnCommand = new Command(ContinueBtn);
                }
                return continueBtnCommand;
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

        #endregion

        #region Methods
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
            catch (System.Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }
        private void ContinueBtn()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                List<string> KeywordsDataList = new List<string>();
                foreach (var item in KeywordThemeList)
                {
                    if (item != null)
                    {
                        KeywordsDataList.Add(item.DisplayName.ToString());
                    }
                }

                List<GoAdsTarLocResult> TargetLocationList = new List<GoAdsTarLocResult>();
                foreach (var item in targetLocList)
                {
                    if (item != null)
                    {
                        TargetLocationList.Add(new GoAdsTarLocResult { id = item.id, name = item.name });
                    }
                }

                Helper.keywordCampaign.KeywordTheme = KeywordsDataList;
                Helper.keywordCampaign.TargetLocation = TargetLocationList;
                nav.PushAsync(new GoogleAdReviewView());
            }
            catch (System.Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
