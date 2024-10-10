using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.ViewModels;
using OneTapMobile.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPopup : PopupPage
    {
        ListPopupViewModel m_viewmodel;


        public ListPopup(List<GoCustomerListResult> result)
        {
            try
            {
                InitializeComponent();

                BindingContext = m_viewmodel = new ListPopupViewModel();
                if (m_viewmodel != null)
                {
                    m_viewmodel.AdAccountsList = new ObservableCollection<GoCustomerListResult>(result);
                    Helper.GocustomerAccList = result;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }

        }

        public ListPopup(List<GoCustomerListResult> result , bool FromNogoogleaccountview)
        {
            try
            {
                InitializeComponent();

                BindingContext = m_viewmodel = new ListPopupViewModel();
                if (m_viewmodel != null)
                {
                    m_viewmodel.AdAccountsList = new ObservableCollection<GoCustomerListResult>(result);
                    Helper.GocustomerAccList = result;
                    m_viewmodel.FromNogoogleaccountview = FromNogoogleaccountview;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }

        }





        void SubAccount_Tapped(object sender,EventArgs e)
        {
            try
            {
                var selectedChildAcc = (ChildAccount)((TappedEventArgs)e).Parameter;
                m_viewmodel.SelectedChildAccount = selectedChildAcc;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
    }
}