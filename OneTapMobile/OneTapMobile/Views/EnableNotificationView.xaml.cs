using OneTapMobile.Interface;
using Xamarin.Forms;
namespace OneTapMobile.Views
{
    public partial class EnableNotificationView : ContentPage
    {
        public EnableNotificationView()
        {
            InitializeComponent();
        }
        void EnableNotification_Tapped(System.Object sender, System.EventArgs e)
        {
            DependencyService.Get<IHandleNotification>().EnablePush();
            this.Navigation.PushAsync(new CongratsView());
        }
        void SkipForNow_Tapped(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new CongratsView());
        }
    }
}
