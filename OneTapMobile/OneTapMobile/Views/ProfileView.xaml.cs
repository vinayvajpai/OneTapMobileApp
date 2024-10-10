using OneTapMobile.Global;
using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileView : ContentPage
    {
        ProfileViewModel m_viewmodel;
        public ProfileView()
        {
            InitializeComponent();
            BindingContext = m_viewmodel = new ProfileViewModel();
           
          //  MessagingCenter.Unsubscribe<object, bool>(this, "RefreshFbAdAccountIddata");
            MessagingCenter.Subscribe<object,bool>(this, "RefreshFbAdAccountIddata", (sender,arg) =>
            {
                if (arg)
                {
                    if(m_viewmodel != null)
                    {
                        if (Helper.profileModel != null)
                        {
                            m_viewmodel.FBAccountId = Helper.profileModel.fb_ad_account_id;
                            m_viewmodel.GoogleCustomerId = Helper.profileModel.google_ad_customer_id;
                            m_viewmodel.FBSpent = Convert.ToString(Helper.profileModel.facebook_spent);
                            m_viewmodel.GoogleSpent = Convert.ToString(Helper.profileModel.google_spent);
                            m_viewmodel.TotalSpent = Convert.ToString(Helper.profileModel.amount_spent);
                            if(!string.IsNullOrWhiteSpace(Helper.profileModel.UserName))
                            m_viewmodel.UserName = Helper.profileModel.UserName;

                        }
                        m_viewmodel.IsBusy = false;
                    }
                }
            });

            MessagingCenter.Subscribe<object, bool>(this, "ChangeIsTapToFalse", (sender, arg) =>
            {
                m_viewmodel.IsTap = false;
                m_viewmodel.IsBusy = false;
            });

            //Getlocation();
        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (m_viewmodel != null)
                {
                    m_viewmodel.nav = this.Navigation;
                    m_viewmodel.IsTap = false;
                    m_viewmodel.UserName = Helper.profileModel.UserName;
                }
                   
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        //private async void Getlocation()
        //{
        //    try
        //    {
        //        var position = await Geolocation.GetLocationAsync();
        //        IEnumerable<Placemark> possibleAddressess = await Geocoding.GetPlacemarksAsync(position.Latitude, position.Longitude);
        //        if (possibleAddressess != null)
        //        {
        //            var placemarkDetails = possibleAddressess?.FirstOrDefault();
        //            string address1 = possibleAddressess.FirstOrDefault().AdminArea.ToString();
        //            string address2 = possibleAddressess.FirstOrDefault().SubAdminArea.ToString();
        //            m_viewmodel.UserLocation = address2 + ", " + address1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //}
    }
}