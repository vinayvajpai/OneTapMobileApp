using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using OneTapMobile.Droid.Service;
using OneTapMobile.Interface;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidKeyboardHelper))]

namespace OneTapMobile.Droid.Service
{
    public class AndroidKeyboardHelper : IKeyboardHelper
    {
        public void HideKeyboard()
        {
            var context = Android.App.Application.Context;
            var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && context is Activity)
            {
                var activity = context as Activity;
                var token = activity.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                activity.Window.DecorView.ClearFocus();
            }
        }

    }
}