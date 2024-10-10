using OneTapMobile.Global;
using OneTapMobile.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class StartCampaignViewModel:BaseViewModel
    {
        #region properties

        public INavigation nav;

        #endregion

        #region Commands

        //private Command _NextCommand;
        //public Command NextCommand
        //{
        //    get { return _NextCommand ?? (_NextCommand = new Command(() => NextButtonPressed())); }
        //}

        private ICommand _BackCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_BackCommand == null)
                {
                    _BackCommand = new Command(PerformBackBtn);
                }
                return _BackCommand;
            }
        }

        #endregion

        #region Next Button Pressed Method
        //private void NextButtonPressed()
        //{
        //    if (IsTap)
        //        return;
        //    else
        //    {
        //        IsTap = true;
        //        try
        //        {
        //            nav.PushAsync(new CampaignObjectiveView());
        //            IsTap = false;
        //        }
        //        catch (Exception ex)
        //        {
        //            IsTap = false;
        //            Debug.WriteLine(ex.Message);
        //        }
        //    }

        //}

        #endregion

        #region back button method
        private void PerformBackBtn()
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

    }
}
