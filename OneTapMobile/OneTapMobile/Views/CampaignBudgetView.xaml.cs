using Acr.UserDialogs;
using OneTapMobile.Global;
using OneTapMobile.LocalDataBase;
using OneTapMobile.LocalDataBases.DataBaseModel;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CampaignBudgetView : ContentPage
    {
        CampaignBudgetViewModel m_viewmodel;

        InitDatabaseTable db = new InitDatabaseTable();

        public CampaignBudgetView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new CampaignBudgetViewModel();
            StartDatePicker.MinimumDate = DateTime.Now;
            EndDatePicker.MinimumDate = StartDatePicker.Date.AddDays(1);
            m_viewmodel.PropertyChanged += M_viewmodel_PropertyChanged;
        }

        private void M_viewmodel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (m_viewmodel != null)
            {
                if (m_viewmodel.StartDate != "Choose Date" && m_viewmodel.EndDate != "Choose Date" && m_viewmodel.BudgetTxt != string.Empty)
                {
                    ContinueFrame.BackgroundColor = Color.FromHex("#AC47ED");
                    ContinueFrame.IsEnabled = true;
                    ContinueFrame.Opacity = 1.0;
                    ContinueBtnText.Opacity = 1.0;
                }
                else
                {
                    ContinueFrame.BackgroundColor = Color.FromHex("#BE7CE9");
                    ContinueFrame.IsEnabled = false;
                    ContinueFrame.Opacity = 0.8;
                    ContinueBtnText.Opacity = 0.5;

                }
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
                    if (!string.IsNullOrWhiteSpace(Helper.profileModel.currency))
                    {
                        if (Helper.profileModel.currency.ToUpper() == "INR")
                        {
                            CurrencyCode.Text = "₹";
                        }
                        else
                        {
                            CurrencyCode.Text = "$";
                        }
                    }
                    else
                    {
                        CurrencyCode.Text = "$";
                    }

                    var previousImageCamp = db.Connection.Table<ImageCampaignModel>();
                    var previousVideoCamp = db.Connection.Table<VideoCampaignModel>();
                    var previouskeywordCamp = db.Connection.Table<KeywordCampaignModel>();

                    if (Helper.CreateCampType.ToLower() == "image")
                    {

                        if (previousImageCamp != null)
                        {

                            if (!string.IsNullOrWhiteSpace(Helper.imageCampaign.Budget))
                            {
                                m_viewmodel.EndDate = Helper.imageCampaign.EndCampDate;
                                m_viewmodel.StartDate = Helper.imageCampaign.StartCampDate;
                                m_viewmodel.BudgetTxt = Helper.imageCampaign.Budget;
                                if (Helper.imageCampaign.BudgetType.ToLower() != "lifetime")
                                {
                                    m_viewmodel.PerformDailyOptionCmd();
                                }
                                else
                                {
                                    m_viewmodel.PerformLifetimeOptionCmd();
                                }
                            }
                        }
                    }

                    if (Helper.CreateCampType.ToLower() == "video")
                    {

                        if (previousVideoCamp != null)
                        {
                            if (!string.IsNullOrWhiteSpace(Helper.videoCampaign.Budget))
                            {
                                m_viewmodel.EndDate = Helper.videoCampaign.EndCampDate;
                                m_viewmodel.StartDate = Helper.videoCampaign.StartCampDate;
                                m_viewmodel.BudgetTxt = Helper.videoCampaign.Budget;
                                if (Helper.videoCampaign.BudgetType.ToLower() != "lifetime")
                                {
                                    m_viewmodel.PerformDailyOptionCmd();
                                }
                                else
                                {
                                    m_viewmodel.PerformLifetimeOptionCmd();
                                }
                            }
                        }

                    }


                    if (previouskeywordCamp != null && Helper.CreateCampType.ToLower() == "keywords")
                    {
                        if (!string.IsNullOrWhiteSpace(Helper.keywordCampaign.Budget))
                        {
                            m_viewmodel.EndDate = Helper.keywordCampaign.EndCampDate;
                            m_viewmodel.StartDate = Helper.keywordCampaign.StartCampDate;
                            m_viewmodel.BudgetTxt = Helper.keywordCampaign.Budget;
                            if (Helper.keywordCampaign.BudgetType.ToLower() != "lifetime")
                            {
                                m_viewmodel.PerformDailyOptionCmd();
                            }
                            else
                            {
                                m_viewmodel.PerformLifetimeOptionCmd();
                            }
                        }
                    }

                    m_viewmodel.IsTap = false;
                    m_viewmodel.nav = this.Navigation;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void EndDatePicked(object sender, EventArgs e)
        {
            EndDatePicker.Focus();
        }

        private void StartDatePicked(object sender, EventArgs e)
        {
            StartDatePicker.Focus();
        }

        private void StartDatePicker_Unfocused(object sender, FocusEventArgs e)
        {
            var StartDate = (DatePicker)sender;
            m_viewmodel.StartDate = StartDate.Date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            Helper.imageCampaign.StartCampDate = m_viewmodel.StartDate;
            Helper.videoCampaign.StartCampDate = m_viewmodel.StartDate;
            Helper.keywordCampaign.StartCampDate = m_viewmodel.StartDate;
        }

        private void EndDatePicker_Unfocused(object sender, FocusEventArgs e)
        {
            var endDate = (DatePicker)sender;
            m_viewmodel.EndDate = endDate.Date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            Helper.imageCampaign.EndCampDate = m_viewmodel.EndDate;
            Helper.videoCampaign.EndCampDate = m_viewmodel.EndDate;
            Helper.keywordCampaign.EndCampDate = m_viewmodel.EndDate;
        }

        private void BudgetTxt_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                if (BudgetTxt.Text.Contains("."))
                {
                    var popupnav = new UserDialogPopup("Message", "budget value should be integer number", "Ok");
                    PopupNavigation.Instance.PushAsync(popupnav);
                    BudgetTxt.Text = e.NewTextValue.Replace(".", "");
                    return;
                }
            }
        }
    }
}