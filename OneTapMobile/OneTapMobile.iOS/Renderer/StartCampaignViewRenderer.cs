using Foundation;
using OneTapMobile.iOS.Renderer;
using OneTapMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FullScreenVideoView), typeof(StartCampaignViewRenderer))]
namespace OneTapMobile.iOS.Renderer
{
    public class StartCampaignViewRenderer : PageRenderer
    {
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            UIDevice.CurrentDevice.SetValueForKey(NSNumber.FromNInt((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
        }
    }
}