using Foundation;
using OneTapMobile.Interface;
using System;
using Xamarin.Forms;
using UIKit;
using OneTapMobile.iOS.Service;

[assembly: Dependency(typeof(iOSKeyboardHelper))]

namespace OneTapMobile.iOS.Service
{
    public class iOSKeyboardHelper : IKeyboardHelper
    {
        public void HideKeyboard()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }
    }
}