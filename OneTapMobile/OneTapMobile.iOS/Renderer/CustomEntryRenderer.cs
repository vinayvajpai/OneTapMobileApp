using System;
using System.Drawing;
using CoreAnimation;
using CoreGraphics;
using OneTapMobile.CustomControl;
using OneTapMobile.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(CustomAllViewCellRendereriOS))]
[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace OneTapMobile.iOS.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var view = (CustomEntry)Element;
                e.NewElement.SizeChanged += (obj, args) =>
                {
                    if (Element == null)
                        return;
                    if (this.Element.Keyboard == Keyboard.Numeric)
                        this.AddDoneButton();

                    if (view.IsCurvedCornersEnabled)
                    {
                        Control.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
                        Control.LeftViewMode = UITextFieldViewMode.Always;

                        Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
                        Control.ReturnKeyType = UIReturnKeyType.Done;
                        // Radius for the curves  
                        Control.Layer.CornerRadius = Convert.ToSingle(view.CornerRadius);
                        // Thickness of the Border Color  
                        Control.Layer.BorderColor = view.BorderColor.ToCGColor();
                        // Thickness of the Border Width  
                        Control.Layer.BorderWidth = view.BorderWidth;
                        Control.ClipsToBounds = true;
                    }
                    if (view.IsIcon)
                    {
                        Control.LeftView = new UIView(new CGRect(0f, 0f, 40f, 20f));
                        Control.LeftViewMode = UITextFieldViewMode.Always;
                    }

                    if (view.ShowSingleLine)
                    {
                        // get native control (UITextField)
                        var entry = this.Control;

                        // Create borders (bottom only)
                        CALayer border = new CALayer();
                        float width = 0.0f;
                        border.BorderColor = UIColor.White.CGColor; //new CoreGraphics.CGColor(0.73f, 0.7451f, 0.7647f);  // gray border color
                        border.Frame = new CGRect(x: 0, y: view.Height - 10, width: view.Width, height: 1.0f);
                        border.BorderWidth = width;

                        entry.Layer.AddSublayer(border);

                        entry.Layer.MasksToBounds = true;
                        entry.BorderStyle = UITextBorderStyle.None;
                        //entry.BackgroundColor = new UIColor(1, 1, 1, 1); // white
                    }

                    //if (view.NoLine)
                    //{
                        Control.BorderStyle = UIKit.UITextBorderStyle.None;
                        //var borderLayer = new CALayer();
                        //borderLayer.Frame = new CoreGraphics.CGRect(0f, Frame.Height+3, Frame.Width, 1f);
                        //borderLayer.BorderColor = UIColor.White.CGColor;
                        //borderLayer.BorderWidth = 1.0f;
                        //borderLayer.CornerRadius = 1;
                        //Control.Layer.AddSublayer(borderLayer);
                    //}
                };

                if (e.NewElement.Keyboard == Keyboard.Numeric && view.ShowSpecialChar)
                {
                    Control.KeyboardType = UIKeyboardType.NumbersAndPunctuation;
                }
            }
        }

        private void AddDoneButton()
        {
            var toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));

            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
            {
                this.Control.ResignFirstResponder();
                var baseEntry = this.Element.GetType();
                ((IEntryController)Element).SendCompleted();
            });

            toolbar.Items = new UIBarButtonItem[] {
                new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
                doneButton
            };
            this.Control.InputAccessoryView = toolbar;
        }
    }


    public class CustomAllViewCellRendereriOS : ViewCellRenderer
    {
        public override UIKit.UITableViewCell GetCell(Cell item, UIKit.UITableViewCell reusableCell, UIKit.UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            if (cell != null)
                cell.SelectionStyle = UIKit.UITableViewCellSelectionStyle.None;
            return cell;
        }
    }
}