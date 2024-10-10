using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using OneTapMobile.Views.ErrorPage;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class CreativeCampaignViewModel : BaseViewModel
    {
        #region properties

        public INavigation nav;

        private bool _ImageRightArrow = true;
        public bool ImageRightArrow
        {
            get
            {
                return _ImageRightArrow;
            }
            set
            {
                _ImageRightArrow = value;
                OnPropertyChanged("ImageRightArrow");
            }
        }

        private bool _VideoRightArrow = true;
        public bool VideoRightArrow
        {
            get
            {
                return _VideoRightArrow;
            }
            set
            {
                _VideoRightArrow = value;
                OnPropertyChanged("VideoRightArrow");
            }
        }

        private bool _KeywordsRightArrow = true;
        public bool KeywordsRightArrow
        {
            get
            {
                return _KeywordsRightArrow;
            }
            set
            {
                _KeywordsRightArrow = value;
                OnPropertyChanged("KeywordsRightArrow");
            }
        }

        private bool _ShowVideoTick = false;
        public bool ShowVideoTick
        {
            get
            {
                return _ShowVideoTick;
            }
            set
            {
                _ShowVideoTick = value;
                OnPropertyChanged("ShowVideoTick");
            }
        }

        private bool _ShowImageTick = false;
        public bool ShowImageTick
        {
            get
            {
                return _ShowImageTick;
            }
            set
            {
                _ShowImageTick = value;
                OnPropertyChanged("ShowImageTick");
            }
        }

        private bool _ShowKeyTick = false;
        public bool ShowKeyTick
        {
            get
            {
                return _ShowKeyTick;
            }
            set
            {
                _ShowKeyTick = value;
                OnPropertyChanged("ShowKeyTick");
            }
        }

        private string _VideoFrmBorder = "#ffffff";

        public string VideoFrmBorder
        {
            get { return _VideoFrmBorder; }
            set
            {
                _VideoFrmBorder = value;
                OnPropertyChanged("VideoFrmBorder");
            }
        }

        private string _ImageFrmBorder = "#ffffff";

        public string ImageFrmBorder
        {
            get { return _ImageFrmBorder; }
            set
            {
                _ImageFrmBorder = value;
                OnPropertyChanged("ImageFrmBorder");
            }
        }

        private string _KeyFrmBorder = "#ffffff";

        public string KeyFrmBorder
        {
            get { return _KeyFrmBorder; }
            set
            {
                _KeyFrmBorder = value;
                OnPropertyChanged("KeyFrmBorder");
            }
        }

        #endregion

        #region Commands

        private Command videoOptionCmd;

        public ICommand VideoOptionCmd
        {
            get
            {
                if (videoOptionCmd == null)
                {
                    videoOptionCmd = new Command(PerformVideoOptionCmd);
                }

                return videoOptionCmd;
            }
        }

        private Command imageOptionCmd;

        public ICommand ImageOptionCmd
        {
            get
            {
                if (imageOptionCmd == null)
                {
                    imageOptionCmd = new Command(PerformImageOptionCmd);
                }

                return imageOptionCmd;
            }
        }

        private Command keywordsOptionCmd;

        public ICommand KeywordsOptionCmd
        {
            get
            {
                if (keywordsOptionCmd == null)
                {
                    keywordsOptionCmd = new Command(PerformKeywordsOptionCmd);
                }

                return keywordsOptionCmd;
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
        #endregion
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
        private void PerformKeywordsOptionCmd()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                if (Helper.profileModel.google_ad_customer_id != null)
                {
                    KeywordsRightArrow = false;
                    ShowKeyTick = true;
                    ShowVideoTick = false;
                    ShowImageTick = false;
                    VideoRightArrow = true;
                    ImageRightArrow = true;
                    KeyFrmBorder = "#AC47ED";
                    ImageFrmBorder = "#00000000";
                    VideoFrmBorder = "#00000000";
                    Helper.CreateCampType = "keywords";
                    nav.PushAsync(new AddKeywordCampWebSiteView());
                }
                else
                {
                    IsTap = false;
                    var popupnav = new UserDialogPopup("Message", "Please Add Google Ad Account before Creating campaign. ", "Ok");
                    PopupNavigation.Instance.PushAsync(popupnav);
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }

        private void PerformImageOptionCmd()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                if (Helper.facebookProfile != null && Helper.imageCampaign != null)
                {
                    if (!string.IsNullOrWhiteSpace(Helper.imageCampaign.facebook_page_id))
                    {
                        ImageRightArrow = false;
                        ShowImageTick = true;
                        ShowVideoTick = false;
                        ShowKeyTick = false;
                        VideoRightArrow = true;
                        KeywordsRightArrow = true;
                        ImageFrmBorder = "#AC47ED";
                        KeyFrmBorder = "#00000000";
                        VideoFrmBorder = "#00000000";
                        Helper.CreateCampType = "image";
                        nav.PushAsync(new UploadImageView());
                    }
                    else
                    {
                        IsTap = false;
                        var popupnav = new UserDialogPopup("Message", "Please Add Facebook Ad Account before Creating campaign. ", "Ok");
                        PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;

                    }
                }
                else
                {
                    IsTap = false;
                    var popupnav = new UserDialogPopup("Message", "Please Add Facebook Ad Account before Creating campaign. ", "Ok");
                    PopupNavigation.Instance.PushAsync(popupnav);
                    IsBusy = false;

                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private void PerformVideoOptionCmd()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                if (Helper.facebookProfile != null && Helper.videoCampaign != null)
                {
                    if (!string.IsNullOrWhiteSpace(Helper.videoCampaign.facebook_page_id))
                    {
                        VideoRightArrow = false;
                        ShowVideoTick = true;
                        ShowImageTick = false;
                        ShowKeyTick = false;
                        KeywordsRightArrow = true;
                        ImageRightArrow = true;
                        VideoFrmBorder = "#AC47ED";
                        KeyFrmBorder = "#00000000";
                        ImageFrmBorder = "#00000000";
                        Helper.CreateCampType = "video";
                        nav.PushAsync(new UploadVideoView());
                    }
                    else
                    {
                        IsTap = false;
                        var popupnav = new UserDialogPopup("Message", "Please Add Facebook Ad Account before Creating campaign. ", "Ok");
                        PopupNavigation.Instance.PushAsync(popupnav);
                        IsBusy = false;
                    }
                }
                else
                {
                    IsTap = false;
                    var popupnav = new UserDialogPopup("Message", "Please Add Facebook Ad Account before Creating campaign. ", "Ok");
                    PopupNavigation.Instance.PushAsync(popupnav);
                    IsBusy = false;
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
    }
}
