using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Popups;
using OneTapMobile.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashBoard : ContentPage
    {
        DashBoardViewModel v_model;
        public DashBoard()
        {
            try
            {
                InitializeComponent();
                BindingContext = v_model = new DashBoardViewModel();
                if (v_model != null)
                {
                    v_model.GetCampaignData();
                }
                //MessagingCenter.Subscribe<object>(this, "Refreshdata", (sender) =>
                //{
                //    if(Convert.ToBoolean(sender))
                //    v_model.GetCampaignData();
                //});
            }
            catch (Exception ex)
            {
                v_model.IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }

        protected async override void OnAppearing()
        {
            try
            {

                ContainerGrid.Margin = DependencyService.Get<INotchService>().HasNotch() ? new Thickness(10, 50) : new Thickness(10, 20);

                base.OnAppearing();
                if (v_model != null)
                {
                    var DeviceHeight = Application.Current.MainPage.Height;
                    var DeviceWidth = Application.Current.MainPage.Width;
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        var expectedHeight = (DeviceHeight + DeviceWidth) / 8;
                        if (expectedHeight > DeviceWidth)
                        {
                            v_model.ProgGraphHeight = 140;
                        }
                        else
                        {
                            v_model.ProgGraphHeight = expectedHeight;
                        }
                    }
                    v_model.nav = this.Navigation;
                    v_model.IsTap = false;

                    MessagingCenter.Subscribe<object, bool>(this, "RefreshCampaignList", (sender, arg) =>
                    {
                        if(Convert.ToBoolean(arg))
                        v_model.GetCampaignData();
                    });

                }

            }
            catch (Exception ex)
            {
                v_model.IsTap = false;
                Debug.WriteLine(ex.Message);
            }           
        }

        private void StartNewCampClicked(object sender, EventArgs e)
        {
            try
            {
                if (v_model.IsTap)
                    return;
                v_model.IsTap = true;
                Navigation.PushAsync(new CampaignObjectiveView());
            }
            catch (Exception ex)
            {
                v_model.IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                if (v_model.IsTap)
                    return base.OnBackButtonPressed();

                v_model.IsTap = true;
                var popupnav = new UserDialogPopup("Confirm", "Are you sure you want to Exit?", true, true, "OK", "Cancel");
                popupnav.eventOK += Popupnav_eventOK;
                popupnav.eventCancel += Popupnav_eventCancel;
                PopupNavigation.Instance.PushAsync(popupnav);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                v_model.IsTap = false;
            }
            return base.OnBackButtonPressed();
        }

        private void Popupnav_eventCancel(object sender, EventArgs e)
        {
            v_model.IsTap = false;
        }
        private void Popupnav_eventOK(object sender, EventArgs e)
        {
            v_model.IsTap = false;
            Constant.IsLoggedOut = true;
            Helper.facebookProfile = null;
            Constant.FbAdPageadded = false;
            Constant.FbRightArrowVisible = true;
            Constant.SkipNowVisisble = false;
            Helper.ResetLoginData();
            App.Current.MainPage = new NavigationPage(new LoginView());

        }
    }
}