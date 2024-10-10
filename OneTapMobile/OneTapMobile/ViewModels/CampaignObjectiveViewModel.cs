using OneTapMobile.Global;
using OneTapMobile.Views;
using OneTapMobile.Views.ErrorPage;
using System;
using OneTapMobile.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class CampaignObjectiveViewModel : BaseViewModel
    {
        #region properties

       public INavigation nav;

        private bool _ShowMoreVisibility = true;
        public bool ShowMoreVisibility
        {
            get
            {
                return _ShowMoreVisibility;
            }
            set
            {
                _ShowMoreVisibility = value;
                OnPropertyChanged("ShowMoreVisibility");
            }
        } 
        
        private bool _ShowGetMoreLead = true;
        public bool ShowGetMoreLead
        {
            get
            {
                return _ShowGetMoreLead;
            }
            set
            {
                _ShowGetMoreLead = value;
                OnPropertyChanged("ShowGetMoreLead");
            }
        }
        
        private bool _ShowGetWebClicks = true;
        public bool ShowGetWebClicks
        {
            get
            {
                return _ShowGetWebClicks;
            }
            set
            {
                _ShowGetWebClicks = value;
                OnPropertyChanged("ShowGetWebClicks");
            }
        } 
        
        private bool _ShowBrandTick = false;
        public bool ShowBrandTick
        {
            get
            {
                return _ShowBrandTick;
            }
            set
            {
                _ShowBrandTick = value;
                OnPropertyChanged("ShowBrandTick");
            }
        }

        private bool _ShowTrafficTick = false;
        public bool ShowTrafficTick
        {
            get
            {
                return _ShowTrafficTick;
            }
            set
            {
                _ShowTrafficTick = value;
                OnPropertyChanged("ShowTrafficTick");
            }
        }

        private bool _ShowConvTick = false ;
        public bool ShowConvTick
        {
            get
            {
                return _ShowConvTick;
            }
            set
            {
                _ShowConvTick = value;
                OnPropertyChanged("ShowConvTick");
            }
        }

        private string _BrandBorder = "#ffffff";

        public string BrandBorder
        {
            get { return _BrandBorder; }
            set
            {
                _BrandBorder = value;
                OnPropertyChanged("BrandBorder");
            }
        }

        private string _TrafficBorder = "#ffffff";

        public string TrafficBorder
        {
            get { return _TrafficBorder; }
            set
            {
                _TrafficBorder = value;
                OnPropertyChanged("TrafficBorder");
            }
        }

        private string _ConversionsBorder = "#ffffff";

        public string ConversionsBorder
        {
            get { return _ConversionsBorder; }
            set
            {
                _ConversionsBorder = value;
                OnPropertyChanged("ConversionsBorder");
            }
        }



        // TrafficBorder

        //ConversionsBorder

        #endregion

        #region Command
        private Command backCommand;

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
        private Command helpCircleCommand;

        public ICommand HelpCircleCommand
        {
            get
            {
                if (helpCircleCommand == null)
                {
                    helpCircleCommand = new Command(HelpCircle);
                }

                return helpCircleCommand;
            }
        }

        private Command brandOptionCmd;

        public ICommand BrandOptionCmd
        {
            get
            {
                if (brandOptionCmd == null)
                {
                    brandOptionCmd = new Command(PerformBrandOptionCmd);
                }

                return brandOptionCmd;
            }
        }

        private Command trafficOptionCmd;

        public ICommand TrafficOptionCmd
        {
            get
            {
                if (trafficOptionCmd == null)
                {
                    trafficOptionCmd = new Command(PerformTrafficOptionCmd);
                }

                return trafficOptionCmd;
            }
        }

        private Command conversationOptionCmd;

        public ICommand ConversationOptionCmd
        {
            get
            {
                if (conversationOptionCmd == null)
                {
                    conversationOptionCmd = new Command(PerformConversationOptionCmd);
                }

                return conversationOptionCmd;
            }
        }

        private Command helpGuideCmd;

        public ICommand HelpGuideCmd
        {
            get
            {
                if (helpGuideCmd == null)
                {
                    helpGuideCmd = new Command(PerformHelpGuideCmd);
                }

                return helpGuideCmd;
            }
        }

        #endregion

        #region back button method
        public void PerformBackBtn()
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



        private void HelpCircle()
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

       

        private void PerformBrandOptionCmd()
        {
            try
            {
                if (IsTap)
                    return;
                    IsTap = true;

                    //green tick visible and hide text
                    ShowBrandTick = true;
                    ShowMoreVisibility = false;
                    //other two option need to reset
                    ShowTrafficTick = false;
                    ShowConvTick = false;
                    ShowGetWebClicks = true;
                    ShowGetMoreLead = true;
                    TrafficBorder = "#ffffff";
                    ConversionsBorder = "#ffffff";
                    BrandBorder = "#AC47ED";
                    Helper.imageCampaign.Objective = "Brand";
                    Helper.videoCampaign.Objective = "Brand";
                    Helper.keywordCampaign.Objective = "Brand";
                    nav.PushAsync(new CreativeCampaignView());

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
            }
        }



        private void PerformTrafficOptionCmd()
        {
            try
            {
                if (IsTap)
                    return;
                else
                {
                    IsTap = true;
                    //green tick visible and hide text
                    ShowTrafficTick = true;
                    ShowGetWebClicks = false;
                    //other two option need to reset
                    ShowConvTick = false;
                    ShowBrandTick = false;
                    ShowMoreVisibility = true;
                    ShowGetMoreLead = true;
                    BrandBorder = "#ffffff";
                    ConversionsBorder = "#ffffff";
                    TrafficBorder = "#AC47ED";
                    Helper.imageCampaign.Objective = "Traffic";
                    Helper.videoCampaign.Objective = "Traffic";
                    Helper.keywordCampaign.Objective = "Traffic";
                    nav.PushAsync(new CreativeCampaignView());
                    
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
            }

        }

       

        private void PerformConversationOptionCmd()
        {
            try
            {
                if (IsTap)
                    return;
                else
                {
                    IsTap = true;
                    //green tick visible and hide text
                    ShowConvTick = true;
                    ShowGetMoreLead = false;
                    //other two option need to reset
                    ShowTrafficTick = false;
                    ShowBrandTick = false;
                    ShowMoreVisibility = true;
                    ShowGetWebClicks = true;
                    TrafficBorder = "#ffffff";
                    BrandBorder = "#ffffff";
                    ConversionsBorder = "#AC47ED";
                    Helper.imageCampaign.Objective = "Conversation";
                    Helper.videoCampaign.Objective = "Conversation";
                    Helper.keywordCampaign.Objective = "Conversation";
                    nav.PushAsync(new CreativeCampaignView()); 
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsTap = false;
            }
           

        }


        private void PerformHelpGuideCmd()
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

    }
}
