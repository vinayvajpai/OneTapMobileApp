using Xamarin.Forms;
using Android.OS;
using OneTapMobile.Droid.Service;
using OneTapMobile.Interface;
[assembly: Dependency(typeof(CloseApplication))]
namespace OneTapMobile.Droid.Service
{
    public class CloseApplication :ICloseApplication
    {
        public void CloseApp()
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}