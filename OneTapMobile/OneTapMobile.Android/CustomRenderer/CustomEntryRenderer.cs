using Android.Graphics.Drawables;
using Android.Util;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using OneTapMobile.CustomControl;
using System.ComponentModel;
using Android.Views;
using OneTapMobile.Droid.Renderer;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellRenderer))]
[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace OneTapMobile.Droid.Renderer
{

    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var view = (CustomEntry)Element;
                if (view.IsCurvedCornersEnabled)
                {
                    // creating gradient drawable for the curved background  
                    var _gradientBackground = new GradientDrawable();
                    _gradientBackground.SetShape(ShapeType.Rectangle);
                    _gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                    // Thickness of the stroke line  
                    _gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

                    // Radius for the curves  
                    _gradientBackground.SetCornerRadius(
                        DpToPixels(this.Context, Convert.ToSingle(view.CornerRadius)));

                    // set the background of the   
                    Control.SetBackground(_gradientBackground);
                    Control.SetPadding(
                   (int)DpToPixels(this.Context, Convert.ToSingle(12)), Control.PaddingTop,
                   (int)DpToPixels(this.Context, Convert.ToSingle(12)), Control.PaddingBottom);
                }
                if (view.ShowSingleLine)
                {

                    var gd = new GradientDrawable();
                    gd.SetShape(ShapeType.Line);
                    gd.SetColor(view.BackgroundColor.ToAndroid());
                    //gd.SetCornerRadius(5);
                    gd.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());
                    Control.SetBackgroundDrawable(gd);

                    // _gradientBackground.SetShape(ShapeType.Rectangle);
                    // _gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                    // // Thickness of the stroke line  
                    //_gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

                    // // Radius for the curves  
                    // //_gradientBackground.SetCornerRadius(
                    //   //  DpToPixels(this.Context, Convert.ToSingle(view.CornerRadius)));

                    // // set the background of the   
                    // Control.SetBackground(_gradientBackground);
                    Control.SetPadding(
                   (int)DpToPixels(this.Context, Convert.ToSingle(12)), 0,
                   (int)DpToPixels(this.Context, Convert.ToSingle(12)), (int)DpToPixels(this.Context, Convert.ToSingle(25)));
                }
                if (view.NoLine)
                {
                    var _gradientBackground = new GradientDrawable();
                    // _gradientBackground.SetShape(ShapeType.Rectangle);
                    _gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                    // Thickness of the stroke line  
                    //  _gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

                    // Radius for the curves  
                    //_gradientBackground.SetCornerRadius(
                    //  DpToPixels(this.Context, Convert.ToSingle(view.CornerRadius)));

                    // set the background of the   
                    Control.SetBackground(_gradientBackground);
                    Control.SetPadding(
                  (int)DpToPixels(this.Context, Convert.ToSingle(12)), Control.PaddingTop,
                  (int)DpToPixels(this.Context, Convert.ToSingle(12)), Control.PaddingBottom);
                }
                if (e.NewElement.Keyboard == Keyboard.Numeric && view.ShowSpecialChar)
                {
                    //this.Control.KeyListener = DigitsKeyListener.GetInstance(true, true); // I know this is deprecated, but haven't had time to test the code without this line, I assume it will work without
                    //   this.Control.InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagSigned | Android.Text.InputTypes.NumberFlagDecimal;
                }

            }
        }
        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }
    }

    public class ExtendedViewCellRenderer : ViewCellRenderer
    {

        private Android.Views.View _cellCore;
        private Drawable _unselectedBackground;
        private bool _selected;

        protected override Android.Views.View GetCellCore(Cell item,
                                                          Android.Views.View convertView,
                                                          ViewGroup parent,
                                                          Context context)
        {
            _cellCore = base.GetCellCore(item, convertView, parent, context);

            _selected = false;
            _unselectedBackground = _cellCore.Background;

            return _cellCore;
        }

        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);

            if (args.PropertyName == "IsSelected")
            {
                _selected = !_selected;

                if (_selected)
                {
                    var extendedViewCell = sender as ViewCell;
                    _cellCore.SetBackgroundColor(global::Android.Graphics.Color.LightGreen);
                }
                else
                {
                    _cellCore.SetBackground(_unselectedBackground);
                }
            }
        }
    }
}