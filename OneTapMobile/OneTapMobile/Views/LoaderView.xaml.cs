using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoaderView : ContentPage
    {
        public LoaderView(string IconName,string DetailText )
        {
            InitializeComponent();
        }
    }
}