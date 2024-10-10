using System;
using System.Collections.Generic;
using OneTapMobile.Models;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace OneTapMobile.Popups
{
    public partial class CurrencyTimeZonePopup : PopupPage
    {
        Image imgChk;
        CurrencyZoonViewModel vm;
        
        public event EventHandler<string> eventDone;
        public CurrencyTimeZonePopup(bool IsCurrency=false)
        {
            InitializeComponent();
            BindingContext = vm = new CurrencyZoonViewModel(IsCurrency);
            if (IsCurrency)
            {
                Heading.Text = "Select Currency";
                currencyList.IsVisible = true;
            }
            else
            {
                Heading.Text = "Select TimeZone";
                timeZoneList.IsVisible = true;
            }

        }


        void SelectTimeZone_Tapped(System.Object sender, System.EventArgs e)
        {
            var timeZoneValue = Convert.ToString(((TappedEventArgs)e).Parameter);
            var grid = (Grid)sender;
            if (imgChk != null)
            {
                imgChk.Source = "RightTickGray";
            }
            imgChk = (Image)grid.Children[1];
            imgChk.Source = "RightTickGreen";
            vm.Selectedvalue = timeZoneValue;
        }

        void SelectCurrency_Tapped(System.Object sender, System.EventArgs e)
        {
            var currencyModel = (CurrencyModel)((TappedEventArgs)e).Parameter;
            var grid = (Grid)sender;
            if (imgChk != null)
            {
                imgChk.Source = "RightTickGray";
            }
            imgChk = (Image)grid.Children[1];
            imgChk.Source = "RightTickGreen";
            vm.Selectedvalue = currencyModel.cc;
        }

        void Done_Tapped(System.Object sender, System.EventArgs e)
        {
            if (eventDone != null)
            {
                eventDone.Invoke(sender, vm.Selectedvalue);
            }
            PopupNavigation.Instance.PopAsync();
        }
    }
}

