using OneTapMobile.CustomControl;
using OneTapMobile.ViewModels;
using OneTapMobile.ViewModels.AI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static OneTapMobile.ViewModels.AddKeywordsViewModel;

namespace OneTapMobile.Views.AI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TapAndFillView : ContentPage
    {

        TapAndFillViewModel m_viewmodel;

        public TapAndFillView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new TapAndFillViewModel();
            if (m_viewmodel != null)
            {
                m_viewmodel.nav = this.Navigation;

                m_viewmodel.PropertyChanged += M_viewmodel_PropertyChanged;
            }
        }



        private void CompanyGoodAtFlex_ChildRemoved(object sender, ElementEventArgs e)
        {
            try
            {
                var a = (TagViewControl)e.Element;
                var ToBeDeleted = m_viewmodel.CompanyGoodAtList.Where(x => x.TagIndex == a.TagIndex).FirstOrDefault();
                m_viewmodel.CompanyGoodAtList.Remove(ToBeDeleted);
                //m_viewmodel.CompanyGoodAtList = ToBeDeleted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void WhoToReachFlex_ChildRemoved(object sender, ElementEventArgs e)
        {
            try
            {
                var a = (TagViewControl)e.Element;
                var ToBeDeleted = m_viewmodel.WhoToReachList.Where(x => x.TagIndex == a.TagIndex).FirstOrDefault();
                m_viewmodel.WhoToReachList.Remove(ToBeDeleted);
                // m_viewmodel.TargetLocChanged = ToBeDeleted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CompanyNameFlex_ChildRemoved(object sender, ElementEventArgs e)
        {
            try
            {
                var a = (TagViewControl)e.Element;
                var ToBeDeleted = m_viewmodel.CompanyNameList.Where(x => x.TagIndex == a.TagIndex).FirstOrDefault();
                m_viewmodel.CompanyNameList.Remove(ToBeDeleted);
                //  m_viewmodel.TargetLocChanged = ToBeDeleted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CompanyGoodAtFlext_Completed(object sender, EventArgs e)
        {
            try
            {
                var flag = sender as Entry;
                if (flag.Text != null)
                {
                    if (CompanyGoodAtFlex.Children.Count > 10)
                        return;
                    else
                    {
                        m_viewmodel.CompanyGoodAtList.Add(new TagsNameModel
                        {
                            DisplayName = flag.Text,
                        });

                        CompanyGoodAtFlex.Children.Add(new CustomControl.TagViewControl() { TagTitle = flag.Text });
                        CompanyGoodAtTxt.Text = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void WhoToReachTxt_Completed(object sender, EventArgs e)
        {
            try
            {
                var flag = sender as Entry;
                if (flag.Text != null)
                {
                    if (WhoToReachFlex.Children.Count > 10)
                        return;
                    else
                    {
                        m_viewmodel.WhoToReachList.Add(new TagsNameModel
                        {
                            DisplayName = flag.Text,
                        });

                        WhoToReachFlex.Children.Add(new CustomControl.TagViewControl() { TagTitle = flag.Text });
                        WhoToReachTxt.Text = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CompanyNameTxt_Completed(object sender, EventArgs e)
        {
            try
            {
                var flag = sender as Entry;
                if (flag.Text != null)
                {
                    if (CompanyNameFlex.Children.Count > 10)
                        return;
                    else
                    {
                        m_viewmodel.CompanyNameList.Add(new TagsNameModel
                        {
                            DisplayName = flag.Text,
                        });

                        CompanyNameFlex.Children.Add(new CustomControl.TagViewControl() { TagTitle = flag.Text });
                        CompanyNameTxt.Text = string.Empty;
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
                ////Company Name
                //if (m_viewmodel.CompanyNameList != null)
                //{
                //    if (m_viewmodel.CompanyNameList.Count > 0)
                //        CompanyNameFlex.IsVisible = true;
                //    else
                //        CompanyNameFlex.IsVisible = false;
                //}

                ////WhoTo Reach
                //else if (m_viewmodel.WhoToReachList != null)
                //{
                //    if (m_viewmodel.WhoToReachList.Count > 0)
                //        WhoToReachFlex.IsVisible = true;
                //    else
                //        WhoToReachFlex.IsVisible = false;
                //}

                ////Company Good At
                //else if (m_viewmodel.CompanyGoodAtList != null)
                //{
                //    if (m_viewmodel.CompanyGoodAtList.Count > 0)
                //        CompanyGoodAtFlex.IsVisible = true;
                //    else
                //        CompanyGoodAtFlex.IsVisible = false;
                //}

                //button
                if (m_viewmodel.CompanyNameList != null && m_viewmodel.WhoToReachList != null && m_viewmodel.CompanyGoodAtList != null)
                {
                    if (m_viewmodel.CompanyNameList.Count > 0 && m_viewmodel.WhoToReachList.Count > 0 && m_viewmodel.CompanyGoodAtList.Count > 0)
                    {
                        ContinueFrame.BackgroundColor = Color.FromHex("#AC47ED");
                        ContinueFrame.Opacity = 1.0;
                        ContinueFrame.IsEnabled = true;
                        GenerateAdTxt.Opacity = 1.0;
                    }
                    else
                    {
                        ContinueFrame.BackgroundColor = Color.FromHex("#BE7CE9");
                        ContinueFrame.Opacity = 0.8;
                        ContinueFrame.IsEnabled = false;
                        GenerateAdTxt.Opacity = 0.5;
                    }
                }
                else
                {
                    ContinueFrame.BackgroundColor = Color.FromHex("#BE7CE9");
                    ContinueFrame.Opacity = 0.8;
                    ContinueFrame.IsEnabled = false;
                    GenerateAdTxt.Opacity = 0.5;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void RegenerateAd_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (m_viewmodel.CompanyNameList != null || m_viewmodel.WhoToReachList != null || m_viewmodel.CompanyGoodAtList != null)
                {
                    if (m_viewmodel.CompanyNameList.Count > 0)
                    {
                       var count=  CompanyNameFlex.Children.AsEnumerable().Count();
                        for (int i = 0; i < count; i++)
                        {
                            if(CompanyNameFlex.Children.Count>0)
                            CompanyNameFlex.Children.Remove(CompanyNameFlex.Children.AsEnumerable().ElementAt(0));
                        }
                        
                    }
                    if (m_viewmodel.WhoToReachList.Count > 0)
                    {
                        var count = WhoToReachFlex.Children.AsEnumerable().Count();
                        for (int i = 0; i < count; i++)
                        {
                            if (WhoToReachFlex.Children.Count > 0)
                                WhoToReachFlex.Children.Remove(WhoToReachFlex.Children.AsEnumerable().ElementAt(0));
                        }
                    }
                    if (m_viewmodel.CompanyGoodAtList.Count > 0)
                    {
                        var count = CompanyGoodAtFlex.Children.AsEnumerable().Count();
                        for (int i = 0; i < count; i++)
                        {
                            if (CompanyGoodAtFlex.Children.Count > 0)
                                CompanyGoodAtFlex.Children.Remove(CompanyGoodAtFlex.Children.AsEnumerable().ElementAt(0));
                        }
                    }
                   
                    // for disable button
                    ContinueFrame.BackgroundColor = Color.FromHex("#BE7CE9");
                    ContinueFrame.Opacity = 0.8;
                    ContinueFrame.IsEnabled = false;
                    GenerateAdTxt.Opacity = 0.5;

                }

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}