using OneTapMobile.CustomControl;
using OneTapMobile.Global;
using OneTapMobile.LocalDataBase;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static OneTapMobile.ViewModels.AddKeywordsViewModel;
namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddKeywordsView : ContentPage
    {
        AddKeywordsViewModel m_viewmodel;
        InitDatabaseTable db = new InitDatabaseTable();

        GoAdsTarLocResult flag;
        public AddKeywordsView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new AddKeywordsViewModel();

            if (m_viewmodel != null)
            {
                //if (Helper.keywordCampaign.TargetLocation != null || Helper.keywordCampaign.KeywordTheme != null)
                //{
                //    AddPreviousSelectedData();
                //}
                //else
                //{
                //    AddExampleLocIntTag();
                //}
                AddExampleLocIntTag();
            }
            MessagingCenter.Subscribe<object, ObservableCollection<GoAdsTarLocResult>>(this, "ChoosedLocations", (sender, arg) =>
            {
                if (arg != null)
                {
                    foreach (var item in arg)
                    {
                        flag = item;
                        TargetLocEntered();
                    }
                }
            });

            m_viewmodel.PropertyChanged += M_viewmodel_PropertyChanged;
        }

        private void AddExampleLocIntTag()
        {
            if (m_viewmodel.KeywordThemeList.Count == 0)
            {
                ThemeflexLayout.Children.Add(new CustomControl.TagViewControl() { TagTitle = "Summer Sale" });

                m_viewmodel.KeywordThemeList.Add(new TagsNameModel
                {
                    DisplayName = "Summer Sale"
                });
            }

            if (m_viewmodel.TargetLocList.Count == 0)
            {
                //LocflexLayout.Children.Add(new CustomControl.TagViewControl() { TagTitle = "Sydney", TagIndex = 1000286 });

                //m_viewmodel.TargetLocList.Add(new GoAdsTarLocResult
                //{
                //    name = "Sydney,New South Wales,Australia",
                //    id = 1000286
                //});
            }
        }

        private void AddPreviousSelectedData()
        {
            try
            {

                if (m_viewmodel.KeywordThemeList.Count == 0)
                {
                    foreach (var element in Helper.keywordCampaign.KeywordTheme)
                    {
                        ThemeflexLayout.Children.Add(new CustomControl.TagViewControl() { TagTitle = element });

                        m_viewmodel.KeywordThemeList.Add(new TagsNameModel
                        {
                            DisplayName = element
                        });
                    }
                }

                if (m_viewmodel.TargetLocList.Count == 0)
                {
                    foreach (var element in Helper.keywordCampaign.TargetLocation)
                    {
                        LocflexLayout.Children.Add(new CustomControl.TagViewControl() { TagTitle = element.name, TagIndex = element.id });

                        m_viewmodel.TargetLocList.Add(new GoAdsTarLocResult
                        {
                            name = element.name,
                            id = element.id
                        });
                    }
                }
                if (m_viewmodel.KeywordThemeList != null && m_viewmodel.TargetLocList != null)
                {
                    if (m_viewmodel.KeywordThemeList.Count > 2 && m_viewmodel.KeywordThemeList.Count < 11 && m_viewmodel.TargetLocList.Count > 0)
                    {
                        ContinueFrame.BackgroundColor = Color.FromHex("#AC47ED");
                        ContinueFrame.Opacity = 1.0;
                        ContinueFrame.IsEnabled = true;
                        ContinueText.Opacity = 1.0;
                    }
                    else
                    {
                        ContinueFrame.BackgroundColor = Color.FromHex("#BE7CE9");
                        ContinueFrame.Opacity = 0.8;
                        ContinueFrame.IsEnabled = false;
                        ContinueText.Opacity = 0.5;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void M_viewmodel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (m_viewmodel.KeywordThemeList != null)
                {
                    if (m_viewmodel.KeywordThemeList.Count > 2 && m_viewmodel.KeywordThemeList.Count < 11 && m_viewmodel.TargetLocList.Count > 0)
                    {
                        ContinueFrame.BackgroundColor = Color.FromHex("#AC47ED");
                        ContinueFrame.Opacity = 1.0;
                        ContinueFrame.IsEnabled = true;
                        ContinueText.Opacity = 1.0;
                    }
                    else
                    {
                        ContinueFrame.BackgroundColor = Color.FromHex("#BE7CE9");
                        ContinueFrame.Opacity = 0.8;
                        ContinueFrame.IsEnabled = false;
                        ContinueText.Opacity = 0.5;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                IsBusy = false;
                if (m_viewmodel != null)
                {
                    m_viewmodel.nav = this.Navigation;
                    m_viewmodel.IsTap = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void KeywordThemeEntered(object sender, EventArgs e)
        {
            try
            {
                var flag = sender as Entry;
                if (flag.Text != null)
                {
                    if (ThemeflexLayout.Children.Count > 10)
                        return;
                    else
                    {
                        m_viewmodel.KeywordThemeList.Add(new TagsNameModel
                        {
                            DisplayName = flag.Text,
                        });

                        ThemeflexLayout.Children.Add(new CustomControl.TagViewControl() { TagTitle = flag.Text });
                        KeywordsthemesTxt.Text = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void TargetLocEntered()
        {
            try
            {
                if (flag != null)
                {
                    if (LocflexLayout.Children.Count > 10)
                        return;
                    else
                    {
                        m_viewmodel.TargetLocList.Add(new GoAdsTarLocResult
                        {
                            name = flag.name,
                            id = flag.id
                        });

                        LocflexLayout.Children.Add(new CustomControl.TagViewControl() { TagTitle = flag.name, TagIndex = flag.id });
                        m_viewmodel.TargetLocTxt = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void LocflexLayout_ChildRemoved(object sender, ElementEventArgs e)
        {
            try
            {
                var a = (TagViewControl)e.Element;
                var ToBeDeleted = m_viewmodel.TargetLocList.Where(x => x.id == a.TagIndex).FirstOrDefault();
                m_viewmodel.TargetLocList.Remove(ToBeDeleted);
                m_viewmodel.TargetLocChanged = ToBeDeleted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ThemeflexLayout_ChildRemoved(object sender, ElementEventArgs e)
        {
            try
            {
                var a = (TagViewControl)e.Element;
                var ToBeDeleted = m_viewmodel.KeywordThemeList.Where(x => x.TagIndex == a.TagIndex).FirstOrDefault();
                m_viewmodel.KeywordThemeList.Remove(ToBeDeleted);
                m_viewmodel.KeywordThemeChanged = ToBeDeleted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void TargerLocFocused(object sender, FocusEventArgs e)
        {
            var popupdefault = new GoogleAdsTarLocPopup();
            PopupNavigation.Instance.PushAsync(popupdefault);
            TargetLocTxt.Unfocus();
        }
    }
}