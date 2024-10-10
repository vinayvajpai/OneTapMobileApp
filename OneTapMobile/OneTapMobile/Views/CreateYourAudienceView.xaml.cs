using OneTapMobile.CustomControl;
using OneTapMobile.Global;
using OneTapMobile.LocalDataBase;
using OneTapMobile.LocalDataBases.DataBaseModel;
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

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateYourAudienceView : ContentPage
    {
        CreateYourAudienceViewModel m_viewmodel;
        FBCitiesResult flag1;
        FBInterestResult flag2;
        PostCodesResult flag3;

        InitDatabaseTable db = new InitDatabaseTable();
        public CreateYourAudienceView()
        {
            InitializeComponent();

            BindingContext = m_viewmodel = new CreateYourAudienceViewModel();
            OtherGender.TextColor = Color.White;
            FAllGender.BackgroundColor = Color.FromHex("#AC47ED");
            GenderMale.TextColor = Color.FromHex("#AC47ED");
            FGenderMale.BackgroundColor = Color.White;
            GenderFemale.TextColor = Color.FromHex("#AC47ED");
            FGenderFemale.BackgroundColor = Color.White;
            //Selected Gender
            m_viewmodel.SelectedGender = "A";

            m_viewmodel.PropertyChanged += M_viewmodel_PropertyChanged;

            if (m_viewmodel != null)
            {
                if (Helper.imageCampaign.TargetLocation != null || Helper.videoCampaign.TargetLocation != null)
                {
                    AddPreviousSelectedData();
                }
                else
                {
                    AddExampleLocIntTag();
                }
            }

            MessagingCenter.Subscribe<object, ObservableCollection<FBCitiesResult>>(this, "SelectedLocation", (sender, arg) =>
            {
                if (arg != null)
                {
                    foreach (var item in arg)
                    {
                        flag1 = item;
                        TargetlocTxt_Completed();
                    }
                }
            });

            MessagingCenter.Subscribe<object, ObservableCollection<FBInterestResult>>(this, "SelectedInterest", (sender, arg) =>
            {
                if (arg != null)
                {
                    foreach (var item in arg)
                    {
                        flag2 = item;
                        DemoInterestsTxt_Completed();
                    }
                }
            });

            MessagingCenter.Subscribe<object, ObservableCollection<PostCodesResult>>(this, "SelectedCodes", (sender, arg) =>
            {
                if (arg != null)
                {
                    foreach (var item in arg)
                    {
                        flag3 = item;
                        PostCodeTxt_Completed();
                    }
                }
            });
        }

        private void M_viewmodel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (m_viewmodel.DemoInterestsList != null)
                {
                    if (m_viewmodel.DemoInterestsList.Count > 2 && m_viewmodel.DemoInterestsList.Count < 11 && (m_viewmodel.TargetLocList.Count > 0 || m_viewmodel.TargetPostList.Count>0))
                    {
                        SaveAudienceFrame.BackgroundColor = Color.FromHex("#AC47ED");
                        SaveAudienceFrame.Opacity = 1.0;
                        SaveAudienceFrame.IsEnabled = true;
                        SaveAudienceText.Opacity = 1.0;
                    }
                    else
                    {
                        SaveAudienceFrame.BackgroundColor = Color.FromHex("#BE7CE9");
                        SaveAudienceFrame.Opacity = 0.8;
                        SaveAudienceFrame.IsEnabled = false;
                        SaveAudienceText.Opacity = 0.5;
                    }
                }
                if (m_viewmodel.TargetLocList.Count > 0)
                {
                    postCodeStk.IsEnabled = false;
                    postCodeStk.Opacity = 0.5;
                    tarLocStk.IsEnabled = true;
                    tarLocStk.Opacity = 1;
                }
                else if (m_viewmodel.TargetPostList.Count > 0)
                {
                    postCodeStk.IsEnabled = true;
                    postCodeStk.Opacity = 1;
                    tarLocStk.IsEnabled = false;
                    tarLocStk.Opacity = 0.5;
                }
                else
                {
                    postCodeStk.IsEnabled = true;
                    postCodeStk.Opacity = 1;
                    tarLocStk.IsEnabled = true;
                    tarLocStk.Opacity = 1;
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
                if (m_viewmodel != null)
                {
                    if (Helper.CreateCampType.ToLower() == "image")
                    {
                        MinAgedropdown.SelectedIndex = Helper.imageCampaign.StartAgeRange == 18 || Helper.imageCampaign.StartAgeRange == 0 ? 0 : Helper.imageCampaign.StartAgeRange - 18;
                        MaxAgeDropDown.SelectedIndex = Helper.imageCampaign.EndAgeRange == 18 || Helper.imageCampaign.EndAgeRange == 0 ? 0 : Helper.imageCampaign.EndAgeRange - 18;
                        MinDropDownIos.SelectedIndex = Helper.imageCampaign.StartAgeRange == 18 || Helper.imageCampaign.StartAgeRange == 0 ? 0 : Helper.imageCampaign.StartAgeRange - 18;
                        MaxDropDownIos.SelectedIndex = Helper.imageCampaign.EndAgeRange == 18 || Helper.imageCampaign.EndAgeRange == 0 ? 0 : Helper.imageCampaign.EndAgeRange - 18;
                    }
                    if(Helper.CreateCampType.ToLower()== "video")
                    {
                        MinAgedropdown.SelectedIndex = Helper.videoCampaign.StartAgeRange == 18 || Helper.videoCampaign.StartAgeRange == 0 ? 0 : Helper.videoCampaign.StartAgeRange - 18;
                        MaxAgeDropDown.SelectedIndex = Helper.videoCampaign.EndAgeRange == 18 || Helper.videoCampaign.EndAgeRange == 0 ? 0 : Helper.videoCampaign.EndAgeRange - 18;
                        MinDropDownIos.SelectedIndex = Helper.videoCampaign.StartAgeRange == 18 || Helper.videoCampaign.StartAgeRange == 0 ? 0 : Helper.videoCampaign.StartAgeRange - 18;
                        MaxDropDownIos.SelectedIndex = Helper.videoCampaign.EndAgeRange == 18 || Helper.videoCampaign.EndAgeRange == 0 ? 0 : Helper.videoCampaign.EndAgeRange - 18;
                    }

                    IsBusy = false;
                    m_viewmodel.IsTap = false;
                    m_viewmodel.nav = this.Navigation;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void DemoInterestsTxt_Completed()
        {
            try
            {
                if (flag2.name != null)
                {
                    // DemflexLayout.IsVisible = true;

                    if (DemflexLayout.Children.Count > 10)
                        return;
                    else
                    {
                        m_viewmodel.DemoInterestsList.Add(new FBInterestResult
                        {
                            audience_size = flag2.audience_size,
                            country = flag2.country,
                            country_access = flag2.country_access,
                            description = flag2.description,
                            id = flag2.id,
                            name = flag2.name,
                            partner = flag2.partner,
                            source = flag2.source,
                            topic = flag2.topic,
                            type = flag2.type
                        });
                        DemflexLayout.Children.Add(new TagViewControl() { TagTitle = flag2.name, Tagkey = flag2.id });
                        m_viewmodel.DemoInterestsTxt = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void PostCodeTxt_Completed()
        {
            try
            {
                if (flag3.name != null)
                {
                    // DemflexLayout.IsVisible = true;

                    if (PostflexLayout.Children.Count > 10)
                        return;
                    else
                    {
                        flag3.name = flag3.name.Trim();
                        m_viewmodel.TargetPostList.Add(flag3);
                        PostflexLayout.Children.Add(new TagViewControl() { TagTitle = flag3.name.Trim()+", "+flag3.primary_city+ ", "+flag3.primary_city+"", Tagkey = flag3.key.ToString() });
                        m_viewmodel.PostCodeTxt = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void TargetlocTxt_Completed()
        {
            try
            {
                //var flag = m_viewmodel.SelectedLocation.name;
                if (flag1 != null)
                {
                    LocflexLayout.IsVisible = true;

                    if (LocflexLayout.Children.Count > 5)
                        return;
                    else
                    {
                        m_viewmodel.TargetLocList.Add(new FBCitiesResult
                        {
                            country_code = flag1.country_code,
                            country_name = flag1.country_name,
                            key = flag1.key,
                            geo_hierarchy_level = flag1.geo_hierarchy_level,
                            geo_hierarchy_name = flag1.geo_hierarchy_name,
                            name = flag1.name,
                            region = flag1.region,
                            region_id = flag1.region_id,
                            supports_city = flag1.supports_city,
                            supports_region = flag1.supports_city,
                            type = flag1.type,
                        });
                        LocflexLayout.Children.Add(new TagViewControl() { TagTitle = flag1.name + ", " + flag1.region + ", " + flag1.country_name, Tagkey = flag1.key });
                        m_viewmodel.TargetlocTxt = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void Other_SelectedCmd(object sender, EventArgs e)
        {
            OtherGender.TextColor = Color.White;
            FAllGender.BackgroundColor = Color.FromHex("#AC47ED");
            GenderMale.TextColor = Color.FromHex("#AC47ED");
            FGenderMale.BackgroundColor = Color.White;
            GenderFemale.TextColor = Color.FromHex("#AC47ED");
            FGenderFemale.BackgroundColor = Color.White;
            // Selected Gender
            m_viewmodel.SelectedGender = "A";
        }

        private void Male_SelectedCmd(object sender, EventArgs e)
        {
            GenderMale.TextColor = Color.White;
            FGenderMale.BackgroundColor = Color.FromHex("#AC47ED");
            OtherGender.TextColor = Color.FromHex("#AC47ED");
            FAllGender.BackgroundColor = Color.White;
            GenderFemale.TextColor = Color.FromHex("#AC47ED");
            FGenderFemale.BackgroundColor = Color.White;
            // Selected Gender
            m_viewmodel.SelectedGender = "M";
        }

        private void Female_SelectedCmd(object sender, EventArgs e)
        {
            GenderFemale.TextColor = Color.White;
            FGenderFemale.BackgroundColor = Color.FromHex("#AC47ED");
            OtherGender.TextColor = Color.FromHex("#AC47ED");
            FAllGender.BackgroundColor = Color.White;
            GenderMale.TextColor = Color.FromHex("#AC47ED");
            FGenderMale.BackgroundColor = Color.White;
            // Selected Gender
            m_viewmodel.SelectedGender = "F";
        }

        private void DemflexLayout_ChildRemoved(object sender, ElementEventArgs e)
        {
            try
            {
                var a = ((TagViewControl)e.Element).Tagkey;
                var ToBeDeleted = m_viewmodel.DemoInterestsList.FirstOrDefault(x => x.id == a);
                m_viewmodel.DemoInterestsList.Remove(ToBeDeleted);
                m_viewmodel.FBInterestResultChanged = ToBeDeleted;
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
                var ToBeDeleted = m_viewmodel.TargetLocList.FirstOrDefault(x => x.key == a.Tagkey);
                m_viewmodel.TargetLocList.Remove(ToBeDeleted);
                m_viewmodel.FBCitiesResultChanged = ToBeDeleted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void TargetlocTxt_Focused(object sender, FocusEventArgs e)
        {
            var popupdefault = new PopulateListView(true);
            PopupNavigation.Instance.PushAsync(popupdefault);
            TargetlocTxt.Unfocus();
        }

        private void DemoInterestsTxt_Focused(object sender, FocusEventArgs e)
        {
            var popupdefault = new PopulateListView(false);
            PopupNavigation.Instance.PushAsync(popupdefault);
            DemoInterestsTxt.Unfocus();
        }

        private void MinAgedropdown_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            if (MinAgedropdown.SelectedIndex > MaxAgeDropDown.SelectedIndex)
            {
                MaxAgeDropDown.SelectedIndex = MinAgedropdown.SelectedIndex;
            }
            if (MinDropDownIos.SelectedIndex > MaxDropDownIos.SelectedIndex)
            {
                MaxAgeDropDown.SelectedIndex = MinAgedropdown.SelectedIndex;
            }
        }

        private void MaxAgeDropDown_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            if (MinAgedropdown.SelectedIndex > MaxAgeDropDown.SelectedIndex)
            {
                MinAgedropdown.SelectedIndex = MaxAgeDropDown.SelectedIndex;
            }

            if (MinDropDownIos.SelectedIndex > MaxDropDownIos.SelectedIndex)
            {
                MinDropDownIos.SelectedIndex = MaxDropDownIos.SelectedIndex;
            }

        }


        private void AddExampleLocIntTag()
        {
            if (m_viewmodel.DemoInterestsList.Count == 0)
            {
                DemflexLayout.Children.Add(new CustomControl.TagViewControl() { TagTitle = "Ice Cream", Tagkey = "6003278094599" });

                m_viewmodel.DemoInterestsList.Add(new FBInterestResult
                {
                    audience_size = 202057980,
                    country = null,
                    country_access = null,
                    description = null,
                    id = "6003278094599",
                    name = "Ice cream",
                    partner = null,
                    source = null,
                    topic = null,
                    type = null
                });
            }

            if (m_viewmodel.TargetLocList.Count == 0)
            {
                //LocflexLayout.Children.Add(new CustomControl.TagViewControl() { TagTitle = "Sydney", Tagkey = "114925" });

                //m_viewmodel.TargetLocList.Add(new FBCitiesResult
                //{
                //    country_code = "AU",
                //    country_name = "Australia",
                //    key = "114925",
                //    geo_hierarchy_level = "",
                //    geo_hierarchy_name = "",
                //    name = "Sydney",
                //    region = "New South Wales",
                //    region_id = 131,
                //    supports_city = true,
                //    supports_region = true,
                //    type = "city"
                //});
            }
        }


        private void AddPreviousSelectedData()
        {

            if (Helper.imageCampaign.DemoInterest != null && Helper.CreateCampType.ToLower() == "image")
            {

                foreach (var element in Helper.imageCampaign.DemoInterest)
                {
                    DemflexLayout.Children.Add(new TagViewControl() { TagTitle = element.ModifiedName, Tagkey = element.id });
                    m_viewmodel.DemoInterestsList.Add(element);
                }

            }

            if(Helper.videoCampaign.DemoInterest != null && Helper.CreateCampType.ToLower() == "video")
            {
                foreach (var element in Helper.videoCampaign.DemoInterest)
                {
                    DemflexLayout.Children.Add(new TagViewControl() { TagTitle = element.ModifiedName, Tagkey = element.id });
                    m_viewmodel.DemoInterestsList.Add(element);
                }
            }


            if (Helper.imageCampaign.TargetLocation != null && Helper.CreateCampType.ToLower() == "image")
            {

                foreach (var element in Helper.imageCampaign.TargetLocation)
                {
                    LocflexLayout.Children.Add(new TagViewControl() { TagTitle = element.ModifiedName, Tagkey = element.key });
                    m_viewmodel.TargetLocList.Add(element);
                }

            }

            if (Helper.videoCampaign.TargetLocation != null && Helper.CreateCampType.ToLower() == "video")
            {

                foreach (var element in Helper.videoCampaign.TargetLocation)
                {
                    LocflexLayout.Children.Add(new TagViewControl() { TagTitle = element.ModifiedName, Tagkey = element.key });
                    m_viewmodel.TargetLocList.Add(element);
                }

            }


            // set previous selected gender

            if (Helper.imageCampaign.Gender?.ToUpper() == "M" || Helper.videoCampaign.Gender?.ToUpper() == "M")
            {
                GenderMale.TextColor = Color.White;
                FGenderMale.BackgroundColor = Color.FromHex("#AC47ED");
                OtherGender.TextColor = Color.FromHex("#AC47ED");
                FAllGender.BackgroundColor = Color.White;
                GenderFemale.TextColor = Color.FromHex("#AC47ED");
                FGenderFemale.BackgroundColor = Color.White;
                // Selected Gender
                m_viewmodel.SelectedGender = "M";
            }
            else if (Helper.imageCampaign.Gender?.ToUpper() == "F" || Helper.videoCampaign.Gender?.ToUpper() == "F")
            {
                GenderFemale.TextColor = Color.White;
                FGenderFemale.BackgroundColor = Color.FromHex("#AC47ED");
                OtherGender.TextColor = Color.FromHex("#AC47ED");
                FAllGender.BackgroundColor = Color.White;
                GenderMale.TextColor = Color.FromHex("#AC47ED");
                FGenderMale.BackgroundColor = Color.White;
                // Selected Gender
                m_viewmodel.SelectedGender = "F";
            }
            else
            {
                OtherGender.TextColor = Color.White;
                FAllGender.BackgroundColor = Color.FromHex("#AC47ED");
                GenderMale.TextColor = Color.FromHex("#AC47ED");
                FGenderMale.BackgroundColor = Color.White;
                GenderFemale.TextColor = Color.FromHex("#AC47ED");
                FGenderFemale.BackgroundColor = Color.White;
                // Selected Gender
                m_viewmodel.SelectedGender = "A";
            }
        }

        void PostflexLayout_ChildRemoved(System.Object sender, Xamarin.Forms.ElementEventArgs e)
        {
            try
            {
                var a = (TagViewControl)e.Element;
                var ToBeDeleted = m_viewmodel.TargetPostList.FirstOrDefault(x => x.key == a.Tagkey);
                m_viewmodel.TargetPostList.Remove(ToBeDeleted);
                m_viewmodel.PostCodeResultChanged = ToBeDeleted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async void TargetPostTxt_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            //if (m_viewmodel != null)
            //{
            //    if (m_viewmodel.TargetLocList != null)
            //    {
            //        if (m_viewmodel.TargetLocList.Count == 0)
            //        {
            //            var popupnav = new UserDialogPopup("Failed", "Please select locations", "Ok");
            //            await PopupNavigation.Instance.PushAsync(popupnav);
            //            return;
            //        }
            //    }
            //}
            var popupdefault = new PostcodeSearchListView(m_viewmodel.TargetLocList.ToList());
            await PopupNavigation.Instance.PushAsync(popupdefault);
            TargetPostTxt.Unfocus();
        }
    }
}
