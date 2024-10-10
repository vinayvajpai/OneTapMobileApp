using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content.PM;
namespace OneTapMobile.Droid
{
    //[Activity(Theme = "@style/MyTheme.Splash", Icon = "@drawable/OneTapLogo", Label = "OneTap", MainLauncher = false, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //public class SplashActivity : Activity
    //{
    //    public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
    //    {
    //        base.OnCreate(savedInstanceState, persistentState);

    //        // Create your application here
    //    }

    //    protected override void OnResume()
    //    {
    //        base.OnResume();
    //        Task task = new Task(() => { Waittilltime(); });
    //        task.Start();
    //    }

    //     void Waittilltime()
    //    {
    //       // await Task.Delay(1000);
    //        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    //    }
    //}
}