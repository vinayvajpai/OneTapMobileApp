using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraGalleryPopup : PopupPage
    {
        public event EventHandler eventCamera;
        public event EventHandler eventGallery;

        public CameraGalleryPopup()
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<object, bool>(this, "ChangeIsTapToFalse", false);
        }

        //private void PopupCancelCommand(object sender, EventArgs e)
        //{
        //    Navigation.PopAsync();
        //}

        private void CameraCommand(object sender, EventArgs e)
        {
            if (eventCamera != null)
            {
                eventCamera.Invoke(sender, e);
            }
        }

        private void GalleryCommand(object sender, EventArgs e)
        {
            if (eventGallery != null)
            {
                eventGallery.Invoke(sender, e);
            }

        }
    }
}