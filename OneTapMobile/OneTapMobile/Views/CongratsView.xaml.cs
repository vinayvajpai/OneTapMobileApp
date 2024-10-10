using OneTapMobile.Global;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace OneTapMobile.Views
{
    public partial class CongratsView : ContentPage
    {
        public CongratsView()
        {
            InitializeComponent();
        }
        async protected override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(1000);
            Constant.FbAdPageadded = true;
            Constant.FbRightArrowVisible = false;
            Constant.SkipNowVisisble = true;
            await Navigation.PopToRootAsync();
            MessagingCenter.Send<object, bool>(this, "RefreshCampaignList", true);
            //App.Current.MainPage = new NavigationPage(new DashBoard());
        }
    }
}
