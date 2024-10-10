using Android.Content;

using System.ComponentModel;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OneTapMobile.CustomControl;
using OneTapMobile.Droid.Renderers;
using System;
using System.Globalization;

[assembly: ExportRenderer(typeof(GradientLabel), typeof(GradientLabelRenderer))]

namespace OneTapMobile.Droid.Renderers
{
    public class GradientLabelRenderer : LabelRenderer
    {
        public GradientLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            SetColors();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetColors();
        }

        private void SetColors()
        {

           var c1 = (Element as GradientLabel).TextColor1.WithSaturation(50).WithLuminosity(0.55).ToAndroid();
           var c2 = (Element as GradientLabel).TextColor2.WithSaturation(20).ToAndroid();

            Shader myShader = new LinearGradient(
                0, 0, Control.MeasuredWidth, 0,
                c1, c2,
                Shader.TileMode.Clamp);

            Control.Paint.SetShader(myShader);
            Control.Invalidate();
        }
    }
}