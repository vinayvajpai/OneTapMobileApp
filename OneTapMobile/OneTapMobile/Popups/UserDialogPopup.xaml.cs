using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDialogPopup : PopupPage
    {
        public event EventHandler eventOK;
        public event EventHandler eventCancel;
        public UserDialogPopup()
        {
            InitializeComponent();
        }

        public UserDialogPopup(string TitleText, string DetailText, bool ShowCancelBtn, bool  ShowOkBtn, string btnSuccess, string btnFail)
        {
            InitializeComponent();
            Heading.Text = TitleText;
            Detail.Text = DetailText;
            btnOKAY.Text = btnSuccess;
            btnCancel.Text = btnFail;
            RightButton.IsVisible = ShowCancelBtn;
            LeftButton.IsVisible = ShowOkBtn;
            if (ShowCancelBtn == false)
            {
                OkCancelBtnGroup.ColumnDefinitions.Clear();
                this.CloseWhenBackgroundIsClicked = true;
            }
            this.CloseWhenBackgroundIsClicked = false;
        }

        public UserDialogPopup(string TitleText, string DetailText, string btnFail)
        {
            InitializeComponent();
            Heading.Text = TitleText;
            Detail.Text = DetailText;
            btnCancel.Text = btnFail;
            RightButton.IsVisible = false;
            LeftButton.IsVisible = true;
            OkCancelBtnGroup.ColumnDefinitions[0].Width = 0;
            OkCancelBtnGroup.ColumnDefinitions[1].Width = new GridLength(10, GridUnitType.Star);
            OkCancelBtnGroup.ColumnSpacing = 0;
            this.CloseWhenBackgroundIsClicked = true;
        }

        public UserDialogPopup(string TitleText, string DetailText)
        {
            InitializeComponent();
            Heading.Text = TitleText;
            Detail.Text = DetailText;
            RightButton.IsVisible = false;
            LeftButton.IsVisible = false;
            this.CloseWhenBackgroundIsClicked = true;
        }

        public UserDialogPopup(string TitleText, string DetailText, bool IsOkayVisible)
        {
            InitializeComponent();
            Heading.Text = TitleText;
            Detail.Text = DetailText;
            RightButton.IsVisible = IsOkayVisible;
            LeftButton.IsVisible = false;
            OkCancelBtnGroup.ColumnDefinitions[0].Width = 0;
            OkCancelBtnGroup.ColumnDefinitions[1].Width = new GridLength(10, GridUnitType.Star);
            OkCancelBtnGroup.ColumnSpacing = 0;
            if(!IsOkayVisible)
             this.CloseWhenBackgroundIsClicked = true;
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
                eventOK.Invoke(sender,e);
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