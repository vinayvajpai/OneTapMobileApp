using Controls.ImageCropper;
using OneTapMobile.Global;
using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditImageView : ContentPage
    {
        EditImageViewModel m_viewmodel;
        public EditImageView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new EditImageViewModel();
           
            if (Helper.imageCampaign != null)
            {
                if (Helper.imageCampaign.Image != null)
                {
                    m_viewmodel.CroppedImage = Helper.imageCampaign.Image;
                    IsBusy = false;
                }
            }

            //MessagingCenter.Subscribe<object, object>(this, "Refresh", (sender, args) =>
            //{
            //    RefreshImage();

            //});
        }

        //private void RefreshImage()
        //{
        //    Selectedimage.ReloadImage();
        //    Selectedimage.LoadingPlaceholder = null;
        //}

        protected override void OnAppearing()
        {
            try
            {
                IsBusy = true;
                if (m_viewmodel != null)
                {
                    m_viewmodel.IsTap = false;
                    m_viewmodel.IsBusy = false;
                    m_viewmodel.IsChecking = false;
                    m_viewmodel.nav = this.Navigation;
                    IsBusy = false;
                }
                base.OnAppearing();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        //void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        //{
        //   // m_viewmodel.OnPinchUpdated(e);
        //}

        //void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        //{
        //    //m_viewmodel.OnPanUpdated(e);
        //}

        private async void Btn16X9_Tapped(object sender, EventArgs e)
        {
            try
            {
                //Selectedimage.HeightRequest = 180;
                //Selectedimage.WidthRequest = 320;
               // var vardata = await myimage.GetImageAsPngAsync(180, 320);//This is the image stream from the cropped transformation

               await ImageCropper.Current.Crop(
                    new CropSettings()
                    {
                        CropShape = CropSettings.CropShapeType.Rectangle,
                        PageTitle = "EDIT IMAGE",
                        AspectRatioX = 16,
                        AspectRatioY = 9,
                    },Helper.UserImagePath).ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            var ex = t.Exception;
                            //alert user
                        }
                        else if (t.IsCanceled)
                        {
                            //do nothing
                        }
                        else if (t.IsCompleted)
                        {
                            var result = t.Result;
                            var vardata = File.ReadAllBytes(t.Result);
                            m_viewmodel.CroppedImage = ImageSource.FromStream(() => new MemoryStream(vardata));
                            Helper.imageCampaign.Image = m_viewmodel.CroppedImage;
                            Helper.CampimageByte = vardata;
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }

        }

        private async void CropSquare_Tapped(object sender, EventArgs e)
        {
            try
            {
                //Selectedimage.HeightRequest = 180;
                //Selectedimage.WidthRequest = 180;
                // var vardata = await Selectedimage.GetImageAsPngAsync(180, 180);//This is the image stream from the cropped transformation

                await ImageCropper.Current.Crop(
                   new CropSettings()
                   {
                       CropShape = CropSettings.CropShapeType.Rectangle,
                       PageTitle = "EDIT IMAGE",
                       AspectRatioX = 16,
                       AspectRatioY = 16,
                   }, Helper.UserImagePath).ContinueWith(t =>
                   {
                       if (t.IsFaulted)
                       {
                           var ex = t.Exception;
                           //alert user
                       }
                       else if (t.IsCanceled)
                       {
                           //do nothing
                       }
                       else if (t.IsCompleted)
                       {
                           var result = t.Result;
                           var vardata = File.ReadAllBytes(t.Result);
                           m_viewmodel.CroppedImage = ImageSource.FromStream(() => new MemoryStream(vardata));
                           Helper.imageCampaign.Image = m_viewmodel.CroppedImage;
                           Helper.CampimageByte = vardata;
                       }
                   });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}