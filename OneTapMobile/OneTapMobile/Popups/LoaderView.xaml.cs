using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OneTapMobile.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoaderView : ContentView
    {
        public LoaderView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty LoadderTextProperty =
  BindableProperty.Create("EventName", typeof(string), typeof(LoaderView), null,propertyChanged: OnTitleChanged);

        public string LoadderText
        {
            get { return (string)GetValue(LoadderTextProperty); }
            set { SetValue(LoadderTextProperty, value); }
        }


        static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var loaderView = (LoaderView)bindable;
            loaderView.LoaderTxt.Text = Convert.ToString(newValue);
            // Property changed implementation goes here
        }
    }
}