using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
	public partial class WebViewPopup : PopupPage
	{
		public WebViewPopup (string WebPagestring)
		{
			InitializeComponent ();
			var browser = new WebView();
			var htmlSource = new HtmlWebViewSource();
			htmlSource.Html = WebPagestring;
			WebPageView.Source = htmlSource;
		}

        private void OkButtonPressed(object sender, EventArgs e)
        {
			PopupNavigation.Instance.PopAsync();
			MessagingCenter.Send<object, bool>(this, "ChangeIsTapToFalse", false);
		}
    }
}