using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategoryPopup : PopupPage
    {
        public event EventHandler<string> eventOK;
        public event EventHandler eventCancel;
       //public string newcategory;
        public AddCategoryPopup()
        {
            InitializeComponent();
        }
        public AddCategoryPopup(string TitleText, string detailText, string btnSuccess, string btnFail)
        {
            InitializeComponent();
            heading.Text = TitleText;
            txtOther.Text = detailText;
            btnOKAY.Text = btnSuccess;
            btnCancel.Text = btnFail;
            RightButton.IsVisible = true;
            LeftButton.IsVisible = true;
            this.CloseWhenBackgroundIsClicked = false;
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                IsBusy = false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void OkButtonPressed(object sender, EventArgs e)
        {
            if (eventOK != null)
            {
                eventOK.Invoke(sender,Convert.ToString(txtOther.Text));
                PopupNavigation.Instance.PopAsync();
            }
        }

        private void CancelButtonPressed(object sender, EventArgs e)
        {
            if (eventCancel != null)
            {
                eventCancel.Invoke(sender, e);
            }
            PopupNavigation.Instance.PopAsync();
        }
    }
}
