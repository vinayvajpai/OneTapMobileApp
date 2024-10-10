using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FBAccIdListPopup : PopupPage
    {
        FBAccIdListPopupViewModel m_viewmodel;
        //private object fromConnectAccountView;

        public FBAccIdListPopup(List<FBAccIdListResult> result)
        {
            InitializeComponent();

            BindingContext = m_viewmodel = new FBAccIdListPopupViewModel();
            if (m_viewmodel != null)
            {
                m_viewmodel.AdAccountsList = new ObservableCollection<FBAccIdListResult>(result);
                Helper.FBcustomerAccList = result;

            }
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Send<object, bool>(this, "ChangeIsTapToFalse", false);
            base.OnDisappearing();
        }


        //public FBAccIdListPopup(List<FBAccIdListResult> result, bool fromConnectAccountView)
        //{
        //    InitializeComponent();
        //    BindingContext = m_viewmodel = new FBAccIdListPopupViewModel();
        //    if (m_viewmodel != null)
        //    {
        //        //m_viewmodel.FromConnectAccountView = fromConnectAccountView;
        //        m_viewmodel.AdAccountsList = new ObservableCollection<FBAccIdListResult>(result);
        //        Helper.FBcustomerAccList = result;

        //    }
        //}
    }
}