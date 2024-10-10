using Acr.UserDialogs;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using OneTapMobile.Views.ErrorPage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.ViewModels
{
    public class UploadImageViewModel : BaseViewModel
    {
        #region properties
        public byte[] ImageByte { get; set; }
        public ImageSource UserImage { get; set; }

        public INavigation nav;

        #endregion

        #region commands

        private ICommand _UploadImageClicked;
        public ICommand UploadImageClicked
        {
            get
            {
                if (_UploadImageClicked == null)
                {
                    _UploadImageClicked = new Command(UploadImageClickedMethod);
                }

                return _UploadImageClicked;
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

        #region methods
        private void Help()
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


        private async void UploadImageClickedMethod(object obj)
        {
            try
            {
                if(PopupNavigation.Instance.PopupStack.Count == 0)
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

        private void Popupdefault_eventGallery(object sender, EventArgs e)
        {
            UploadImage_Gallery();
        }

        private void Popupdefault_eventCamera(object sender, EventArgs e)
        {
            UploadImage_Camera();
        }

        async void UploadImage_Camera()
        {
            try
            {
                //Check Camera Availablety in Device
                if (!CrossMedia.Current.IsCameraAvailable)
                {

                    UserDialogs.Instance.Alert("No Camera", " No Camera Available", "Ok");
                    IsTap = false;
                    return;
                }

                //Get Picture on Camera
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    //PhotoSize = PhotoSize.Full,
                    DefaultCamera = CameraDevice.Rear,
                    AllowCropping = true,
                    SaveMetaData = false,
                    RotateImage = false,
                });

                if (file == null)
                {
                    IsTap = false;
                    return;
                }

                Helper.UserImagePath = file.Path;

                // Get Image as Stream Format
                Stream stream = file.GetStream();


                // this below condition is only for ios camera clicked photo which is rotating automatically.
                if (file != null && Device.RuntimePlatform == Device.iOS)
                {
                    stream = file.GetStreamWithImageRotatedForExternalStorage();
                    stream.Seek(0, SeekOrigin.Begin);
                }
                byte[] imageData;

                //Convert Image Stream to Byte Array  
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                imageData = ms.ToArray();
                Helper.CampimageByte = imageData;
                UserImage = ImageSource.FromStream(() => new MemoryStream(imageData));
                //Helper.UserImage = UserImage;
                Helper.imageCampaign.CroppedImage = UserImage;
                Helper.imageCampaign.Image = UserImage;
                await PopupNavigation.Instance.PopAsync();
                IsBusy = false;
                await nav.PushAsync(new EditImageView());

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsBusy = false;
                IsTap = false;
            }
            finally
            {
                // Dispose of the Java side bitmap.
                GC.Collect();
                IsBusy = false;
            }
        }

        async void UploadImage_Gallery()
        {
            try
            {
                //Check Gallery Availablety in Device
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {

                    UserDialogs.Instance.Alert("No Image", "No Image Available in gallery", "Ok");
                    IsTap = false;
                    return;
                }

                //Get Picture on Gallery
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = PhotoSize.Full
                });

                if (file == null)
                {
                    IsTap = false;
                    return;
                }
                    

                IsBusy = false;

                Helper.UserImagePath = file.Path;

                // Get Image as Stream Formate
                var stream = file.GetStream();
                byte[] imageData;

                //Convert Image Stream to Byte Array  
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                imageData = ms.ToArray();
                Helper.CampimageByte = imageData;
                UserImage = ImageSource.FromStream(() => new MemoryStream(imageData));
                //Helper.UserImage = UserImage;
                Helper.imageCampaign.Image = UserImage;
                Helper.imageCampaign.CroppedImage = UserImage;
                IsBusy = false;
                await PopupNavigation.Instance.PopAsync();
                await nav.PushAsync(new EditImageView());


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsBusy = false;
                IsTap = false;
            }
            finally
            {
                // Dispose of the Java side bitmap.
                GC.Collect();
                IsBusy = false;
            }
        }

        #endregion
    }
}
