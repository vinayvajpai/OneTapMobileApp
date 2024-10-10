using OneTapMobile.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.WelcomePopups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnboardingPopup : ContentView
    {
        public OnboardingPopup()
        {
            InitializeComponent();
            FirstWcView.IsVisible = true;
            SecondWcView.IsVisible = false;
            ThirdWcView.IsVisible = false;
        }

        private void SkipTapped(object sender, EventArgs e)
        {
            FirstWcView.IsVisible = false;
            SecondWcView.IsVisible = false;
            ThirdWcView.IsVisible = false;
            Helper.SavePropertyData("WelcomeView", false);
            MessagingCenter.Send<object, bool>(this, "WelcomeView", false);
        }

        private void FirstNextTapped(object sender, EventArgs e)
        {
            FirstWcView.IsVisible = false;
            SecondWcView.IsVisible = true;
            ThirdWcView.IsVisible = false;
            if(!Application.Current.Properties.ContainsKey("WelcomeView"))
            {
                Helper.SavePropertyData("WelcomeView", false);
            }
        }

        private void SecondNextTapped(object sender, EventArgs e)
        {
            FirstWcView.IsVisible = false;
            SecondWcView.IsVisible = false;
            ThirdWcView.IsVisible = true;
            if (!Application.Current.Properties.ContainsKey("WelcomeView"))
            {
                Helper.SavePropertyData("WelcomeView", false);
            }
        }
    }
}