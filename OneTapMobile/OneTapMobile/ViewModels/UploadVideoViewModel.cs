using OneTapMobile.Global;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using OneTapMobile.Views.ErrorPage;
using Plugin.Media;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{

    public class UploadVideoViewModel : BaseViewModel
    {
        #region properties
        public INavigation nav;
        public FileResult  UserVideo { get; set; }

        public FileResult SelectedVideo;

        #endregion

        #region command

        private Command _BacButtonCommand;
        public ICommand BacButtonCommand
        {
            get
            {
                if (_BacButtonCommand == null)
                {
                    _BacButtonCommand = new Command(BackBtnMethod);
                }

                return _BacButtonCommand;
            }
        }
        
        private Command _HelpBtnCommand;
        public ICommand HelpBtnCommand
        {
            get
            {
                if (_HelpBtnCommand == null)
                {
                    _HelpBtnCommand = new Command(HelpBtnMethod);
                }

                return _HelpBtnCommand;
            }
        }

        private Command _UploadVideoClicked;
        public ICommand UploadVideoClicked
        {
            get
            {
                if (_UploadVideoClicked == null)
                {
                    _UploadVideoClicked = new Command(UploadVideoClickedMethod);
                }

                return _UploadVideoClicked;
            }
        }

        #endregion

        #region methods

        private async void UploadVideoClickedMethod()
        {
            try
            {
                if (PopupNavigation.Instance.PopupStack.Count == 0)
                {
                    IsTap = false;
                }

                if (IsTap)
                    return;
                IsTap = true;

                var popupdefault = new CameraGalleryPopup();
                await PopupNavigation.Instance.PushAsync(popupdefault);
                popupdefault.eventCamera += Popupdefault_eventCamera;
                popupdefault.eventGallery += Popupdefault_eventGallery;
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                // Dispose of the Java side bitmap.
                GC.Collect();
            }
        }

        private async void  Popupdefault_eventGallery(object sender, EventArgs e)
        {
            try
            {
                var SelectedVideo = await CrossMedia.Current.PickVideoAsync();

                if (SelectedVideo != null)
                {
                    try
                    {
                        FileResult fileResult = new FileResult(SelectedVideo.Path);
                        fileResult.ContentType = "video/quicktime";
                        
                        Helper.videoCampaign.Video = fileResult;
                        await nav.PushAsync(new SelectVideoThumbView(fileResult));
                        await PopupNavigation.Instance.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        IsTap = false;
                        Debug.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private async void Popupdefault_eventCamera(object sender, EventArgs e)
        {

            try
            {
                var pickResult = await MediaPicker.CaptureVideoAsync();
                if (pickResult != null)
                {
                    try
                    {
                        Helper.videoCampaign.Video = pickResult;
                        await nav.PushAsync(new SelectVideoThumbView(pickResult));
                        await PopupNavigation.Instance.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        IsTap = false;
                        Debug.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }
        private void BackBtnMethod()
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
        private void HelpBtnMethod()
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
        #endregion

    }
}
