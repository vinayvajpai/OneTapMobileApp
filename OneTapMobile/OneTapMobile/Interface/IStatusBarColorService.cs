using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Interface
{
    public interface IStatusBarColorService
    {
        void SetDefaultTheme();
        void SetTheme(string ColorCode);
        void SetLightTheme();
        void SetDarkTheme();

        void HideStatusBar();
        void ShowStatusBar();
    }
}
